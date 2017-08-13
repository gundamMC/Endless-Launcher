using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EndlessLauncher
{
    class PointDistance
    {
        public static Thickness GetClosestPoint(Thickness Origin, List<Point> PointsList, Thickness GoBack, Button Background)
        {
            Point CloesestPoint = PointsList.OrderBy(x => Math.Pow(x.X - Origin.Left, 2) + Math.Pow(x.Y - Origin.Top, 2)).ToList().First();
            //Square root removed to save performance

            if (Math.Pow(CloesestPoint.X - Origin.Left, 2) + Math.Pow(CloesestPoint.Y - Origin.Top, 2) < 200*200)
                //Use 200^2 instead of square rooting the distance to maximize performance..? I mean.. It is easier to square a number... Right?
                
            {
                Background.Margin = new Thickness(CloesestPoint.X - 55, CloesestPoint.Y - 55, 0, 0);
                return new Thickness(CloesestPoint.X, CloesestPoint.Y, 0, 0);
            }

            //If the distance is larger than 200, cancel it and bring it back
            else
            {
                Background.Margin = new Thickness(GoBack.Left - 55, GoBack.Top - 55, 0, 0);
                return GoBack;
            }

            
        }
    }
}
