using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


    public static class VectorOps
    {
        public static float AngleVec(Vector3 one, Vector3 two)
     {
        return (float)(Mathf.Acos(Vector3.Dot(one, two)) * 180f * 0.3184f);
     }
    public static Vector3 PerpVector(Vector3 one, Vector3 two)
    {
        return Vector3.Cross(one, two);
    }
}

