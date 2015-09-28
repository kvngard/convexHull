using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using _1_convex_hull;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        System.Drawing.Graphics g;
        System.Windows.Forms.PictureBox pictureBoxView;

        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
        }

        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }

        public void Pause(int milliseconds)
        {
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

        public Hull Solve(List<System.Drawing.PointF> pointList)
        {
            Debug.WriteLine(pointList.Count);
            if (pointList.Count < 4)
            {
                return new Hull(pointList);
            }
            else
            { 
                Hull left = Solve(pointList.Take(pointList.Count / 2).ToList());
                Hull right = Solve(pointList.Skip(pointList.Count / 2).ToList());

                return Hull.Combine(left, right);
            }
        }

        public double findSlope(PointF a, PointF b)
        {
            return (a.X - b.X) / (a.Y - b.Y);
        }
    }
}
