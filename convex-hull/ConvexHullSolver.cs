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
        public static System.Drawing.Graphics g;
        static System.Windows.Forms.PictureBox pictureBoxView;

        public ConvexHullSolver(System.Drawing.Graphics G, System.Windows.Forms.PictureBox PictureBoxView)
        {
            g = G;
            pictureBoxView = PictureBoxView;
        }

        public static void drawLine(Hull.Edge e, bool up = false)
        {
            Pen p;
            if (up)
                p = new Pen(Color.Blue);
            else
                p = new Pen(Color.Coral);

            g.DrawLine(p, e.a, e.b);
            Refresh();
            Pause(100);
        }

        public static void drawPolygon(Hull h)
        {
            Pen p = new Pen(Color.Black);

            g.DrawPolygon(p, h.vertices.ToArray());
            Refresh();
            Pause(100);
        }

        static public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }

        static public void Pause(int milliseconds)
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

                ConvexHullSolver.drawPolygon(left);
                ConvexHullSolver.drawPolygon(right);

                Hull combo = Hull.Combine(left, right);
                drawPolygon(combo);
                return combo;
            }
        }

        public double findSlope(PointF a, PointF b)
        {
            return (a.X - b.X) / (a.Y - b.Y);
        }
    }
}