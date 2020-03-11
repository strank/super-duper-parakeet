using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampBasicScenario : RampScenarioManager {

    //public override void BuildScenario()
    //{
    //    CreateRamp();
    //    CreateObstacle();
    //}

    public override void BuildScenario()
    {
        CreateRamp();
        CreateScenarioWall();
    }
}
