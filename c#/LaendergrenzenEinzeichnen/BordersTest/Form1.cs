using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace BordersTest
{
    public partial class Form1 : Form
    {
        Image _drawArea;
        string[] _infos;
        int width = 21600;
        int height = 10800;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _infos = File.ReadAllLines("borders.csv");
            //_drawArea = Bitmap.FromFile("world.jpg");
            
            pictureBox1.Image = _drawArea;
            textBox1.Text = "3";
        }


        List<Polygon> getPolygons(string txt)
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
                    List<Point> coordinateList = new List<Point>();
                    foreach (string s in coordinates)
                    {
                        string[] st = s.Split(' ');
                        float lon = float.Parse(st[0].Replace('.', ','));
                        float lat = float.Parse(st[1].Replace('.', ','));
                        //float lon = float.Parse(st[0]);
                        //float lat = float.Parse(st[1]);
                        pol.Points.Add(LatLongToPixelXY(lat, lon));
                    }
                    polygons.Add(pol);
                //}
                    ind = txt.IndexOf("(", end);
            }
            return polygons;
        }

        class Polygon
        {
            List<Point> _points;

            public List<Point> Points
            {
                get { return _points; }
                set { _points = value; }
            }

            public Polygon()
            {
                _points = new List<Point>();
            }
        }

        void DrawBorder()
        {
            _drawArea = Bitmap.FromFile("world.jpg");
            Graphics g = Graphics.FromImage(_drawArea);
            Pen pen = new Pen(Color.Red, int.Parse(textBox1.Text));

            foreach (string line in _infos)
            {
                string[] tmp = line.Split(';');
                List<Polygon> polygons = getPolygons(tmp[0].Replace("\"", string.Empty));
                foreach (Polygon polygon in polygons)
                {
                    List<Point> coordinateList = polygon.Points;
                    for (int i = 0; i < coordinateList.Count - 1; i++)
                    {
                        Point p1 = coordinateList[i];
                        Point p2 = coordinateList[i + 1];
                        g.DrawLine(pen, p1, p2);
                    }
                    g.DrawLine(pen, coordinateList[coordinateList.Count - 1], coordinateList[0]);
                }
            }


            pictureBox1.Image = _drawArea;
            
            g.Dispose();
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private Point LatLongToPixelXY(float lat, float lon){
            Point p = new Point();
            p.X = (int)Math.Round((width / 360.0) * (180 + lon));
            p.Y = (int)Math.Round((height / 180.0) * (90 - lat));
	        return p;
        }

        private void Save()
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
        100L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            _drawArea.Save("world_with_borders.jpg", jgpEncoder, myEncoderParameters);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawBorder();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
