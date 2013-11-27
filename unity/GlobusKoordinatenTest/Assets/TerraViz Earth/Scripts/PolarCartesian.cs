using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Globalization;

public class PolarCartesian : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ViewportPointToRay (new Vector3(0.5f,0.5f,0));
			RaycastHit hit;
		
			if (collider.Raycast (ray, out hit, 10000f)) {
				Debug.Log("hit: " + hit.point.x + " - " + hit.point.y + " - " + hit.point.z);
				
				int w = (int)Mathf.Round(hit.textureCoord.x * 4096);
				int h = (int)Mathf.Round(2048- hit.textureCoord.y * 2048);
				
				Debug.Log("pixel: " + w + " - " + h); 
				
				Vector2 v2 = new Vector2();
				v2.x = w / 4096;
				v2.y = (h - 2048)/2048 * - 1 ;
				Debug.Log("textur coord berechnung: " + v2.x + " - " + v2.y);
				//asdf = UvTo3D(v2);
				//Debug.Log("back: " + asdf.x + " - " + asdf.y + " - " + asdf.z);
				
				Debug.Log("----");

				v2 = PixelXYToLatLong(w, h);
				Debug.Log(v2.x + " - " + v2.y);
				string url = "http://api.geonames.org/countryCode?lat="+v2.x+"&lng="+v2.y+"&username=stefan900";
				string data = getData(url).Trim();
				if(data.Length == 2){
					RegionInfo info = new RegionInfo(data);
					Debug.Log(info.ThreeLetterWindowsRegionName);
				}
			}
		}
	}

	string getData(string url){
		WebRequest g = HttpWebRequest.Create(url);
		HttpWebResponse response = (HttpWebResponse)g.GetResponse();
		Stream dataStream = response.GetResponseStream ();
		StreamReader reader = new StreamReader (dataStream);
		string responseFromServer = reader.ReadToEnd ();
		reader.Close ();
		dataStream.Close ();
		response.Close ();
		return responseFromServer;
	}

	Vector2 PixelXYToLatLong(int pixelX, int pixelY)
	{
		Vector2 vector = new Vector2();
		float longitude = pixelX / (4096/360.0f) - 180;
		float latitude = (pixelY / (2048/180.0f) - 90) * -1;
		vector.x = latitude;
		vector.y = longitude;
		return vector;
	}
	
	Vector2 LatLongToPixelXY(float lat, float lon){
		Vector2 vector = new Vector2();
		vector.x = (4096/360.0f) * (180 + lon);
		vector.y = (2048/180.0f) * (90 - lat);
		return vector;
	}
	
	Vector3 UvTo3D(Vector2 uv) {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		int[] tris = mesh.triangles;
		Vector2[] uvs = mesh.uv;
		Vector3[] verts = mesh.vertices;
		for (int i = 0; i < tris.Length; i += 3){
			Vector2 u1= uvs[tris[i]]; 
			Vector2 u2= uvs[tris[i+1]];
			Vector2 u3= uvs[tris[i+2]];
			
			float a = Area(u1, u2, u3); if (a == 0) continue;
			
			
			float a1= Area(u2, u3, uv)/a; if (a1 < 0) continue;
			float a2 = Area(u3, u1, uv)/a; if (a2 < 0) continue;
			float a3 = Area(u1, u2, uv)/a; if (a3 < 0) continue;
			
			Vector3 p3D = a1*verts[tris[i]]+a2*verts[tris[i+1]]+a3*verts[tris[i+2]];
			
			return transform.TransformPoint(p3D);
		}
		
		return Vector3.zero;
	}
	
	float Area(Vector2 p1, Vector2 p2, Vector2 p3){
		Vector2 v1= p1 - p3;
		Vector2 v2 = p2 - p3;
		return (v1.x * v2.y - v1.y * v2.x)/2;
	}
}
