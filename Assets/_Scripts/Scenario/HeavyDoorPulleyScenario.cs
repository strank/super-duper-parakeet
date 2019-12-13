using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyDoorPulleyScenario : PulleyScenarioManager {

    // Will contain specific information for the Heavy Door variation of a Pulley Scenario
    // This is the final step in the Scenario inheritance ladder, and will also be responsible 
    // for interacting with Create classes

    //private void Awake()
    //{
        // Determine Create classes needed
        // Pass parameterization data on to create classes
        // Instantiate objects created by Create classes
        // Activate everything within instantiated objects (to control timing of internal HFFWS scripts)
    //}

    public override void BuildScenario()
    {
        CreateVariedRope();
        CreatePulleyWheelSetup();
    }
}
