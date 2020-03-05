using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverOneScenario : LeverScenarioManager {

    //TODO: NEEDS FIXED, VERY BROKEN CURRENTLY

    public override void BuildScenario()
    {
        CreateSeesaw();
        CreateObstacle();
    }

    private void CreateSeesaw()
    {
        CreateSeesaw seesaw = creationManager.GetComponent<CreateSeesaw>();

        seesaw.BuildSeesaw(minStart, maxStart, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight,
            scenarioParent, rng);

        // TODO: Determine which parameters to pass on for obstacle information

    }
}
