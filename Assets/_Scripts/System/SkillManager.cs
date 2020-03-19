using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

    // Skills
    [Header("Skills")]
    [SerializeField] private Skill push = new Skill("Push");
    [SerializeField] private Skill pull = new Skill("Pull");
    [SerializeField] private Skill pickupObject = new Skill("PickupObject");
    [SerializeField] private Skill interact = new Skill("Interact");
    [SerializeField] private Skill openDoor = new Skill("OpenDoor");
    [SerializeField] private Skill jump = new Skill("Jump");
    [SerializeField] private Skill liftSelf = new Skill("LiftSelf");
    [SerializeField] private Skill reach = new Skill("Reach");
    [SerializeField] private Skill grab = new Skill("Grab");
    [SerializeField] private Skill hold = new Skill("Hold");

    // Concepts
    [Header("Concepts")]
    [SerializeField] private Skill gravity = new Skill("Gravity");
    [SerializeField] private Skill ramp = new Skill("Ramp");
    [SerializeField] private Skill roll = new Skill("Roll");
    [SerializeField] private Skill createPlatform = new Skill("CreatePlatform");
    [SerializeField] private Skill momentum = new Skill("Momentum");
    [SerializeField] private Skill pulley = new Skill("Pulley");
    [SerializeField] private Skill resourceManagement = new Skill("ResourceManagement");
    [SerializeField] private Skill memory = new Skill("Memory");
    [SerializeField] private Skill applyForce = new Skill("ApplyForce");
    [SerializeField] private Skill forceMassAcceleration = new Skill("ForceMassAcceleration");
    [SerializeField] private Skill breakableObject = new Skill("BreakableObject");
    [SerializeField] private Skill lever = new Skill("Lever");
    [SerializeField] private Skill torque = new Skill("Torque");
    [SerializeField] private Skill reduceMass = new Skill("ReduceMass");
    [SerializeField] private Skill increaseMass = new Skill("IncreaseMass");

    public Skill[] originalSkillSet;
    public Skill[] skillSet;

    public List<List<Skill>> skillSetsUsed;
    public List<Skill[]> recordedSkillSets;

    private void Awake()
    {
        originalSkillSet = CreateSkillList();
        skillSet = originalSkillSet;
        Debug.Log("Push name is: " + push.Name + "and Push useSkill is: " + push.UseSkill);
        Debug.Log("Push priority is: " + push.Priority);
    }

    /*
     * Outputs a bool array with all the skills and concepts decided on in the 
     * editor.
     */
    private Skill[] CreateSkillList()
    {
        Skill[] allSkills = new Skill[]
        {
            push,
            pull,
            pickupObject,
            interact,
            openDoor,
            jump,
            liftSelf,
            reach,
            grab,
            hold,
            gravity,
            ramp,
            roll,
            createPlatform,
            momentum,
            pulley,
            resourceManagement,
            memory,
            applyForce,
            forceMassAcceleration,
            breakableObject,
            lever,
            torque,
            reduceMass,
            increaseMass
        };

        return allSkills;
            
    }

    /*
     * Each scenario can add their own individual skill set that they specifically used in their generation process 
     * for recording purposes.
     */
    public void AddSkillSetToList(List<Skill> skillList)
    {
        skillSetsUsed.Add(skillList);
    }

    /*
     *  Add a snapshot of how the current skill set array looks to a list which will record all the various states
     *  of the skill set as the generation processes.
     */
    public void RecordSkillSet(Skill[] set)
    {
        recordedSkillSets.Add(set);
    }
}
