using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using _2_convex_hull;

namespace _1_convex_hull
{
    class Hull
    {
        public PointF leftMost { get; set; }
        public PointF rightMost { get; set; }
        public List<PointF> vertices { get; set; }

        public struct Edge
        {
            public PointF a;
            public PointF b;
            public double slope;

            public Edge(PointF a, PointF b)
            {
                this.a = a;
                this.b = b;
                slope = findSlope(a, b);
            }
        }
        public Hull()
        {
            vertices = new List<PointF>();
        }

        public Hull(List<System.Drawing.PointF> pointList)
        {
            leftMost = pointList[0];
            rightMost = pointList.Last();
            this.Order(leftMost, pointList);
            vertices = new List<PointF>(pointList);
        }

        public int getIndex(PointF a)
        {
            return vertices.IndexOf(a);
        }

        public PointF vertexAt(int index)
        {
            return vertices.ElementAt(index);
        }

        public static int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        public static double findSlope(PointF a, PointF b)
        {
            return (b.Y - a.Y) / (b.X - a.X);
        }

        public void Order(PointF leftMost, List<System.Drawing.PointF> pointList)
        {
            Dictionary<double, PointF> slopes = new Dictionary<double, PointF>();
            pointList.Remove(leftMost);

            foreach (PointF a in pointList)
            {
                slopes.Add(findSlope(leftMost, a), a);
            }

            pointList.Clear();

            foreach (var p in slopes.OrderBy(i => i.Key))
                pointList.Add(p.Value);

            pointList.Insert(0, leftMost);
        }

        public static Hull Combine(Hull A, Hull B)
        {
            Hull combined = new Hull();
            Edge upper = findUpper(A, B);
            Edge lower = findLower(A, B);

            combined.leftMost = A.leftMost;
            combined.rightMost = B.rightMost;
            int i = B.getIndex(lower.b);
            int j = A.getIndex(upper.a);

            while (!combined.vertices.Contains(lower.a))
            {
                if (!combined.vertices.Contains(upper.b))
                {
                    combined.vertices.Add(B.vertexAt(i % B.vertices.Count));
                    i++;
                }
                else
                {
                    combined.vertices.Add(A.vertexAt(j % A.vertices.Count));
                    j++;
                }
            }

            combined.Order(combined.leftMost, combined.vertices);
            return combined;
        }

        public static Edge findUpper(Hull A, Hull B)
        {
            PointF a = A.rightMost;
            PointF b = B.leftMost;
            Edge currentEdge = new Edge(a, b);

            int i = A.getIndex(a);
            int j = B.getIndex(b);
            bool iterateA = false;
            bool potential = false;
            bool found = false;

            while (!found)
            {
                if (iterateA)
                {
                    a = A.vertexAt((i + 1) % A.vertices.Count);
                    Edge next = new Edge(a, currentEdge.b);
                    //ConvexHullSolver.drawLine(next);

                    if (next.slope < currentEdge.slope)
                    {
                        potential = false;
                        currentEdge = next;
                        i++;
                    }
                    else
                    {
                        iterateA = false;

                        if (potential)
                            found = true;
                        else
                            potential = true;
                    }
                }
                else
                {
                    b = B.vertexAt(mod(j-1,B.vertices.Count));
                    Edge next = new Edge(currentEdge.a, b);
                    //ConvexHullSolver.drawLine(next);

                    if (next.slope > currentEdge.slope)
                    {
                        potential = false;
                        currentEdge = next;
                        j--;
                    }
                    else
                    {
                        iterateA = true;

                        if (potential)
                            found = true;
                        else
                            potential = true;
                    }
                }
            }
            return currentEdge;
        }

        public static Edge findLower(Hull A, Hull B)
        {
            PointF a = A.rightMost;
            PointF b = B.leftMost;
            Edge currentEdge = new Edge(a, b);

            int i = A.getIndex(a);
            int j = B.getIndex(b);
            bool peek = true;
            bool potential = false;
            bool found = false;

            while (!found)
            {
                if (peek)
                {
                    a = A.vertexAt(mod(i - 1, A.vertices.Count));
                    Edge next = new Edge(a, currentEdge.b);
                    //ConvexHullSolver.drawLine(next, true);

                    if (next.slope > currentEdge.slope)
                    {
                        potential = false;
                        currentEdge = next;
                        i--;
                    }
                    else
                    {
                        peek = false;

                        if (potential)
                            found = true;
                        else
                            potential = true;
                    }
                }
                else
                {
                    b = B.vertexAt((j + 1) % B.vertices.Count);
                    Edge next = new Edge(currentEdge.a, b);
                    //ConvexHullSolver.drawLine(next, true);

                    if (next.slope < currentEdge.slope)
                    {
                        potential = false;
                        currentEdge = next;
                        j++;
                    }
                    else
                    {
                        peek = true;

                        if (potential)
                            found = true;
                        else
                            potential = true;
                    }
                }
            }
            return currentEdge;
        }
    }
}
