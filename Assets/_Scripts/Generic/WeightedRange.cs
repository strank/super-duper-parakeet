using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedRange : ParameterRange {

    private float weight;
    public float Weight
    {
        get
        {
            return weight;
        }

        set
        {
            weight = value;
        }
    }

    public WeightedRange(float min, float max, float p) : base(min, max)
    {
        Weight = p;
    }
}
