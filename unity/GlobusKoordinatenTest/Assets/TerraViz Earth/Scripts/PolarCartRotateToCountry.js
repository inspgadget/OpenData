#pragma strict

var point:Vector3;
var polar:Vector2;
var move:boolean;
var pos:Vector3;
public var speed = 20000.0;
private var qTo : Quaternion;
private var trans : Transform;


function Start () {
	trans = transform;
    qTo = trans.rotation;
}

function Update () {
	//http://answers.unity3d.com/questions/561417/rotate-sphere-based-on-texture-coordinates.html
	if (Input.GetMouseButtonDown(0)){
		//London
		var lat = 51;
		var lon = 0;
		var vector :Vector2;

		vector = LatLongToPixelXY(lat, lon);
		var v2 :Vector2;
		//Debug.Log("London (pixels): " + vector.x + " - " + vector.y); 
		v2.x = vector.x / 4096;
		v2.y = (vector.y - 2048)/2048 * - 1 ;
		var coord3d : Vector3= UvTo3D(v2);
	    
	    qTo = Quaternion.FromToRotation(coord3d - trans.position, Camera.main.transform.position - trans.position);
 		qTo = qTo * transform.rotation;
    }
    
    trans.rotation = Quaternion.RotateTowards(trans.rotation, qTo, Time.deltaTime * speed);
}

function getData(url){
	var www : WWW = new WWW (url);
	yield www;
	Debug.Log(www.data);
}

function PixelXYToLatLong(pixelX: int, pixelY: int) :Vector2
{
	var vector :Vector2;
	var longitude = pixelX / (4096/360.0) - 180;
	var latitude = (pixelY / (2048/180.0) - 90) * -1;
    vector.x = latitude;
    vector.y = longitude;
    var tex : Texture2D = renderer.material.mainTexture;
    
    Debug.Log(latitude + " - " + longitude);
    return vector;
}

function LatLongToPixelXY(lat: float, lon: float):Vector2{
	var vector :Vector2;
	vector.x = (4096/360.0) * (180 + lon);
	vector.y = (2048/180.0) * (90 - lat);
	return vector;
}

function UvTo3D(uv: Vector2): Vector3 {
  var mesh: Mesh = GetComponent(MeshFilter).mesh;
  var tris: int[] = mesh.triangles;
  var uvs: Vector2[] = mesh.uv;
  var verts: Vector3[] = mesh.vertices;
  for (var i: int = 0; i < tris.length; i += 3){
    var u1: Vector2 = uvs[tris[i]]; 
    var u2: Vector2 = uvs[tris[i+1]];
    var u3: Vector2 = uvs[tris[i+2]];

    var a: float = Area(u1, u2, u3); if (a == 0) continue;

    var a1: float = Area(u2, u3, uv)/a; if (a1 < 0) continue;
    var a2: float = Area(u3, u1, uv)/a; if (a2 < 0) continue;
    var a3: float = Area(u1, u2, uv)/a; if (a3 < 0) continue;

    var p3D: Vector3 = a1*verts[tris[i]]+a2*verts[tris[i+1]]+a3*verts[tris[i+2]];

    return transform.TransformPoint(p3D);
  }

  return Vector3.zero;
}

function Area(p1: Vector2, p2: Vector2, p3: Vector2): float {
  var v1: Vector2 = p1 - p3;
  var v2: Vector2 = p2 - p3;
  return (v1.x * v2.y - v1.y * v2.x)/2;
}