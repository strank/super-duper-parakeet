using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour {

    public System.Random RNG { get { return rng; } }
    private System.Random rng;

    public GameObject ScenarioParent { get { return scenarioParent; } }
    private GameObject scenarioParent;

    public Vector3 CentralLocation { get { return centralLocation; } }
    private Vector3 centralLocation;

    private List<Skill> skillSet;

    public Scenario(System.Random _rng, GameObject _scenarioParent, Vector3 _centralLocation)
    {
        rng = _rng;
        scenarioParent = _scenarioParent;
        centralLocation = _centralLocation;
        skillSet = new List<Skill>();
    }

    public void RecordSkill(Skill skill)
    {
        skillSet.Add(skill);
    }
}
