using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Taiote {
	public class LevelTicker : MonoBehaviour {

		public GameManager manager;
		public ScoreTicker score;
		public Text levelText;

		public int level;
		private int experience;
		private int expToLevel;
		private float expIncrement = 1.15f;

		// Use this for initialization
		void Start () {
			if (SceneManager.GetActiveScene ().buildIndex == 1) {
				level = 1;
				expToLevel = 100;
				PlayerPrefs.SetInt ("Level", level);
				PlayerPrefs.SetInt ("ExpToLevel", expToLevel);
			} else {
				level = PlayerPrefs.GetInt ("Level");
				expToLevel = PlayerPrefs.GetInt ("ExpToLevel");
			}
		}
		
		// Update is called once per frame
		void Update () {
			experience = score.experience;
			CheckForLevelUp ();
			SetTicker ();
		}

		void CheckForLevelUp() {
			if (experience > expToLevel) {
				LevelUp ();
			}
		}

		void LevelUp() {
			level++;
			PlayerPrefs.SetInt ("Level", level);

			expToLevel = expToLevel + (int)(expToLevel * expIncrement);
			PlayerPrefs.SetInt ("ExpToLevel", expToLevel);

		}

		void SetTicker() {
			levelText.text = level.ToString();
		}
	}
}