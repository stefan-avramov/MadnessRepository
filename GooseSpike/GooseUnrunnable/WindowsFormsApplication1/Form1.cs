using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GooseCore;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Scene scene;
        DateTime start;

        bool shouldReload = false;

        public Form1()
        {
            InitializeComponent();
            scene = SceneLoader.LoadScene("scene.css");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            start = DateTime.Now;
            while (this.Visible)
            {
                if (shouldReload)
                {
                    shouldReload = false;
                    start = DateTime.Now;
                    scene = SceneLoader.LoadScene("scene.css");
                }

                scene.Update(DateTime.Now - start);
                this.Invalidate();
                Application.DoEvents();
                Thread.Sleep(500);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RenderScene(e.Graphics);
        }

        private void RenderScene(Graphics g)
        {
            foreach (SceneObject obj in scene.Objects)
            {
                if (obj.Texture != null)
                {
                    if (obj.Texture.Image != null)
                    {
                        using (TextureBrush brush = new TextureBrush(obj.Texture.Image))
                        {
                            brush.WrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
                            g.FillRectangle(brush, obj.Bounds);
                        }
                    }
                    else
                    {
                        using(SolidBrush brush = new SolidBrush(obj.Texture.Color))
                        {
                            g.FillRectangle(brush, obj.Bounds);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            shouldReload = true;
        }
    }
}
