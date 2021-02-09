using System;
using UnityEngine;

[Serializable]
public class Range
{
    public float min;
    public float max;

    public float range
    {
        get => this.max - this.min;
    }
    
    public Range(float min, float max)
    {
        this.max = Mathf.Max(min, max);
        this.min = Mathf.Min(min, max);
    }

    public float Clamp(float value)
    {
        return Mathf.Clamp(value, min, max);
    }
}