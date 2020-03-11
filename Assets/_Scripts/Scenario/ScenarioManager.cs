using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScenarioManager : MonoBehaviour {

    #region Variables
    public Vector3 centralLocation;
    public bool replaceWidthByDM = false;

    protected GameObject creationManager;
    protected GameObject scenarioParent;
    protected System.Random rng;
    protected float scenarioWidth;
    protected float scenarioLength;
    protected float effectiveScenarioLength;

    #endregion

    #region Methods

    private void Awake()
    {
        // Ensures every object inheriting from ScenarioManager has a reference to the 
        // creationManager gameObject that holds all the Create classes
        creationManager = DemoManager.Instance.creationManager;
    }
    
    public void SetScenarioParent(GameObject parent)
    {
        scenarioParent = parent;
    }

    public void SetRNG(System.Random psuedoRandom)
    {
        rng = psuedoRandom;
    }

    public void SetScenarioWidth(float width)
    {
        scenarioWidth = width;
    }

    public void SetScenarioLength(float length)
    {
        scenarioLength = length;
        effectiveScenarioLength = length / 2;
    }

    /*
     * Allows any ScenarioManager type objects to set any pair of parameters according to the scenarioWidth 
     * passed down from the DemoManager.
     * The passed parameter values are directly changed since this method uses a direct reference.
     * Contains extra calculations to accurately determine how to use scenarioWidth.
     */
    protected void ReplaceParametersWithDMWidth(ref float minValue, ref float maxValue)
    {
        if (replaceWidthByDM)
        {
            Debug.Log("Width override enacted.");
            minValue = -scenarioWidth / 2;
            maxValue = scenarioWidth / 2;
        }
    }

    public abstract void BuildScenario();


    #endregion
}
