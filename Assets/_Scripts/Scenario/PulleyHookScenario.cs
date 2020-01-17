using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyHookScenario : PulleyScenarioManager {

    public override void BuildScenario()
    {
        CreateVariedRope();
        CreatePulleyWheelSetup();
    }
}
