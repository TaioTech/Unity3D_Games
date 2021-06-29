using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;			//used for the Enum class
using UnityEngine.SceneManagement;

namespace Behavior.Health {
	public class CharacterGenerator : MonoBehaviour {

		private PlayerCharacter _toon;
		private const int STARTING_POINTS = 350;
		private const int MIN_STARTING_ATTRIBUTE_VALUE = 10;
		private const int STARTING_POINTS_VALUE = 50;
		private int pointsLeft;

		private const int OFFSET = 5;
		private const int LINE_HEIGHT = 20;

		private const int STAT_LABEL_WIDTH = 100;
		private const int BASEVALUE_LABEL_WIDTH = 30;
		private const int BUTTON_WIDTH = 20;
		private const int BUTTON_HEIGHT = 20;

		private int statStartingPos = 40;

		public GUIStyle plusStyle;
		public GUIStyle minusStyle;
		public GUIStyle createStyle;
		public GUISkin mySkin;

		public GameObject playerPrefab;

		// Use this for initialization
		void Start () {

			GameObject pc = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

			pc.name = "pc";

			//_toon = new PlayerCharacter ();
			//_toon.Awake ();
			_toon = pc.GetComponent<PlayerCharacter>();

			pointsLeft = STARTING_POINTS;
			for(int cnt = 0; cnt < Enum.GetValues(typeof(AttributeName)).Length; cnt++) {
				_toon.GetPrimaryAttribute (cnt).BaseValue = STARTING_POINTS_VALUE;
				pointsLeft -= (STARTING_POINTS_VALUE - MIN_STARTING_ATTRIBUTE_VALUE);
			}
			_toon.StatUpdate ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void OnGUI() {
			DisplayName ();
			DisplayPointsLeft ();
			DisplayAttributes ();
			DisplayVitals ();
			DisplaySkills ();

			if (_toon.Name == "" || pointsLeft > 0)
				DisplayCreateLabel ();
			else
				DisplayCreateButton ();

		}

		private void DisplayName() {
			GUI.Label (new Rect(10, 10, 50, 25), "Name:");
			_toon.Name = GUI.TextField (new Rect(65, 10, 100, 25), _toon.Name);
		}

		private void DisplayAttributes() {
			for(int cnt = 0; cnt < Enum.GetValues(typeof(AttributeName)).Length; cnt++) {
				GUI.Label (new Rect(OFFSET,										//x
									statStartingPos + (cnt * LINE_HEIGHT),		//y
									STAT_LABEL_WIDTH,							//width
									LINE_HEIGHT									//height
							), ((AttributeName)cnt).ToString() );

				GUI.Label (new Rect(STAT_LABEL_WIDTH + OFFSET,					//x
									statStartingPos + (cnt * LINE_HEIGHT),		//y
									BASEVALUE_LABEL_WIDTH,						//width
									LINE_HEIGHT									//height
							), _toon.GetPrimaryAttribute(cnt).AdjustedBaseValue.ToString() );

				if (GUI.Button (new Rect (OFFSET + STAT_LABEL_WIDTH + BASEVALUE_LABEL_WIDTH,	//x
										  statStartingPos + (cnt * BUTTON_HEIGHT),				//y
										  BUTTON_WIDTH,											//width
										  BUTTON_HEIGHT											//height
							), "-", minusStyle)) {
					if (_toon.GetPrimaryAttribute(cnt).BaseValue > MIN_STARTING_ATTRIBUTE_VALUE) {
						_toon.GetPrimaryAttribute (cnt).BaseValue--;
						pointsLeft++;
						_toon.StatUpdate ();
					}
				}
				if (GUI.Button (new Rect (OFFSET + STAT_LABEL_WIDTH + BASEVALUE_LABEL_WIDTH + BUTTON_WIDTH,		//x
										  statStartingPos + (cnt * BUTTON_HEIGHT),								//y
										  BUTTON_WIDTH,															//width
										  BUTTON_HEIGHT															//height
							), "+", plusStyle)) {
					if (pointsLeft > 0) {
						_toon.GetPrimaryAttribute (cnt).BaseValue++;
						pointsLeft--;
						_toon.StatUpdate ();
					}
				}
			}
		}

		private void DisplayVitals() {
			for(int cnt = 0; cnt < Enum.GetValues(typeof(VitalName)).Length; cnt++) {
				GUI.Label (new Rect(OFFSET,													//x
									statStartingPos + ((cnt + 7) * LINE_HEIGHT),			//y
									STAT_LABEL_WIDTH,										//width
									LINE_HEIGHT												//height
						), ((VitalName)cnt).ToString() );

				GUI.Label (new Rect(OFFSET + STAT_LABEL_WIDTH,								//x
									statStartingPos + ((cnt + 7) * LINE_HEIGHT),			//y
									BASEVALUE_LABEL_WIDTH,									//width
									LINE_HEIGHT												//height
						), _toon.GetVital(cnt).AdjustedBaseValue.ToString() );
			}
		}

		private void DisplaySkills() {
			for(int cnt = 0; cnt < Enum.GetValues(typeof(SkillName)).Length; cnt++) {
				GUI.Label (new Rect(OFFSET + STAT_LABEL_WIDTH + BASEVALUE_LABEL_WIDTH + BUTTON_WIDTH * 2 + OFFSET * 2,		//x
									statStartingPos + (cnt * LINE_HEIGHT),													//y
									STAT_LABEL_WIDTH,																		//width
									LINE_HEIGHT																				//height
						), ((SkillName)cnt).ToString() );

				GUI.Label (new Rect(OFFSET + STAT_LABEL_WIDTH + BASEVALUE_LABEL_WIDTH + BUTTON_WIDTH * 2 + OFFSET * 2 + STAT_LABEL_WIDTH,	//x
									statStartingPos + (cnt * LINE_HEIGHT),																	//y
									BASEVALUE_LABEL_WIDTH,																					//width
									LINE_HEIGHT																								//height
						), _toon.GetSkill(cnt).AdjustedBaseValue.ToString() );
			}
		}

		private void DisplayPointsLeft() {
			GUI.Label (new Rect(250,						//x
								10,							//y
								100,						//width
								25							//height
						), "Points Left: " + pointsLeft.ToString());
		}

		private void DisplayCreateLabel () {
			GUI.Label (new Rect (Screen.width / 2 - 50,									//x
								statStartingPos + (10) * LINE_HEIGHT,					//y
								100,													//width
								LINE_HEIGHT												//height
						), "Creating...", "Button");
		}

		private void DisplayCreateButton () 
		{
			
			if (GUI.Button (new Rect (Screen.width / 2 - 50, statStartingPos + (10) * LINE_HEIGHT, 100, LINE_HEIGHT), "Create", createStyle)) 
				{

					GameSettings gsScript = GameObject.Find("__GameSettings").GetComponent<GameSettings> ();

					//Change the current value of the vitals to the max modified value of that vital
					UpdateCurVitalValues();


					gsScript.SaveCharacterData ();


					SceneManager.LoadScene (1);
				}
		}

		private void UpdateCurVitalValues() {
			for (int cnt = 0; cnt < Enum.GetValues (typeof(VitalName)).Length; cnt++) {
				_toon.GetVital (cnt).CurValue = _toon.GetVital (cnt).AdjustedBaseValue;
			}
		}
	}
}