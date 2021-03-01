using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 SetX(this Vector3 v, float value) => new Vector3(value, v.y, v.z);
}