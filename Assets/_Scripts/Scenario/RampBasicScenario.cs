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

        Debug.Log("s scenario completed");
        OutputLog();
    }

    public void OutputLog()
    {
        Debug.Log("minLength: " + minLength + 
            " maxLength: " + maxLength + 
            " minWidth: " + minWidth + 
            " maxWidth: " + maxWidth + 
            " minHeight: " + minHeight +
            " maxHeight: " + maxHeight + 
            " minStart: " + minStart + 
            " maxStart: " + maxStart);
    }
}
