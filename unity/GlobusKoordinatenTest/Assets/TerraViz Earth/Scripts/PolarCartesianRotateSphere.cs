using UnityEngine;
using System.Collections;

public class PolarCartesianRotateSphere : MonoBehaviour {
	public float speed = 500;
	private Quaternion qTo;
	private Transform trans;

	// Use this for initialization
	void Start () {
		trans = transform;
		qTo = trans.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			//London
			float lat = 51;
			float lon = 0;
			Vector2 vector = LatLongToPixelXY(lat, lon);
			Vector2 v2 = new Vector2(); 
			v2.x = vector.x / 4096;
			v2.y = (vector.y - 2048)/2048 * - 1 ;
			Vector3 coord3d = UvTo3D(v2);
			
			qTo = Quaternion.FromToRotation(coord3d - trans.position, Camera.main.transform.position - trans.position);
			qTo = qTo * transform.rotation;
		}

		trans.rotation = Quaternion.RotateTowards(trans.rotation, qTo, Time.deltaTime * speed);
	}

	Vector2 PixelXYToLatLong(int pixelX, int pixelY)
	{
		Vector2 vector = new Vector2();
		float longitude = pixelX / (4096/360) - 180;
		float latitude = (pixelY / (2048/180) - 90) * -1;
		vector.x = latitude;
		vector.y = longitude;
		return vector;
	}

	Vector2 LatLongToPixelXY(float lat, float lon){
		Vector2 vector = new Vector2();
		vector.x = (4096/360) * (180 + lon);
		vector.y = (2048/180) * (90 - lat);
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
