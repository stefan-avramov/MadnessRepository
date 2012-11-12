using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World;
using System.ComponentModel;
using System.Windows.Input;
using Common;

namespace World
{
    public static class WorldContainer
    {
        private static IList<GroundObject> groundObjects;
        private static GooseObject goose;

        static WorldContainer()
        {
            Init();
        }

        internal static void Init()
        {
            groundObjects = new List<GroundObject>()
            {
                new GroundObject(0, 100, 600),
                new GroundObject(800, 200, 500),
                new GroundObject(1600, 150, 200),
                new GroundObject(2150, 50, 50),
                new GroundObject(2450, 100, 300)                
            };

            goose = new GooseObject();
        }

        public static IList<GroundObject> GroundObjects
        {
            get
            {
                return groundObjects;
            }
        }

        public static GooseObject Goose
        {
            get
            {
                return goose;
            }
        }

        public static void GameTick()
        {
            Goose.Tick();
        }

        public static void Move(int offset)
        {
            foreach (var groundObject in WorldContainer.GroundObjects)
            {
                groundObject.OffsetX -= offset;
            }
        }
    }
}
