using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace GooseCore
{
    public class Texture
    {
        public bool TileX { get; set; }
        public bool TileY { get; set; }
        public virtual Image Image { get; set; }
        public virtual Color Color { get; set; }
    }

    public class SceneObject
    {
        public Texture Texture { get; set; }
        public Rectangle Bounds { get; set; }
        public string Name { get; set; }

        public SceneObject()
        {
            this.Name = String.Empty;
            this.Texture = new Texture();
            this.Bounds = Rectangle.Empty;
        }

        public void Update(TimeSpan time)
        {

        }
    }

    public class Scene
    {
        public List<SceneObject> Objects = new List<SceneObject>();

        public PointF Offset { get; set; }
        public SizeF Scale { get; set; }

        public void Update(TimeSpan time)
        {
            foreach (SceneObject sceneObjects in this.Objects)
            {
                sceneObjects.Update(time);
            }
        }
    }

    public static class SceneLoader
    {
        static ColorConverter ColorConverter = new ColorConverter();
        static RectangleConverter RectangleConverter = new RectangleConverter();

        public static Scene LoadScene(string fileName)
        {
            string text = File.ReadAllText(fileName);
            List<string> objectsText = new List<string>();
            StringBuilder sb = new StringBuilder();

            int open = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '{')
                {
                    sb.Clear();
                    open++;
                }
                else if (text[i] == '}')
                {
                    open--;
                    if (open < 0)
                    {
                        throw new FormatException("Unexpected }");
                    }
                    objectsText.Add(sb.ToString());
                }
                else
                {
                    sb.Append(text[i]);
                }
            }
            
            Scene scene = new Scene();

            foreach (string str in objectsText)
            {
                string[] settings = str.Split(';');
                SceneObject scnObj = new SceneObject();
                foreach (string setting in settings)
                {
                    if (string.IsNullOrWhiteSpace(setting))
                    {
                        continue;
                    }

                    string[] values = setting.Split(':');
                    if (values.Length != 2)
                    {
                        throw new FormatException("Bad formatted value");
                    }
                    values[0] = values[0].Trim().ToLower();
                    values[1] = values[1].Trim().ToLower();

                    

                    switch (values[0])
                    {
                        case "name":
                            scnObj.Name = values[1];
                            break;
                        case "color":
                            scnObj.Texture.Color = (Color)ColorConverter.ConvertFromString(values[1]);
                            break;
                        case "bounds":
                            scnObj.Bounds = (Rectangle)RectangleConverter.ConvertFromString(values[1]);
                            break;
                        case "image":
                            scnObj.Texture.Image = Image.FromFile(values[1]);
                            break;
                    }
                }

                scene.Objects.Add(scnObj);
            }

            return scene;
        }
    }
}
