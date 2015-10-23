using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using _2_convex_hull;

namespace _1_convex_hull
{
    class Hull
    {
        private PointF leftMost { get; set; }
        private PointF rightMost { get; set; }
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
            //Time Complexity: O(n)
            //Space Complexity: O(2n) = O(n)

            leftMost = pointList[0];
            rightMost = pointList.Last();

            //Time: O(n), Space: O(n)
            Order(leftMost, pointList);
            //Space: O(n)
            vertices = new List<PointF>(pointList);
        }

        private PointF getNextVertex(int i)
        {
            return vertices.ElementAt((i + 1) % vertices.Count);
        }

        private PointF getPrevVertex(int i)
        {
            return vertices.ElementAt(mod(i - 1, vertices.Count));
        }

        private static int mod(int x, int m)
        {
            //Performs modular division and accounts for looping with negative numbers.
            return (x % m + m) % m;
        }

        private static double findSlope(PointF a, PointF b)
        {
            return (b.Y - a.Y) / (b.X - a.X);
        }

        public void Order(PointF leftMost, List<System.Drawing.PointF> pointList)
        {
            //Time Complexity: O(n)
            //Space Complexity: O(n)

            //Create a dictionary that maps slope values to the corresponding endpoint.
            Dictionary<double, PointF> slopes = new Dictionary<double, PointF>();

            //Remove the leftmost point from the list, as this will act as our origin for each edge.
            pointList.Remove(leftMost);

            //Space = O(n-1), Time = O(n-1)
            //Find the slopes and add them into the dictionary.
            foreach (PointF a in pointList)
            {
                slopes.Add(findSlope(leftMost, a), a);
            }

            //Time = O(n-1)
            //Remove all the points from the list.
            pointList.Clear();

            //Time = O(n-1)
            //Add them back into the list in the correct order.
            foreach (var p in slopes.OrderBy(i => i.Key))
                pointList.Add(p.Value);

            //Add the origin back in at the beginning of the list.
            pointList.Insert(0, leftMost);
        }

        public static Hull Combine(Hull Left, Hull Right)
        {
            //Time Complexity: O(n + 2n + m) = O(n)
            //Space Complexity: O(3n + m) = O(n)

            Hull combined = new Hull();
            //Find the upper and lower tangent edges.
            //Time: O(2n), Space: O(n)
            Edge upper = findUpper(Left, Right);
            Edge lower = findLower(Left, Right);

            //Set the rightmost and leftmost points.
            combined.leftMost = Left.leftMost;
            combined.rightMost = Right.rightMost;

            //Find the point at which to start iterating on each hull.
            int i = Right.vertices.IndexOf(lower.b);
            int j = Left.vertices.IndexOf(upper.a);

            //Start at the lower b point and iterate around until you arrive at the lower a point.
            while (!combined.vertices.Contains(lower.a))
            {
                //If you have not arrive at upper b, add points from hull B into the combined hull.
                if (!combined.vertices.Contains(upper.b))
                {
                    combined.vertices.Add(Right.vertices.ElementAt(i % Right.vertices.Count));
                    i++;
                }
                //If you have arrived at upper b, push all of the points from hull A into the combined hull from upper a to lower a.
                else
                {
                    combined.vertices.Add(Left.vertices.ElementAt(j % Left.vertices.Count));
                    j++;
                }
            }

            combined.Order(combined.leftMost, combined.vertices);
            return combined;
        }

        //These two methods are designed to find the upper and lower tangents of two hulls.
        //They're essentially mirror methods, with the findUpper iterating up left and down right
        //and vice-versa for findLower.
        private static Edge findUpper(Hull Left, Hull Right)
        {
            //Time Complexity: ~O(n/2) = O(n)
            //Space Complexity: ~O(2n/2) = O(n)

            //Define the starting edge as the edge between the rightmost and leftmost points of the two hulls.
            Edge currentEdge = new Edge(Left.rightMost, Right.leftMost);
            //These values, along with the getNext and getPrev functions, act as our system for iterating over a hull.
            int i = Left.vertices.IndexOf(Left.rightMost);
            int j = Right.vertices.IndexOf(Right.leftMost);

            while(true)
            {

                Edge nextLeft = new Edge(Left.getNextVertex(i), currentEdge.b);
                Edge nextRight = new Edge(currentEdge.a, Right.getPrevVertex(j));

                //Check for conditions that indicate that we've found the upper tangent.
                if (nextLeft.slope > currentEdge.slope && nextRight.slope < currentEdge.slope)
                {
                    return currentEdge;
                }

                //Check to see in which direction to iterate and then advance the currentEdge.
                if (nextLeft.slope < currentEdge.slope)
                {
                    currentEdge = nextLeft;
                    i++;
                }
                else if (nextRight.slope > currentEdge.slope)
                {
                    currentEdge = nextRight;
                    j--;
                }
            }
        }

        private static Edge findLower(Hull A, Hull B)
        {
            Edge currentEdge = new Edge(A.rightMost, B.leftMost);
            int i = A.vertices.IndexOf(A.rightMost);
            int j = B.vertices.IndexOf(B.leftMost);

            while (true)
            {
                Edge nextA = new Edge(A.getPrevVertex(i), currentEdge.b);
                Edge nextB = new Edge(currentEdge.a, B.getNextVertex(j));

                if (nextA.slope < currentEdge.slope && nextB.slope > currentEdge.slope)
                    return currentEdge;

                if (nextA.slope > currentEdge.slope)
                {
                    currentEdge = nextA;
                    i--;
                }
                else if (nextB.slope < currentEdge.slope)
                {
                    currentEdge = nextB;
                    j++;
                }
            }
        }
    }
}
