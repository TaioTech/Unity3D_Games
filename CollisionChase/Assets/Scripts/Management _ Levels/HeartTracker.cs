using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Taiote {
	public class HeartTracker : MonoBehaviour {

		public GameManager manager;

		public Sprite heartFull;
		public Sprite heartEmpty;
		public Image[] hearts = new Image[5];

		private int heartCount;
		private int emptyCount;

		void Start() {
			heartCount = 0;
			emptyCount = 0;
			PlayerPrefs.SetInt ("Health", PlayerPrefs.GetInt("StartingHealth"));

			SetStartingHearts ();
		}

		void Update() {

			if (PlayerPrefs.GetInt ("StartingHealth") < PlayerPrefs.GetInt ("Health")) {
				PlayerPrefs.SetInt ("Health", PlayerPrefs.GetInt ("StartingHealth"));
			}

			if (PlayerPrefs.GetInt("Health") != heartCount) {
				SetHearts ();
			}
		}

		void SetHearts() {
			
			heartCount = 0;
			emptyCount = 0;


			for (int i = 0; i < PlayerPrefs.GetInt("Health", PlayerPrefs.GetInt("StartingHealth")); i++) {
				hearts [i].sprite = heartFull;
				heartCount++;

			}


			for (int j = heartCount; j < 5; j++) {
				hearts [j].sprite = heartEmpty;
				emptyCount++;

			}

		}//End of SetHearts

		void SetStartingHearts() {

			heartCount = 0;
			emptyCount = 0;

			for (int i = 0; i < PlayerPrefs.GetInt("StartingHealth"); i++) {
				hearts [i].sprite = heartFull;
				heartCount++;
			}

			for (int j = heartCount; j < 5; j++) {
				hearts [j].sprite = heartEmpty;
				emptyCount++;
			}
		}





	}
}