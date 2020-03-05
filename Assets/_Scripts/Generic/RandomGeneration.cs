using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGeneration {

    /*
     * Gives a randomized Vector3 based on minimum and maximum bounds set by parameters.
     * Vector3 will be somewhere within a bounding cube created by the min and max 
     * bounding parameters.
     * The bounding cubes dimensions would be (max.x - min.x, max.y - min.y, max.z - min.z)
     */
    public static Vector3 RandomVector3WithinBounds(Vector3 min, Vector3 max, System.Random seedValue)
    {
        //Vector3 determinedVector = new Vector3(
        //    Random.Range(min.x, max.x),
        //    Random.Range(min.y, max.y),
        //    Random.Range(min.z, max.z));

        Vector3 determinedVector = new Vector3(
            CalculateRandomFloatRange(min.x, max.x, seedValue),
            CalculateRandomFloatRange(min.y, max.y, seedValue),
            CalculateRandomFloatRange(min.z, max.z, seedValue));

        return determinedVector;
    }

    /*
     * Calculation to use the 0.0 to 1.0 range from System.Random.NextDouble() to select a value between 
     * a designated min and max value.
     */
    public static float CalculateRandomFloatRange(float minValue, float maxValue, System.Random seedValue)
    {
        return minValue + (float)seedValue.NextDouble() * (maxValue - minValue);
    }
}
