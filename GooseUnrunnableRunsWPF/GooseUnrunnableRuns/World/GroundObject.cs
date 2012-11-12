using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Common;

namespace World
{
    public class GroundObject : ViewModel
    {
        int offsetX;
        readonly int offsetY;
        readonly int height;
        readonly int width;

        public GroundObject(int offsetX, int height, int width)
        {
            this.offsetX = offsetX;
            this.offsetY = GameEnvironment.WindowHeight - height;
            this.height = height;
            this.width = width;
        }

        public int Height
        {
            get 
            {
                return height;
            }
        }

        public int Width
        {
            get 
            {
                return width;
            }
        }

        public int OffsetX
        {
            get
            {
                return offsetX;
            }
            set
            {
                if (offsetX != value)
                {
                    offsetX = value;
                    OnPropertyChanged("OffsetX");
                }
            }
        }

        public int OffsetY
        {
            get
            {
                return offsetY;
            }
        }
    }
}
