using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Taiote {
	public class PlayerStats : MonoBehaviour {

		public int health;
		public float experience;
		public float level;
		public int startHealth;

		public float startFF;
		public float startHF;
		public float currentF;
		public float currentH;
		public float maxFF;
		public float dampFPercent;
		public float dampHPercent;

		public float Experience {
			get { return experience; }
			set { experience = value; }
		}
			

		// Use this for initialization
		void Start () {

			if (PlayerPrefs.GetString ("Difficulty") != null) {
				if (PlayerPrefs.GetString ("Difficulty") == "EASY") {
					startHealth = 5;
					PlayerPrefs.SetInt ("StartingHealth", startHealth);
				} 
				if (PlayerPrefs.GetString ("Difficulty") == "MEDIUM") {
					startHealth = 3;
					PlayerPrefs.SetInt ("StartingHealth", startHealth);
				}
				if (PlayerPrefs.GetString ("Difficulty") == "HARD") {
					startHealth = 1;
					PlayerPrefs.SetInt ("StartingHealth", startHealth);
				}
				if (PlayerPrefs.GetString ("Difficulty") == "SURVIVAL") {
					startHealth = 5;
					PlayerPrefs.SetInt ("StartingHealth", startHealth);
				} 
			} else {
				startHealth = 5;
			}

			health = startHealth;
			currentF = startFF;
			currentH = startHF;
		}
		
		// Update is called once per frame
		void Update () {
			
		}
			
	}
}
