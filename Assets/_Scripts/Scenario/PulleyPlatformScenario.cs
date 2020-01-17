using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyPlatformScenario : PulleyScenarioManager {

    public override void BuildScenario()
    {
        CreateVariedRope();
        CreatePulleyWheelSetup();
    }
}
