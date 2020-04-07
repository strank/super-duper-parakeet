using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Skill
{
	// Name of the Skill
	public string Name
	{
		get { return name; }
	}
	private string name;

	// Determine whether the Skill should be used for this current generation at all
	public bool UseSkill
	{
		get { return useSkill; }
	}
	[SerializeField]private bool useSkill;

	// Keep track of whether a Skill has been used in the generation process so far
	public bool Used
	{
		get { return used; }
	}
	private bool used;

	// Used to determine how likely a Skill should be used in each pass of the generation system
	public int Priority
	{
		get { return priority; }
	}
	[SerializeField]private int priority;

	public Skill(string _name, bool _useSkill, bool _used, int _priority)
	{
		name = _name;
		useSkill = _useSkill;
		used = _used;
		priority = _priority;
	}

	public Skill(string _name, bool _useSkill)
	{
		name = _name;
		useSkill = _useSkill;
		used = false;
		priority = 0;
	}

	public Skill(string _name)
	{
		name = _name;
		useSkill = false;
		used = false;
		priority = 0;
	}

	public void SetPriority(int value)
	{
		priority = value;
	}

	public void UsedSkill()
	{
		used = true;
	}

	
}
