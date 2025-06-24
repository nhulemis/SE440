using System.Collections.Generic;
using UnityEngine;

namespace GJK
{
    public class GJKPhysic2D
    {
        public const int MaxIterations = 30;
        
        public static bool IsColliding(IVertexShape shapeA, IVertexShape shapeB)
        {
            // Initialize the simplex with a random direction
            Vector2 direction = Random.insideUnitCircle.normalized;
            List<Vector2> simplex = new List<Vector2>();

            Vector2 point = Support(shapeA, shapeB, direction);
            simplex.Add(point);
            direction = -point; // New direction towards origin

            for (int i = 0; i < MaxIterations; i++)
            {
                point = Support(shapeA, shapeB, direction);
                if (Vector2.Dot(point, direction) <= 0)
                    return false; // No collision

                simplex.Add(point);

                if (UpdateSimplexAndDirection(ref simplex, ref direction))
                    return true; // Collision detected
            }

            return false;
        }

        private static bool UpdateSimplexAndDirection(ref List<Vector2> simplex,ref Vector2 direction)
        {
            if (simplex.Count == 2)
            {
                // Line segment case
                Vector2 a = simplex[1];
                Vector2 b = simplex[0];
                Vector2 ab = b - a;
                Vector2 ao = -a;

                direction = Vector2.Perpendicular(ab);
                if (Vector2.Dot(direction, ao) < 0)
                    direction = -direction;

            }
            else if (simplex.Count == 3)
            {
                Vector2 a = simplex[2];
                Vector2 b = simplex[1];
                Vector2 c = simplex[0];

                Vector2 ab = b - a;
                Vector2 ac = c - a;
                Vector2 ao = -a;

                Vector2 acPerp = Vector2.Perpendicular(ac);
                if (Vector2.Dot(acPerp, ao) > 0)
                {
                    simplex.RemoveAt(0); // remove c
                    direction = acPerp;
                }
                else
                {
                    Vector2 abPerp = Vector2.Perpendicular(ab);
                    if (Vector2.Dot(abPerp, ao) > 0)
                    {
                        simplex.RemoveAt(1); // remove b
                        direction = abPerp;
                    }
                    else
                    {
                        return true; // Origin is inside triangle => collision
                    }
                }
            }

            return false;
        }

        private static Vector2 Support(IVertexShape shapeA, IVertexShape shapeB, Vector2 direction)
        {
            // Get the support points in the direction of the Minkowski difference
            Vector2 pointA = shapeA.Support(direction);
            Vector2 pointB = shapeB.Support(-direction);
            
            // Return the Minkowski difference point
            return pointA - pointB;
        }
    }
}