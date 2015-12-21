using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BasicGIS;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

            //MapLayer layer1;
            //MapLayer layer2;
            //MapLayer layer3;
            List<MapLayer> layers = new List<MapLayer>();
            MapView mapview;
            double pandist = 2;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            string[] filenames = {};
            if (of.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filenames = of.FileNames;
            }
            if (filenames.Length == 0)
            {
                MessageBox.Show("Choose shpfile!");
            }
            else {
                layers.Clear();
                for(int i = 0; i<filenames.Length; i++)
                {
                    ShapeFileRW shp = new ShapeFileRW(filenames[i]);
                    layers.Add(shp.GetLayer());
                }
                FullExtent();
            }

        }

        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            if (mapview == null) return;
            SimpleMapPoint point = mapview.ToMapP(new Point(e.X,e.Y));
            label1.Text = "X:" + point.x + "  Y:" + point.y;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FullExtent();
        }

        private void FullExtent()
        {
            if (layers.Count > 0)
            {
                mapview = new MapView(layers[0].mapextent.GetCenter(), layers[0].mapextent.GetWidth() / pb.Width, pb.ClientRectangle);
                DrawMap();
                pandist = mapview.scale * 50;
            }
            else
            {
                MessageBox.Show("choose shp before zoom2Extent!");
            }
        }

        private void ZoomIn()
        {
            if (mapview != null)
            {
                mapview.scale = mapview.scale / 2;
                pandist = mapview.scale * 50;
                DrawMap();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ZoomIn();
            
        }

        private void ZoomOut()
        {
            mapview.scale = mapview.scale * 2;
            pandist = mapview.scale * 50;
            DrawMap();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void DrawMap()
        {
            pb.CreateGraphics().Clear(Color.Black);
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].draw(mapview, pb.CreateGraphics());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mapview.MapCenter.y += pandist;
            DrawMap();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mapview.MapCenter.x -= pandist;
            DrawMap();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            mapview.MapCenter.x += pandist;
            DrawMap();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            mapview.MapCenter.y -= pandist;
            DrawMap();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ShapeFileRW shapefilerw3 = new ShapeFileRW(@"data\states.shp");
            layers.Add(shapefilerw3.GetLayer());
            ShapeFileRW shapefilerw1 = new ShapeFileRW(@"data\roads.shp");
            layers.Add(shapefilerw1.GetLayer());
            ShapeFileRW shapefilerw2 = new ShapeFileRW(@"data\cities.shp");
            layers.Add(shapefilerw2.GetLayer());            

            FullExtent();
        }

    }
}
