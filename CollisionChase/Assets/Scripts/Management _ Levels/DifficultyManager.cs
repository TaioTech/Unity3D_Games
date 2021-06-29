using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Taiote {
	public class DifficultyManager : MonoBehaviour {

		public void EasyButtonPress() {
			SetDifficulty (Difficulty.EASY);
		}

		public void MediumButtonPress() {
			SetDifficulty (Difficulty.MEDIUM);
		}

		public void HardButtonPress() {
			SetDifficulty (Difficulty.HARD);
		}

		public void SurvivalButtonPress() {
			SetDifficulty (Difficulty.SURVIVAL);
		}


		public void SetDifficulty(Difficulty chosen) {
			switch (chosen) {
			case Difficulty.EASY:
				PlayerPrefs.SetString ("Difficulty", "EASY");
				break;
			case Difficulty.MEDIUM:
				PlayerPrefs.SetString ("Difficulty", "MEDIUM");
				break;
			case Difficulty.HARD:
				PlayerPrefs.SetString ("Difficulty", "HARD");
				break;
			case Difficulty.SURVIVAL:
				PlayerPrefs.SetString ("Difficulty", "SURVIVAL");
				break;

			}
		}

		public enum Difficulty {
			EASY,
			MEDIUM,
			HARD,
			SURVIVAL
		}
	}
}
