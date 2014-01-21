using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

using Globetrotter.GuiLayer.ViewModel;
using Globetrotter;
using Globetrotter.InputLayer;

public class CountrySelection : MonoBehaviour
{
	private object m_lockObj = new object();

	private Camera m_mainCamera;

	private CountrySelectorViewModel m_countrySelectorViewModel;

	private GlobeViewModel m_globeViewModel;

	private Dictionary<string, Country> _countries;

	private Vector3 m_rotationVector;
	private Vector2[] m_uv;
	private Vector3[] m_vertices;
	private int[] m_triangles;

	public void Init(GlobeViewModel globeviewmodel, CountrySelectorViewModel countrySelectorViewModel, Camera mainCamera)
	{
		m_globeViewModel = globeviewmodel;
		m_countrySelectorViewModel = countrySelectorViewModel;
		m_countrySelectorViewModel.PropertyChanged -= PropertyChangedHandler;
		m_countrySelectorViewModel.PropertyChanged += PropertyChangedHandler;

		m_mainCamera = mainCamera;

		m_rotationVector = new Vector3(0.0f, 0.0f, 0.0f);
	}

	// Use this for initialization
	void Start () {
		m_firstRun = true;

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		m_uv = new Vector2[mesh.uv.Length];
		for(int i = 0; i < mesh.uv.Length; i++){
			Vector2 v = mesh.uv[i];
			m_uv[i] = new Vector2(v.x, v.y);
		}
		m_vertices = new Vector3[mesh.vertices.Length];
		for(int i = 0; i < mesh.vertices.Length; i++){
			Vector3 v = mesh.vertices[i];
			m_vertices[i] = new Vector3(v.x, v.y, v.z);
		}
		m_triangles = new int[mesh.triangles.Length];
		for(int i = 0; i < mesh.triangles.Length; i++){
			m_triangles[i] = mesh.triangles[i];
		}

		trans = transform;
		qTo = trans.rotation;
		_countries = new Dictionary<string, Country>();

		try{
			string[] lines = File.ReadAllLines (Application.dataPath + "/Data/Resources/borders.csv");

			foreach (string str in lines)
			{
				string[] tmp = str.Split(';');
				List<Polygon> polygons = getPolygons(tmp[0].Replace("\"", string.Empty));
				Country c = new Country(tmp[5], tmp[3], float.Parse(tmp[10], System.Globalization.CultureInfo.InvariantCulture),
				                        float.Parse(tmp[11], System.Globalization.CultureInfo.InvariantCulture), polygons);
				_countries[c.Code] = c;
			}
		} catch (Exception ex){
			Debug.LogError(ex);
		}
	}

	public float speed = 500;
	private Quaternion qTo;
	private Transform trans;

	bool m_rotateToCountry;
	bool m_firstRun;

	// Update is called once per frame
	void Update () {
		if(m_globeViewModel.ReactOnInput){
			Ray ray = Camera.main.ViewportPointToRay (new Vector3(0.5f,0.5f,0));
			RaycastHit hit;
			
			if (collider.Raycast (ray, out hit, 10000f)) {
				//Debug.Log("hit: " + hit.point.x + " - " + hit.point.y + " - " + hit.point.z);

				int w = (int)Mathf.Round(hit.textureCoord.x * 4096);
				int h = (int)Mathf.Round(2048- hit.textureCoord.y * 2048);
				
				Vector2 v = PixelXYToLatLong(w, h);

				Country c = getCountry(v.y, v.x);
				if(c != null){
					int ind = m_countrySelectorViewModel.GetIndexOfCountry(c.Code);
					if(ind != -1){
						m_countrySelectorViewModel.CurrCountryIndex	= ind;
					}
				} else {
					//Debug.Log("NULL");
				}
			}
		} else {
			if(m_firstRun){
				rotate();
				m_firstRun = false;
			}

			if(m_rotateToCountry){
				Vector3 v;

				lock(m_lockObj)
				{
					v = transform.TransformPoint(m_rotationVector);
				}

				if((v.x != 0.0f) && (v.y != 0.0f) && (v.z != 0.0f))
				{
					//Debug.Log("update change");
					qTo = Quaternion.FromToRotation(v - trans.position, m_mainCamera.transform.position - trans.position);
					qTo = qTo * transform.rotation;
					transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, Time.deltaTime * 500);
				}
				
				trans.rotation = Quaternion.RotateTowards (trans.rotation, qTo, Time.deltaTime * speed);


				if(v.x == 0.0f && v.y == 0.0f && v.z == 0.0f){
					m_rotateToCountry = false;
				}
				Ray ray2 = Camera.main.ViewportPointToRay (new Vector3(0.5f,0.5f,0));
				RaycastHit hit2;
				if (collider.Raycast (ray2, out hit2, 10000f)) {
					//Debug.Log("hit: " + hit.textureCoord.x + " - " + hit.textureCoord.y );
					//Debug.Log("uv3d: " + v.x + " - " + v.y + " - " + v.z);

					int w = (int)Mathf.Round(hit2.textureCoord.x * 4096);
					int h = (int)Mathf.Round(2048- hit2.textureCoord.y * 2048);
					
					//Debug.Log("pixel: " + w + " - " + h); 
					
					Vector2 v2 = new Vector2();
					v2.x = w / 4096f;
					v2.y = (h - 2048)/2048f * - 1 ;
					Vector3 v3 = UvTo3D(v2);
					Vector3 v4 = transform.InverseTransformPoint(v);
					//Debug.Log("v3: " + v3.x + " - " + v3.y + " - " + v3.z );
					//Debug.Log("v4: " + v4.x + " - " + v4.y + " - " + v4.z );

					if(Math.Abs(v3.x - v4.x) <= 0.1f && Math.Abs(v3.y - v4.y) <= 0.1f && Math.Abs(v3.z - v4.z) <= 0.1f){
						//Fertig rotiert, wieder freigeben für Steuerung
						m_rotateToCountry = false;
					}
				}
			}
		}
	}

	void OnDestroy()
	{
		m_countrySelectorViewModel.PropertyChanged -= PropertyChangedHandler;
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
		Vector2[] uvs = m_uv;
		Vector3[] verts = m_vertices;
		int[] tris = m_triangles;

		for (int i = 0; i < tris.Length; i += 3){
			Vector2 u1= uvs[tris[i]]; 
			Vector2 u2= uvs[tris[i+1]];
			Vector2 u3= uvs[tris[i+2]];
			
			float a = Area(u1, u2, u3); if (a == 0) continue;
			
			
			float a1= Area(u2, u3, uv)/a; if (a1 < 0) continue;
			float a2 = Area(u3, u1, uv)/a; if (a2 < 0) continue;
			float a3 = Area(u1, u2, uv)/a; if (a3 < 0) continue;
			
			Vector3 p3D = a1*verts[tris[i]]+a2*verts[tris[i+1]]+a3*verts[tris[i+2]];
			
			return p3D;
		}
		
		return Vector3.zero;
	}
	
	float Area(Vector2 p1, Vector2 p2, Vector2 p3){
		Vector2 v1= p1 - p3;
		Vector2 v2 = p2 - p3;
		return (v1.x * v2.y - v1.y * v2.x)/2;
	}

	Country getCountry(float lon, float lat){
		Country country = null;

		foreach (Country c in _countries.Values) {
			if (c.isInCountry (lon, lat)) {
					country = c;
					break;
			}
		}
		return country;
	}

	public void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
	{
		//Debug.Log("property changed");
		if(sender == m_countrySelectorViewModel)
		{
			if(args.PropertyName == "CurrentCountry")
			{
				rotate();
			}
		}
	}

	private void rotate(){
		//Debug.Log("change");
		Country c = null;
		_countries.TryGetValue(m_countrySelectorViewModel.CurrentCountry.IsoAlphaThreeCode, out c);
		
		if(c != null)
		{
			//Debug.Log(c.Longitude + " - " + c.Latitude);
			Vector2 v2 = LatLongToPixelXY(c.Latitude, c.Longitude);
			v2.x = v2.x/ 4096;
			v2.y = (v2.y - 2048) / 2048 * - 1;
			Vector3 vector = UvTo3D(v2);
			//Debug.Log(vector.x+"-"+vector.y+"-"+vector.z);
			
			lock(m_lockObj)
			{
				m_rotationVector = vector;
				m_rotateToCountry = true;
			}
		}
	}

	public class Country
	{
		private float _longitude, _latitude;
		private string _name, _code;
		private List<Polygon> _polygons;
		
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		
		public string Code
		{
			get { return _code; }
			set { _code = value; }
	   }

		public float Latitude
		{
			get { return _latitude; }
			set { _latitude = value; }
		}
		
		public float Longitude
		{
			get { return _longitude; }
			set { _longitude = value; }
		}
		
		public Country(string name, string code, float longitude, float latitude, List<Polygon> polygons)
		{
			_name = name;
			_code = code;
			_longitude = longitude;
			_latitude = latitude;
			_polygons = polygons;
		}
		
		public bool isInCountry(float longitude, float latitude)
		{
			foreach (Polygon polygon in _polygons)
			{
				if (!(longitude < polygon.MinLon || longitude > polygon.MaxLon || latitude < polygon.MinLat || latitude > polygon.MaxLat) && polygon.pnpoly(longitude, latitude))
				{
					return true;
				}
			}
			return false;
		}
		
		
	}
	
	static List<Polygon> getPolygons(string txt)
	{
		txt = Regex.Replace(txt, @"\(+", "(");
		txt = Regex.Replace(txt, @"\)+", ")");
		List<Polygon> polygons = new List<Polygon>();
		//if (txt.StartsWith("MULTIPOLYGON"))
		//{
		//    txt = txt.Substring(txt.IndexOf("(") + 1);
		//    txt = txt.Remove(txt.Length - 1);
		//}
		
		int ind = txt.IndexOf("(");
		while (ind != -1)
		{
			int end = txt.IndexOf(")", ind);
			//if (end != -1)
			//{
			string tmp = txt.Substring(ind + 1, end - ind - 1);
			Polygon pol = new Polygon();
			string[] coordinates = tmp.Split(',');
			List<Polygon.Point> coordinateList = new List<Polygon.Point>();
			foreach (string s in coordinates)
			{
				string[] st = s.Split(' ');
				float lon = float.Parse(st[0], System.Globalization.CultureInfo.InvariantCulture);
				float lat = float.Parse(st[1], System.Globalization.CultureInfo.InvariantCulture);
				//float lon = float.Parse(st[0]);
				//float lat = float.Parse(st[1]);
				coordinateList.Add(new Polygon.Point(lon, lat));
			}
			pol.Points = coordinateList;
			polygons.Add(pol);
			//}
			ind = txt.IndexOf("(", end);
		}
		return polygons;
	}
	
	public class Polygon
	{
		private List<Point> _points;
		private float _minLon, _maxLon, _minLat, _maxLat;
		
		public float MinLon
		{
			get { return _minLon; }
		}
		
		public float MaxLon
		{
			get { return _maxLon; }
		}
		
		public float MinLat
		{
			get { return _minLat; }
		}
		
		public float MaxLat
		{
			get { return _maxLat; }
		}
		
		public List<Point> Points
		{
			get { return _points; }
			set { 
				_points = value;
				_minLon = float.MaxValue;
				_maxLon = float.MinValue;
				_minLat = float.MaxValue;
				_maxLat = float.MinValue;
				foreach (Point p in _points)
				{
					_minLon = Math.Min(p.Longitude, _minLon);
					_minLat = Math.Min(p.Latitude, _minLat);
					_maxLon = Math.Max(p.Longitude, _maxLon);
					_maxLat = Math.Max(p.Latitude, _maxLat);
				}
			}
		}
		
		public Polygon()
		{
			_points = new List<Point>();
		}
		
		
		//http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
		public bool pnpoly(float testx, float testy)
		{
			int i, j;
			bool c = false;
			for (i = 0, j = _points.Count - 1; i < _points.Count; j = i++)
			{
				if (((_points[i].Latitude > testy) != (_points[j].Latitude > testy)) &&
				    (testx < (_points[j].Longitude - _points[i].Longitude) * (testy - _points[i].Latitude) / (_points[j].Latitude - _points[i].Latitude) + _points[i].Longitude))
				{
					c = !c;
				}
			}
			return c;
		}
		
		public class Point
		{
			private float _longitude, _latitude;
			
			public float Latitude
			{
				get { return _latitude; }
				set { _latitude = value; }
			}
			
			public float Longitude
			{
				get { return _longitude; }
				set { _longitude = value; }
			}
			
			public Point(float longitude, float latitude)
			{
				_longitude = longitude;
				_latitude = latitude;
			}
		}
	}
}