using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterRange
{
    private float minimum;
    private float maximum;

    public float Minimum
    {
        get {
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

public class WeightedRange : ParameterRange
{
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

public class CreatePulley : MonoBehaviour {

    #region Variables
    [Header("Objects to Use as Pulley Ends")]
    //public GameObject ledge;

    [Header("Parameter Ranges")]
    [Header("Rope")]
    [Header("Length")]
    public float minLength = 2.0f;
    public float maxLength = 12.0f;
    private float length;
    private Vector3 ropeDirection;
    private float ropeAngle;

    [Header("Start Point")]
    public Vector3 minStart = Vector3.zero;
    public Vector3 maxStart = new Vector3(20.0f, 20.0f, 20.0f);
    private Vector3 startPosition;

    //[Header("End Point")]
    //public Vector3 minEnd = new Vector3(1.0f, 1.0f, 1.0f);
    //public Vector3 maxEnd = new Vector3(20.0f, 20.0f, 20.0f);

    // End will be determined by system to fit the proper length
    private Vector3 endPosition;

    [Header("Wheels")]
    [Header("Object to Use as Wheels")]
    public GameObject pulleyWheel;
    [Header("Quantity")]
    public int minWheels = 1;
    public int maxWheels = 3;
    private int numberOfWheels;

    [Header("Other")]
    public float minSeparationBetweenWheels = 2.0f;
    public float distanceFromEnds = 2.0f;
    private Vector3 wheelPosition;
    private Vector3[] wheelPositions;
    private float minDistanceBelowRope = 3.0f;
    private Vector3 wheelRangeStartPoint;
    private Vector3 wheelRangeEndPoint;


    // Used if setting up to create several instances at once
    private int numberOfTests;
    private float separationDistance;

    // Trying new singleton pattern within the property itself
    private static CreatePulley _instance;

    private CreatePulley()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static CreatePulley Instance
    {
        get
        {
            if(_instance == null)
            {
                new CreatePulley();
            }

            return _instance;
        }
    }

    public GenerationSystem generationSystem;
    #endregion

    #region Unity Methods

    private void Start()
    {
        RandomizeStartPosition();
        DetermineLength();
        PositionEndPoint();
        CreateWheelPositionRange();
        PositionWheels();
        GenerateWheels();
    }

    /*
     * Sets start position to a random vector3 within the min and max bounds.
     */
    private void RandomizeStartPosition()
    {
        startPosition = RandomVector3WithinBounds(minStart, maxStart);
        Debug.Log("Randomzied pulley start position is: " + startPosition);
    }

    /*
     * Determines a random length value based on the parameter bounds
     */
    private void DetermineLength()
    {
        length = Random.Range(minLength, maxLength);
        Debug.Log("Length of pulley rope is: " + length);
    }

    ///*
    // * Randomly positions the end point based on where the start point has been positioned 
    // * and the length that should be between the start and end point.
    // */
    //private void PositionEndPoint()
    //{
    //    // Chooses a random direction, then scales this by the determined length 
    //    // and adds it to the starting position
    //    ropeDirection = Random.onUnitSphere;
    //    endPosition = startPosition + ropeDirection * length;
    //    Debug.Log("Randomized pulley end position is: " + endPosition);
    //}

    /*
     * Randomly positions the end point based on where the start point has been positioned 
     * and the length that should be between the start and end point.
     * Since rope will need to fall anyway, end point is just positioned within the same xz plane as 
     * the start point for simplicity.
     */
    private void PositionEndPoint()
    {
        // Chooses a random direction, then scales this by the determined length 
        // and adds it to the starting position
        ropeAngle = Random.Range(0.0f, 360.0f);
        // Converts from degrees to radians
        ropeAngle *= Mathf.Deg2Rad;

        ropeDirection = new Vector3(
            Mathf.Cos(ropeAngle),
            0.0f,
            Mathf.Sin(ropeAngle));

        endPosition = startPosition + ropeDirection * length;
        Debug.Log("Randomized pulley end position is: " + endPosition);
    }


    // The following method is overdone with the simplification done to PositionEndPoint, but I will 
    // keep it for now as a reference or if the approach changes.
    // With update, it should currently basically just draw the same line dictated by the startPosition and 
    // endPosition, but lower in the y value.
    /*
     * Creates the initial range of positions for instantiating the pulley wheel
     */
    private WeightedRange CreateWheelPositionRange()
    {
        // Create the xz plane projection of the line between the start and end point
        // Also place that line below both the start and end point

        // First determine which of start and end position is lower
        float wheelHeight = startPosition.y;
        if (endPosition.y < startPosition.y)
        {
            wheelHeight = endPosition.y;
        }
        // Assuming separationDistance is enough lower to start the wheel height to ensure rope falls onto wheel
        wheelHeight -= minSeparationBetweenWheels;

        // Creates the start and end point for wheel positions which is a projection of the start and end positions 
        // of the rope onto the xz plane at the determined height value
        wheelRangeStartPoint = new Vector3(
            startPosition.x,
            wheelHeight,
            startPosition.z);

        wheelRangeEndPoint = new Vector3(
            endPosition.x,
            wheelHeight,
            endPosition.z);

        float lengthOfRange = Vector3.Distance(wheelRangeStartPoint, wheelRangeEndPoint);
        WeightedRange initialWheelPositionRange = new WeightedRange(distanceFromEnds, lengthOfRange - distanceFromEnds, 1.0f);

        Debug.DrawLine(startPosition, endPosition, Color.blue, 500.0f);
        Debug.DrawLine(wheelRangeStartPoint, wheelRangeEndPoint, Color.red, 500.0f);
        Debug.Log("Ray drawn for wheel position range.");
        Debug.Log("Randomized wheel range start position is: " + wheelRangeStartPoint);
        Debug.Log("Randomized wheel range end position is: " + wheelRangeEndPoint);

        return initialWheelPositionRange;
    }

    /*
     * Positions the pulley wheels along the designated range of positions
     */
    private void PositionWheels()
    {
        numberOfWheels = Random.Range(minWheels, maxWheels + 1);
        wheelPositions = new Vector3[numberOfWheels];

        List<WeightedRange> positionRanges = new List<WeightedRange>();
        WeightedRange firstRange = CreateWheelPositionRange();
        positionRanges.Add(firstRange);

        for (int i = 0; i < numberOfWheels; i++)
        {
            // Need to pick a range from the list of ranges, and then randomly select a value within that range
            float rangePicker = Random.Range(0.0f, 1.0f);
            float weightCheck = 0.0f;
            WeightedRange selectedRange;
            int rangeCounter = 0;

            while(weightCheck < rangePicker)
            {
                weightCheck += positionRanges[rangeCounter].Weight;
                rangeCounter++;
            }

            // Loop completes once it finds the proper range, but still increments
            // the counter one last time, so the selectedRange subtracts 1 to make up for this.
            selectedRange = positionRanges[rangeCounter - 1];

            // Selects a random value from the chosen range and uses this to set this wheel position
            float selectedValue = Random.Range(selectedRange.Minimum, selectedRange.Maximum);
            wheelPositions[i] = wheelRangeStartPoint + ropeDirection * selectedValue;

            // Uses the randomly selected value to recalculate the possible position ranges
            DetermineRanges(minSeparationBetweenWheels, selectedValue, positionRanges);
            ReweightRangeList(positionRanges);
        }
    }

    /*
     * Determines if a check value is within the buffer distance of any ranges within a list of ranges, 
     * and edits the list of ranges accordingly.
     */
    private void DetermineRanges(float buffer, float check, List<WeightedRange> rangeList)
    {
        foreach (WeightedRange range in rangeList)
        {
            if(check <= (range.Minimum + buffer) && 
                check >= (range.Maximum - buffer))
            {
                // Means there is no more room around this current range, so just remove it
                rangeList.Remove(range);
                return;
            }

            if (check <= (range.Minimum + buffer))
            {
                // Keeps the same amount of ranges, but one must be modified
                // Edit range
                range.Minimum = check + buffer;
                return;
            }

            if (check >= (range.Maximum - buffer))
            {
                // Keeps the same amount of ranges, but one must be modified
                // Edit range
                range.Maximum = check - buffer;
                return;
            }

            // Final check is if it is within the current range, but not near the bounds
            if(check > range.Minimum && check < range.Maximum)
            {
                // Effectively splits the range into two new ranges
                // Edit range and create new range

                // The new range uses the existing range's maximum, but has a new minimum 
                // starting after the current check value.
                WeightedRange additionalRange = new WeightedRange(check + buffer, range.Maximum, 0.0f);

                // The current range is edited so that it keeps the same minimum, but its maximum is now 
                // located before the check value.
                range.Maximum = check - buffer;

                rangeList.Add(additionalRange);
                return;
            }

        }
    }

    /*
     * Takes a list of weighted ranges and recalculates their weights so that each individual weighted range 
     * has a weight value which corresponds directly to the proportion of the total range of all weighted ranges 
     * that that specific weighted range covers.
     */
    private void ReweightRangeList(List<WeightedRange> rangeList)
    {
        float rangeSum = 0.0f;

        foreach (WeightedRange range in rangeList)
        {
            rangeSum += (range.Maximum - range.Minimum);
        }

        foreach (WeightedRange range in rangeList)
        {
            range.Weight = (range.Maximum - range.Minimum) / rangeSum;
        }
    }

    /*
     * Instantiates a number of pulley wheels equal to the number of locations within the 
     * wheelPositions array at the location specified by the wheelPositions array.
     */
    private void GenerateWheels()
    {
        foreach (Vector3 v in wheelPositions)
        {
            Instantiate(pulleyWheel, v, Quaternion.identity);
        }
    }

    /*
     * Gives a randomized Vector3 based on minimum and maximum bounds set by parameters.
     * Vector3 will be somewhere within a bounding cube created by the min and max 
     * bounding parameters.
     * The bounding cubes dimensions would be (max.x - min.x, max.y - min.y, max.z - min.z)
     */
    private Vector3 RandomVector3WithinBounds(Vector3 min, Vector3 max)
    {
        Vector3 determinedVector = new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z));

        return determinedVector;
    }

    #endregion
}
