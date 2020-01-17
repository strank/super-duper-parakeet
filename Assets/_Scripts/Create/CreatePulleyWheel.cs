using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePulleyWheel : Create {

    #region Variables
    // Received from Scenario class
    // Rope Information
    private Vector3 ropeDirection;
    private float ropeAngle;
    private Vector3 startPosition;
    private Vector3 endPosition;

    // Wheels
    private GameObject[] pulleyWheelOptions;
    private int minWheels;
    private int maxWheels; // Must not be too large that they do not fit on rope length options

    // Other
    private float minSeparationBetweenWheels = 2.0f;
    private float distanceFromEnds = 2.0f;
    private bool useDefaultDistanceBelowRopeForWheels = true;
    private float distanceBelowRopeToSpawnWheels = 2.0f;

    // Determined within class itself
    private int numberOfWheels;
    private Vector3[] wheelPositions;
    private Vector3 wheelRangeStartPoint;
    private Vector3 wheelRangeEndPoint;


    #endregion

    #region Unity Methods
    
    public void BuildPulleyWheel(Vector3 _startPosition, Vector3 _endPosition, bool _useDefaultDistanceBelowRopeForWheels, 
        float _distanceBelowRopeToSpawnWheels, float _minSeparationBetweenWheels, float _ropeAngle, Vector3 _ropeDirection, 
        float _distanceFromEnds, int _minWheels, int _maxWheels, GameObject[] _pulleyWheelOptions, GameObject parent)
    {
        startPosition = _startPosition;
        endPosition = _endPosition;
        useDefaultDistanceBelowRopeForWheels = _useDefaultDistanceBelowRopeForWheels;
        distanceBelowRopeToSpawnWheels = _distanceBelowRopeToSpawnWheels;
        minSeparationBetweenWheels = _minSeparationBetweenWheels;
        ropeAngle = _ropeAngle;
        ropeDirection = _ropeDirection;
        distanceFromEnds = _distanceFromEnds;
        minWheels = _minWheels;
        maxWheels = _maxWheels;
        pulleyWheelOptions = _pulleyWheelOptions;

        scenarioParent = parent.transform;

        PositionWheels();
        GenerateWheels();
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
        // Assuming separationDistance is lower enough to start the wheel height to ensure rope falls onto wheel
        // Can use another value if that assumption gives poor results
        if (useDefaultDistanceBelowRopeForWheels)
            wheelHeight -= minSeparationBetweenWheels;
        else
            wheelHeight -= distanceBelowRopeToSpawnWheels;

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
        numberOfWheels = Random.Range(minWheels, maxWheels + 1); // Need the plus 1 because the maximum of the range is exclusive
        wheelPositions = new Vector3[numberOfWheels];

        List<WeightedRange> positionRanges = new List<WeightedRange>();
        WeightedRange firstRange = CreateWheelPositionRange();
        positionRanges.Add(firstRange);

        for (int i = 0; i < numberOfWheels; i++)
        {
            if (positionRanges.Count == 0)
                Debug.LogError("Trying to create another wheel, but there is not enough room based on other parameters.");
            
            // Need to pick a range from the list of ranges, and then randomly select a value within that range
            float rangePicker = Random.Range(0.0f, 1.0f);
            float weightCheck = 0.0f;
            WeightedRange selectedRange;
            int rangeCounter = 0;

            Debug.Log("The total wheels is: " + numberOfWheels + " and this is wheel ID: " + i);

            while (weightCheck < rangePicker)
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
            if (check <= (range.Minimum + buffer) &&
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
            if (check > range.Minimum && check < range.Maximum)
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
        // Determine the rotation needed for the wheels to line up with the rope
        ropeAngle *= -1 * Mathf.Rad2Deg; // Needs the -1 so it properly lines up the Unity rotation
        Debug.Log("Wheel generator ropeAngle is: " + ropeAngle);
        Debug.Log("Count for wheel positions is: " + wheelPositions.Length);

        foreach (Vector3 v in wheelPositions)
        {
            int randomWheelIndex = Random.Range(0, pulleyWheelOptions.Length);
            Debug.Log("The random wheel index is: " + randomWheelIndex);
            GameObject pulleyWheel = pulleyWheelOptions[randomWheelIndex];
            Instantiate(pulleyWheel, v, Quaternion.Euler(0f, ropeAngle, 0f), scenarioParent);
        }
    }

    #endregion
}
