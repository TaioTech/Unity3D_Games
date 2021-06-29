using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {



	public GameObject diffView;
	public GameObject highView;
	public GameObject mainMenuView;



	public void DifficultyView () {
		mainMenuView.SetActive (false);
		highView.SetActive (false);
		diffView.SetActive (true);
	}

	public void HighscoreView() {
		mainMenuView.SetActive (false);
		diffView.SetActive (false);
		highView.SetActive (true);
	}

	public void BackToMainMenu() {
		diffView.SetActive (false);
		highView.SetActive (false);
		mainMenuView.SetActive (true);
	}



}
