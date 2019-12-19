using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyHeavyDoorScenario : PulleyScenarioManager {

    // Will contain specific information for the Heavy Door variation of a Pulley Scenario
    // This is the final step in the Scenario inheritance ladder, and will also be responsible 
    // for interacting with Create classes

    // 1) Determine Create classes needed
    // 2) Pass parameterization data on to Create classes
    // 3) Instantiate objects created by Create classes
    // 4) Get updated parameterization data from Create classes
    // 5) Repeat 2 - 4 as needed
    // 6) Activate everything within instantiated objects (to control timing of internal HFFWS scripts)

    public override void BuildScenario()
    {
        CreateVariedRope();
        CreatePulleyWheelSetup();
    }

}
