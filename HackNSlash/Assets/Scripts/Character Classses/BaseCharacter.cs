﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;				//Added to access the enum class;

public class BaseCharacter : MonoBehaviour {

	private string _name;
	private int _level;
	private uint _freeExp;


	private Attribute[] _primaryAttribute;
	private Vital[] _vital;
	private Skill[] _skill;

	public void Awake() {
		_name = string.Empty;
		_level = 0;
		_freeExp = 0;

		_primaryAttribute = new Attribute[Enum.GetValues (typeof(AttributeName)).Length];
		_vital = new Vital[Enum.GetValues (typeof(VitalName)).Length];
		_skill = new Skill[Enum.GetValues (typeof(SkillName)).Length];

		SetupPrimaryAttribute ();
		SetupVitals ();
		SetupSkills ();
	}
		
	public string Name {
		get { return _name; }
		set { _name = value; }
	}

	public uint FreeExp {
		get { return _freeExp; }
		set { _freeExp = value; }
	}

	public void AddExp(uint exp) {
		_freeExp += exp;

		CalculateLevel ();
	}

	//take the average of all the players skills and assign that as the player level
	public void CalculateLevel() {

	}

	private void SetupPrimaryAttribute() {
		for (int cnt = 0; cnt < _primaryAttribute.Length; cnt++) {
			_primaryAttribute [cnt] = new Attribute ();
			_primaryAttribute [cnt].Name = ((AttributeName)cnt).ToString ();
		}
	}

	private void SetupVitals() {
		for (int cnt = 0; cnt < _vital.Length; cnt++) {
			_vital [cnt] = new Vital ();
		}

		SetupVitalModifiers ();
	}

	private void SetupSkills() {
		for (int cnt = 0; cnt < _skill.Length; cnt++) {
			_skill [cnt] = new Skill ();
		}

		SetupSkillModifiers ();
	}

	public Attribute GetPrimaryAttribute(int index) {
		return _primaryAttribute [index];
	}

	public Vital GetVital(int index) {
		return _vital [index];
	}

	public Skill GetSkill(int index) {
		return _skill [index];
	}

	private void SetupVitalModifiers() {
		//health
		//		ModifyingAttribute health = new ModifyingAttribute();
		//		health.attribute = GetPrimaryAttribute ((int)AttributeName.Constitution);
		//		health.ratio = .5f;
		//		GetVital ((int)VitalName.Health).AddModifier (health);
		GetVital((int)VitalName.Health).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Constitution), .5f));
		//energy
		GetVital((int)VitalName.Energy).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Constitution), 1));
		//mana
		GetVital((int)VitalName.Mana).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Willpower), 1));
	}

	private void SetupSkillModifiers() {
//		ModifyingAttribute MeleeOffenseModifier1 = new ModifyingAttribute ();
//		ModifyingAttribute MeleeOffenseModifier2 = new ModifyingAttribute ();
//
//		MeleeOffenseModifier1.attribute = GetPrimaryAttribute ((int)AttributeName.Might);
//		MeleeOffenseModifier1.ratio = .33f;
//
//		MeleeOffenseModifier2.attribute = GetPrimaryAttribute ((int)AttributeName.Nimbleness);
//		MeleeOffenseModifier2.ratio = .33f;
//
//		GetSkill ((int)SkillName.Melee_Offense).AddModifier (MeleeOffenseModifier1);
//		GetSkill ((int)SkillName.Melee_Offense).AddModifier (MeleeOffenseModifier2);

		//Melee Offense
		GetSkill((int)SkillName.Melee_Offense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Might), .33f));
		GetSkill((int)SkillName.Melee_Offense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Nimbleness), .33f));
		//Melee Defense
		GetSkill((int)SkillName.Melee_Defense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Speed), .33f));
		GetSkill((int)SkillName.Melee_Defense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Constitution), .33f));
		//Magic Offense
		GetSkill((int)SkillName.Magic_Offense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Concentration), .33f));
		GetSkill((int)SkillName.Magic_Offense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Willpower), .33f));
		//Magic Defense
		GetSkill((int)SkillName.Magic_Defense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Concentration), .33f));
		GetSkill((int)SkillName.Magic_Defense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Willpower), .33f));
		//Ranged Offense
		GetSkill((int)SkillName.Ranged_Offense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Concentration), .33f));
		GetSkill((int)SkillName.Ranged_Offense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Speed), .33f));
		//Ranged Defense
		GetSkill((int)SkillName.Ranged_Defense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Speed), .33f));
		GetSkill((int)SkillName.Ranged_Defense).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Nimbleness), .33f));
	}

	public void StatUpdate() {
		for (int cnt = 0; cnt < _vital.Length; cnt++) 
			_vital [cnt].Update ();
		for (int cnt = 0; cnt < _skill.Length; cnt++) 
			_skill [cnt].Update ();
	}
}
