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
        }

        public static void drawPolygon(Hull h)
        {
            Pen p = new Pen(Color.Black);
            g.DrawPolygon(p, h.vertices.ToArray());
            Refresh();
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
            //Our main recursive method.
            //Time Complexity: O(n log(n))
            //Space Complexity: O(n)

            //The base-case where we have fewer than 3 points and can confidently join them without intersection.
            if (pointList.Count < 4)
            {
                //Time: O(1), Space O(1), given that we'll never be working with more than 3 vertices.
                return new Hull(pointList);
            }
            else
            {
                //Recurse down the tree to the base-case.
                //Time: O(log(n))
                Hull left = Solve(pointList.Take(pointList.Count / 2).ToList());
                Hull right = Solve(pointList.Skip(pointList.Count / 2).ToList());

                //Time: O(n), Space O(n)
                Hull combinedHull = Hull.Combine(left, right);
                return combinedHull;
            }
        }
    }
}