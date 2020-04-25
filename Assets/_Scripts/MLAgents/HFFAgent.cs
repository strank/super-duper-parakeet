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
    private short[] xLookChoices = new short[5] { 0, -short.MaxValue, -short.MaxValue / 2, short.MaxValue / 2, short.MaxValue };
    private short[] yLookChoices = new short[5] { 0, -short.MaxValue, -short.MaxValue / 2, short.MaxValue / 2, short.MaxValue };
    private short[] xWalkChoices = new short[5] { 0, -short.MaxValue, -short.MaxValue / 2, short.MaxValue / 2, short.MaxValue };
    private short[] yWalkChoices = new short[5] { 0, -short.MaxValue, -short.MaxValue / 2, short.MaxValue / 2, short.MaxValue };
    private GameObject hffPlayer;
    private Human hffHumanScript;
    private HumanControls hffHumanControlsScript;
    private Rigidbody playerRB;
    private Vector3 mDirToTarget;
    const float SPAWN_HEIGHT = 10;

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
        hffHumanScript = hffPlayer.GetComponent<Human>();
        hffHumanControlsScript = hffPlayer.transform.parent.gameObject.GetComponent<HumanControls>();
        playerRB = hffPlayer.GetComponent<Rigidbody>();
        if (spawnpoint == null) {
            spawnpoint = GameObject.Find("InitialSpawnpoint").transform;
        }
        if (target == null) {
            target = GameObject.Find("Checkpoint").GetComponent<Collider>();
        }
        mDirToTarget = target.gameObject.transform.position - hffPlayer.transform.position;
    }

    private void ResetSimInputs() {
        hffInput.Look(0, 0);
        hffInput.Walk(0, 0);
        hffInput.JumpSet(false);
        hffInput.LeftGrabSet(0x00);
        hffInput.RightGrabSet(0x00);
        hffInput.SubmitInput();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(mDirToTarget);
        sensor.AddObservation(hffHumanScript.onGround);
        // maybe use those later, when we add grabbing back in:
        //sensor.AddObservation(hffHumanScript.hasGrabbed);
        //sensor.AddObservation(hffHumanScript.isClimbing);

        // and later use raycast observations?
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // The action space is discrete. 7 branches:
        // xLook: 5, yLook: 5, xWalk: 5, yWalk: 5, jump: 2, leftgrab: 2, rightgrab: 2
        // 5 options: 0 nothing, 1-4 from far negative to far positive
        // 2 options: 0 off, 1 on
        hffInput.Look(xLookChoices[(int) vectorAction[0]], yLookChoices[(int) vectorAction[1]]);
        hffInput.Walk(xWalkChoices[(int) vectorAction[2]], yWalkChoices[(int) vectorAction[3]]);
        hffInput.JumpSet((int) vectorAction[4] == 1);
        //hffInput.LeftGrabSet((byte) ((int) vectorAction[5] == 1 ? 0xFF : 0x00));
        //hffInput.RightGrabSet((byte) ((int) vectorAction[6] == 1 ? 0xFF : 0x00));
        hffInput.SubmitInput();
    }

    public override float[] Heuristic() {
        // set some keys to trigger all the possible actions for testing, to see if they make sense
        var action = new float[7];
        action[0] = Input.GetKey(KeyCode.F) ? 2f : (Input.GetKey(KeyCode.H) ? 3f :
            Input.GetKey(KeyCode.R) ? 1f : (Input.GetKey(KeyCode.U) ? 4f : 0f)); // U should by Y, but HFF already takes Y as collapse!
        action[1] = Input.GetKey(KeyCode.G) ? 2f : (Input.GetKey(KeyCode.T) ? 3f :
            Input.GetKey(KeyCode.B) ? 1f : (Input.GetKey(KeyCode.Alpha5) ? 4f : 0f));
        action[2] = Input.GetKey(KeyCode.K) ? 2f : (Input.GetKey(KeyCode.Semicolon) ? 3f :
            Input.GetKey(KeyCode.I) ? 1f : (Input.GetKey(KeyCode.P) ? 4f : 0f));
        action[3] = Input.GetKey(KeyCode.L) ? 2f : (Input.GetKey(KeyCode.O) ? 3f :
            Input.GetKey(KeyCode.Period) ? 1f : (Input.GetKey(KeyCode.Alpha9) ? 4f : 0f));
        action[4] = Input.GetKey(KeyCode.J) ? 1.0f : 0.0f;
        action[5] = Input.GetKey(KeyCode.N) ? 1.0f : 0.0f;
        action[6] = Input.GetKey(KeyCode.M) ? 1.0f : 0.0f;
        return action;
    }

    public void FixedUpdate() {
        mDirToTarget = target.gameObject.transform.position - hffPlayer.transform.position;
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
            if (mDirToTarget.magnitude > failDistance) {
                AddReward(-1);
                EndEpisode();
            }
        }
    }

    void Update()
    {
        // This shows that the targetDirection attribute better reflects the heading of the character than .forward:
        //Debug.Log("Forward " + hffPlayer.transform.forward + " targetDir " + hffHumanScript.targetDirection);
        // Debug output to test how sensible the reward functions are:
        //Debug.Log("Moving Towards Reward: " + 0.03f * Vector3.Dot(playerRB.velocity, mDirToTarget.normalized));
        //Debug.Log("Facing Target Reward:  " + 0.01f * Vector3.Dot(mDirToTarget.normalized, hffHumanScript.targetDirection));
    }

    /// <summary>
    /// Reward moving towards target & Penalize moving away from target.
    /// </summary>
    void RewardFunctionMovingTowards() {
        var movingTowardsDot = Vector3.Dot(playerRB.velocity, mDirToTarget.normalized);
        AddReward(0.03f * movingTowardsDot);
    }

    /// <summary>
    /// Reward facing target & Penalize facing away from target
    /// </summary>
    void RewardFunctionFacingTarget()
    {
        var facingDot = Vector3.Dot(mDirToTarget.normalized, hffHumanScript.targetDirection);
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
        // Use the play-dead button in the hope that this will reduce the movement that remains into the next episode:
        hffInput.PlayDeadSet(true); // that should remove any momentum from the HFF input system?
        hffInput.SubmitInput();
        StartCoroutine(UnsetPlayDead());
        // Try to tell the HFF character to ignore default mouse/keyboard input:
        hffHumanControlsScript.allowMouse = false;
        // clear any momentum from the rigidbody:
        playerRB.velocity = Vector3.zero;
        playerRB.angularVelocity = Vector3.zero;
        playerRB.isKinematic = true;
        hffPlayer.transform.position = spawnpoint.position + SPAWN_HEIGHT * Vector3.up;
        playerRB.isKinematic = false;
        // probably not needed to zero those again:
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
            StartCoroutine(CloneResettableParts()); // next frame
        }
    }

    private IEnumerator CloneResettableParts()
    {
        // simply wait a frame to avoid physics-engine chaos
        yield return null;
        clonedParts = Instantiate(resettableParts, resettableParts.transform.parent);
        clonedParts.SetActive(true);
    }

    private IEnumerator UnsetPlayDead()
    {
        yield return null; // wait a frame so the button can register
        hffInput.PlayDeadSet(false);
        hffInput.SubmitInput();
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

}
