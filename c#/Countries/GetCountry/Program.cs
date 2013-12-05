using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace GetCountry
{
    class Program
    {
        //Dictionary<

        static void Main(string[] args)
        {
            //_infos = File.ReadAllLines("borders.csv");
            //5 - land
            //    10 - long
            //        11 - lat

            List<Country> countries = new List<Country>();
            foreach (string str in File.ReadAllLines("borders.csv"))
            {
                string[] tmp = str.Split(';');
                List<Polygon> polygons = getPolygons(tmp[0].Replace("\"", string.Empty));
                countries.Add(new Country(tmp[5], float.Parse(tmp[10].Replace('.', ',')), float.Parse(tmp[11].Replace('.', ',')), polygons));
            }
            Console.WriteLine("Longitude: ");
            float lon = float.Parse(Console.ReadLine());
            Console.WriteLine("Latitude: ");
            float lat = float.Parse(Console.ReadLine());
            DateTime dt = DateTime.Now;
            Country country = null;
            foreach (Country c in countries)
            {
                if (c.isInCountry(lon, lat))
                {
                    country = c;
                }
            }
            if (country != null)
            {
                Console.WriteLine(country.Name);
                Console.WriteLine("It took {0} seconds to find the country", (DateTime.Now - dt).TotalSeconds);
            }
            else
            {
                Console.WriteLine("No country found");
            }
            Console.ReadLine();
        }

        public class Country
        {
            private float _longitude, _latitude;
            private string _name;
            private List<Polygon> _polygons;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
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

            public Country(string name, float longitude, float latitude, List<Polygon> polygons)
            {
                _name = name;
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
                List<Program.Polygon.Point> coordinateList = new List<Program.Polygon.Point>();
                foreach (string s in coordinates)
                {
                    string[] st = s.Split(' ');
                    float lon = float.Parse(st[0].Replace('.', ','));
                    float lat = float.Parse(st[1].Replace('.', ','));
                    //float lon = float.Parse(st[0]);
                    //float lat = float.Parse(st[1]);
                    coordinateList.Add(new Program.Polygon.Point(lon, lat));
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
}
