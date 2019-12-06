using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterRange {

    private float minimum;
    private float maximum;

    public float Minimum
    {
        get
        {
            return minimum;
        }

        set
        {
            minimum = value;
        }
    }

    public float Maximum
    {
        get
        {
            return maximum;
        }

        set
        {
            maximum = value;
        }
    }

    public ParameterRange(float min, float max)
    {
        Minimum = min;
        Maximum = max;
    }
}
