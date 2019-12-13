using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScenarioManager : MonoBehaviour {

    #region Variables
    public Vector3 centralLocation;
    protected GameObject creationManager;

    #endregion

    #region Methods

    private void Awake()
    {
        // Ensures every object inheriting from ScenarioManager has a reference to the 
        // creationManager gameObject that holds all the Create classes
        creationManager = DemoManager.Instance.creationManager;
    }

    public abstract void BuildScenario();


    #endregion
}
