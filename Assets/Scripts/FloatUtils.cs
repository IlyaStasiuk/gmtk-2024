using System;
using UnityEngine;

public static class FloatUtils
{
    public static float MapRange(float value, float from1, float from2, float to1, float to2, bool clamped = true)
    {
        float result = to1 + (value - from1) * (to2 - to1) / (from2 - from1);
        return clamped ? Math.Clamp(result, Math.Min(to1, to2), Math.Max(to1, to2)) : result;
    }


    public static float MapRange01(float value, float from1, float from2, bool clamped = true)
    {
        float result = (value - from1) / (from2 - from1);
        return clamped ? Mathf.Clamp01(result) : result;
    }
}