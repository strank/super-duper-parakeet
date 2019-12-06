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
    public static Vector3 RandomVector3WithinBounds(Vector3 min, Vector3 max)
    {
        Vector3 determinedVector = new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z));

        return determinedVector;
    }
}
