using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace EndlessLauncher
{
    class IconPointClass
    {
        public class IconPoint
        {
            public Point Point { get; set; }

            public Boolean Used { get; set; } = false;
        }

        public static List<IconPoint> IconPoints = new List<IconPoint>()
        {
            //1A
            new IconPoint() {Point = new Point(195, 270) },

            //1B
            new IconPoint() {Point = new Point(325, 270) },

            //1C
            new IconPoint() {Point = new Point(505, 270) },

            //1D
            new IconPoint() {Point = new Point(635, 270) },

            //1E
            new IconPoint() {Point = new Point(765, 270) },

            //2A
            new IconPoint() {Point = new Point(195, 420) },

            //2B
            new IconPoint() {Point = new Point(325, 420) },

            //2C
            new IconPoint() {Point = new Point(505, 420) },

            //2D
            new IconPoint() {Point = new Point(635, 420) },

            //2E
            new IconPoint() {Point = new Point(765, 420) }

        };
    }
}
