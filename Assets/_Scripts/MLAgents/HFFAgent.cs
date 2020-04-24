using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
using Random = UnityEngine.Random;


public class HFFAgent : Agent
{
    private HFFSimInput hffInput;
    private short xLook = 0;
    private short yLook = 0;
    private short xWalk = 0;
    private short yWalk = 0;
    private const float lookWalkFactor = 100f;
    private const short lookWalkMax = short.MaxValue / 3;
    private GameObject hffPlayer;
    private Rigidbody playerRB;
    private Vector3 m_dirToTarget;

    [Header("Spawnpoint, Target To Reach (auto-populated by name if null)")]
    public Transform spawnpoint;
    public Collider target;
    public float failDistance = 10f;

    [Header("Parts of the puzzle that need to be reset on training")]
    public GameObject resettableParts;

    private GameObject clonedParts;

    [Header("Reward Functions To Use")]
    [Space(10)]
    public bool rewardMovingTowardsTarget; // Agent should move towards target
    public bool rewardFacingTarget; // Agent should face the target
    public bool rewardUseTimePenalty; // Hurry up

    // ML-Agent overrides:

    public override void Initialize() {
        //instantiatedLevel = Instantiate(level);
        hffPlayer = GameObject.Find("/Player(Clone)/Ball");
        playerRB = hffPlayer.GetComponent<Rigidbody>();
        if (spawnpoint == null) {
            spawnpoint = GameObject.Find("InitialSpawnpoint").transform;
        }
        if (target == null) {
            target = GameObject.Find("Checkpoint").GetComponent<Collider>();
        }
        m_dirToTarget = target.gameObject.transform.position - hffPlayer.transform.position;
    }

    private void ResetSimInputs() {
        hffInput.Look(0, 0);
        hffInput.Walk(0, 0);
        hffInput.JumpSet(false); // so 0 or less means release jump button
        hffInput.LeftGrabSet(false);
        hffInput.RightGrabSet(false);
        hffInput.SubmitInput();
        xLook = 0;
        yLook = 0;
        xWalk = 0;
        yWalk = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(hffPlayer.transform.localPosition);
        sensor.AddObservation(target.gameObject.transform.localPosition);
        // add an observation touchingGround?
        // or make observations orientation dependent?
        // or/and use raycast observations?
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        for (var i = 0; i < vectorAction.Length; i++)
        {
            vectorAction[i] = Mathf.Clamp(vectorAction[i], -1f, 1f);
        }
        xLook = Convert.ToInt16(Mathf.Clamp(xLook + vectorAction[0] * lookWalkFactor, -lookWalkMax, lookWalkMax));
        yLook = Convert.ToInt16(Mathf.Clamp(yLook + vectorAction[1] * lookWalkFactor, -lookWalkMax, lookWalkMax));
        hffInput.Look(xLook, yLook);
        xWalk = Convert.ToInt16(Mathf.Clamp(xWalk + vectorAction[2] * lookWalkFactor, -lookWalkMax, lookWalkMax));
        yWalk = Convert.ToInt16(Mathf.Clamp(yWalk + vectorAction[3] * lookWalkFactor, -lookWalkMax, lookWalkMax));
        hffInput.Walk(xWalk, yWalk);
        hffInput.JumpSet(vectorAction[4] > 0); // so 0 or less means release jump button
        //hffInput.LeftGrabSet(vectorAction[5] > 0);
        //hffInput.RightGrabSet(vectorAction[6] > 0);
        hffInput.SubmitInput();
    }

    public void FixedUpdate() {
        m_dirToTarget = target.gameObject.transform.position - hffPlayer.transform.position;
        if (rewardMovingTowardsTarget) {
            RewardFunctionMovingTowards();
        }
        if (rewardFacingTarget) {
            RewardFunctionFacingTarget();
        }
        if (rewardUseTimePenalty) {
            RewardFunctionTimePenalty();
        }
        // reward hitting the target:
        var playerPos = hffPlayer.transform.position;
        var closestPoint = target.ClosestPoint(playerPos);
        if (closestPoint == playerPos) {
            AddReward(1f);
            EndEpisode();
        } else {
            if (m_dirToTarget.magnitude > failDistance) {
                AddReward(-1);
                EndEpisode();
            }
        }
    }

    /// <summary>
    /// Reward moving towards target & Penalize moving away from target.
    /// </summary>
    void RewardFunctionMovingTowards() {
        var movingTowardsDot = Vector3.Dot(playerRB.velocity, m_dirToTarget.normalized);
        AddReward(0.03f * movingTowardsDot);
    }

    /// <summary>
    /// Reward facing target & Penalize facing away from target
    /// </summary>
    void RewardFunctionFacingTarget()
    {
        var facingDot = Vector3.Dot(m_dirToTarget.normalized, hffPlayer.transform.forward);
        AddReward(0.01f * facingDot);
    }

    /// <summary>
    /// Existential penalty for time-constrained tasks.
    /// </summary>
    void RewardFunctionTimePenalty() {
        AddReward(-0.001f);
    }

    public override void OnEpisodeBegin()
    {
        ResetSimInputs();
        // clear any momentum from the rigidbody:
        playerRB.velocity = Vector3.zero;
        playerRB.angularVelocity = Vector3.zero;
        playerRB.isKinematic = true;
        hffPlayer.transform.position = spawnpoint.position + 10 * Vector3.up;
        playerRB.isKinematic = false;
        playerRB.velocity = Vector3.zero;
        playerRB.angularVelocity = Vector3.zero;
        // reset the movable Parts of the puzzle:
        if (resettableParts != null)
        {
            if (clonedParts != null)
            {
                Destroy(clonedParts);
            }
            resettableParts.SetActive(false);
            StartCoroutine(CloneResettableParts());
        }
    }

    private IEnumerator CloneResettableParts()
    {
        // simply wait a frame to avoid physics-engine chaos
        yield return null;
        clonedParts = Instantiate(resettableParts, resettableParts.transform.parent);
        clonedParts.SetActive(true);
    }

    // Unity callbacks to establish the simulated controller:

    public void Awake()
    {
        hffInput = new HFFSimInput();
        hffInput.ConnectController();
        Debug.Log("Simulated Controller connected.");
    }

    public void OnApplicationQuit() {
        hffInput.DisconnectController();
        Debug.Log("Simulated Controller disconnected.");
    }

    public void Start()
    {
        //Debug.Log("starting input simulation!");
        //StartCoroutine(this.TestSimulateInput());
    }

    private IEnumerator TestSimulateInput() {
        yield return new WaitForSeconds(12);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("simulating input start");
            hffInput.Look(32000, 32000);
            yield return new WaitForSeconds(1f);
            Debug.Log("simulating input end");
            hffInput.Look(0, 0);
        }
    }
}
