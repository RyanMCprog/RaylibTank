using System;
using System.Collections.Generic;
using System.Text;
using MathClasses;
using Raylib;

namespace ConsoleApp1
{
    class AABB
    {
       public MathClasses.Vector3 min = new MathClasses.Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
       public MathClasses.Vector3 max = new MathClasses.Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        public AABB()
        {

        }

        public AABB(MathClasses.Vector3 min, MathClasses.Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        public MathClasses.Vector3 Center()
        {
            return (min + max) * 0.5f;
        }

        public MathClasses.Vector3 Extents()
        {
            return new MathClasses.Vector3(Math.Abs(max.x - min.x) * 0.5f, Math.Abs(max.y - min.y) * 0.5f, Math.Abs(max.z - min.z) * 0.5f);
        }

        public List<MathClasses.Vector3> Corners()
        {
            List<MathClasses.Vector3> corners = new List<MathClasses.Vector3>(4);
            corners[0] = min;
            corners[1] = new MathClasses.Vector3(min.x, max.y, min.z);
            corners[2] = max;
            corners[3] = new MathClasses.Vector3(max.x, min.y, min.z);
            return corners;
        }

        public void Fit(List<MathClasses.Vector3> points)
        {
            min = new MathClasses.Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            max = new MathClasses.Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            foreach(MathClasses.Vector3 p in points)
            {
                min = MathClasses.Vector3.Min(min, p);
                max = MathClasses.Vector3.Max(max, p);
            }
        }

        public void Fit(MathClasses.Vector3[] points)
        {
            min = new MathClasses.Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            max = new MathClasses.Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            foreach (MathClasses.Vector3 p in points)
            {
                min = MathClasses.Vector3.Min(min, p);
                max = MathClasses.Vector3.Max(max, p);
            }
        }

        public bool Overlaps(MathClasses.Vector3 p)
        {
            return !(p.x < min.x || p.y < min.y || p.x > max.x || p.y > max.y);
        }

        public bool Overlaps(AABB other)
        {
            return !(max.x < other.min.x || max.y < other.min.y || min.x > other.max.x || min.y > other.max.y);
        }

        public MathClasses.Vector3 ClosestPoint(MathClasses.Vector3 p)
        {
            return MathClasses.Vector3.Clamp(p,min,max);
        }

        public bool IsEmpty()
        {
            if (float.IsNegativeInfinity(min.x) && float.IsNegativeInfinity(min.y) && float.IsNegativeInfinity(min.z) && 
                float.IsInfinity(max.x) && float.IsInfinity(max.y) && float.IsInfinity(max.z))
                return true;
            return false;
        }

        public void Empty()
        {
            min = new MathClasses.Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            max = new MathClasses.Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        }

        void SetToTransformedBox(AABB box, Matrix3 m)
        {
            if(box.IsEmpty())
            {
                Empty();
                return;
            }

            // Examine each of the nine matrix elements
            // and compute the new AABB
            if (m.m1 > 0.0f)
            {
                min.x += m.m1 * box.min.x; max.x += m.m1 * box.max.x;
            }
            else
            {
                min.x += m.m1 * box.max.x; max.x += m.m1 * box.min.x;
            }

            if (m.m2 > 0.0f)
            {
                min.y += m.m2 * box.min.x; max.y += m.m2 * box.max.x;
            }
            else
            {
                min.y += m.m2 * box.max.x; max.y += m.m2 * box.min.x;
            }

            if (m.m3 > 0.0f)
            {
                min.z += m.m3 * box.min.x; max.z += m.m3 * box.max.x;
            }
            else
            {
                min.z += m.m3 * box.max.x; max.z += m.m3 * box.min.x;
            }

            if (m.m4 > 0.0f)
            {
                min.x += m.m4 * box.min.x; max.x += m.m4 * box.max.x;
            }
            else
            {
                min.x += m.m4 * box.max.x; max.x += m.m4 * box.min.x;
            }

            if (m.m5 > 0.0f)
            {
                min.y += m.m5 * box.min.x; max.y += m.m5 * box.max.x;
            }
            else
            {
                min.y += m.m5 * box.max.x; max.y += m.m5 * box.min.x;
            }

            if (m.m6 > 0.0f)
            {
                min.z += m.m6 * box.min.x; max.z += m.m6 * box.max.x;
            }
            else
            {
                min.z += m.m6 * box.max.x; max.z += m.m6 * box.min.x;
            }

            if (m.m7 > 0.0f)
            {
                min.x += m.m7 * box.min.x; max.x += m.m7 * box.max.x;
            }
            else
            {
                min.x += m.m7 * box.max.x; max.x += m.m7 * box.min.x;
            }

            if (m.m8 > 0.0f)
            {
                min.y += m.m8 * box.min.x; max.y += m.m8 * box.max.x;
            }
            else
            {
                min.y += m.m8 * box.max.x; max.y += m.m8 * box.min.x;
            }

            if (m.m9 > 0.0f)
            {
                min.z += m.m9 * box.min.x; max.z += m.m9 * box.max.x;
            }
            else
            {
                min.z += m.m9 * box.max.x; max.z += m.m9 * box.min.x;
            }
        }
    }
}
