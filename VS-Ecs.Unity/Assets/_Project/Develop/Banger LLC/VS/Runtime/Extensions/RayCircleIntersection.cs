using System;
using UnityEngine;

namespace VS.Runtime.Extensions
{
    class RayCircleIntersection
    {
        /*public static bool FindIntersection(
            Vector2 origin, Vector2 direction, // Ray origin & direction
            Vector2 center, float r, // Circle center & radius
            out float ix, out float iy) // Output intersection point
        {
            // Vector from circle center to ray origin
            Vector2 v = center - origin;

            // Quadratic coefficients
            float A = direction.x * direction.x + direction.y * direction.y; // Should be 1 if D is normalized
            float B = 2 * (v.x * direction.x + v.y * direction.y);
            float C = (v.x * v.x + v.y * v.y) - (r * r);

            // Compute discriminant
            float discriminant = B * B - 4 * A * C;

            if (discriminant < 0)
            {
                ix = iy = 0;
                Debug.Log("D is negative, no intersection");
                return false; // No intersection
            }

            // Compute two possible t values
            float sqrtD = Mathf.Sqrt(discriminant);
            float t1 = (-B - sqrtD) / (2 * A);
            float t2 = (-B + sqrtD) / (2 * A);

            // Take the smallest non-negative t
            float t = (t1 >= 0) ? t1 : (t2 >= 0) ? t2 : float.NaN;

            if (float.IsNaN(t))
            {
                Debug.Log("t is NaN, no intersection");
                ix = iy = 0;
                return false; // Intersection is behind the ray origin
            }

            // Compute intersection point
            ix = origin.x + t * direction.x;
            iy = origin.y + t * direction.y;
            return true;
        }*/
        
        
        public static bool FindIntersection(
            Vector2 o, Vector2 d, // Ray origin & direction
            Vector2 c, float r, // Circle center & radius
            out Vector2 ip) // Output intersection point
        {
            Vector2 oc = c - o;
            float ocDistance = oc.magnitude;
            if (ocDistance > r)
            {
                ip = new Vector2(float.NaN, float.NaN);
                return false;
            }

            var projectionLength = Vector2.Dot(oc, d);
            Vector2 point = o + d * projectionLength;
            float cat = (c - point).magnitude;
            var line = Mathf.Sqrt(r * r - cat * cat);
            ip = point - (-d * line);
            return true;
        }
    }
}