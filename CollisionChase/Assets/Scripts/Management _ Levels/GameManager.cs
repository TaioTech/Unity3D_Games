using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Taiote {
	public class GameManager : MonoBehaviour {

		public PlayerStats stats;

		public float completeDelay = 2f;
		public float restartDelay = 2f;

		public int currentFloor;

		public int playerLevel;
		public int totalExperience;
		public int previousLevelExperience;

		//UI VIEW
		public GameObject completeLevelUI;
		public GameObject failureLevelUI;


		public Transform playerPrefab;
		public Camera mainCamera;
		public Transform SpawnPoint;

		//UI
		public bool healthChanged = false;
		public GameObject ticker;
		public bool restartLevel;

		void Start() {

			Instantiate (playerPrefab.gameObject, SpawnPoint.position, Quaternion.identity);
			Instantiate (mainCamera, SpawnPoint.position, Quaternion.identity);

			if (SceneManager.GetActiveScene ().buildIndex == 1) {
				ResetExperience ();
			} else {
				totalExperience = PlayerPrefs.GetInt ("Experience");
			}

			ResetHealth ();
			currentFloor = SceneManager.GetActiveScene ().buildIndex;
		}

		void Update() {

		}
			
		public void CompleteLevel() {

			//Stop Counting
			ticker.GetComponent<ScoreTicker>().StopCounting();

			//Track experience to manager
			previousLevelExperience = (int)ticker.GetComponent<ScoreTicker>().scoreInt;
			totalExperience += previousLevelExperience;
			//Save Experience & Highscores variables
			PlayerPrefs.SetInt ("PastExperience", previousLevelExperience);
			PlayerPrefs.SetInt ("Experience", totalExperience);
			if (PlayerPrefs.GetInt ("Highscore", 0) < totalExperience)
				PlayerPrefs.SetInt ("Highscore", totalExperience);
			
			Debug.Log ("LEVEL WON!");
			Debug.Log ("Total experience is: " + totalExperience);
			completeLevelUI.SetActive (true);
			Invoke ("ProgressToNextLevel", completeDelay);
		}
			
		public void EndGame () {


			ticker.GetComponent<ScoreTicker>().ResetScoreOffset ();
			//Display Failure Message
			failureLevelUI.SetActive (true);

			ResetExperience();
			ResetHealth ();

			//Restart same level, may need to change here
			Debug.Log ("GAME OVER");
			Debug.Log ("Congratulations. You made it to floor " + currentFloor + ".");
			//Debug.Log ("You made it to level " + playerLevel);

			if (restartLevel)
				Invoke ("Restart", restartDelay);
			else
				Invoke ("GoToMainMenu", restartDelay);
		}

		public void GoToMainMenu() {
			failureLevelUI.SetActive (false);
			SceneManager.LoadScene ("Start Menu");
		}

		public void Restart() {
			failureLevelUI.SetActive (false);
			playerPrefab.position.Set (0, 1, 0);
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
			//SceneManager.GetActiveScene ().name
		}

		public void ProgressToNextLevel() {
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
		}

		public void ResetExperience() {
			totalExperience = 0;
			previousLevelExperience = 0;
			PlayerPrefs.DeleteKey ("Experience");
			PlayerPrefs.DeleteKey ("PastExperience");
		}

		public void ResetHealth() {
			stats.health = stats.startHealth;
			PlayerPrefs.SetInt ("Health", stats.health);
			PlayerPrefs.SetInt ("StartingHealth", stats.startHealth);
		}
	}
}