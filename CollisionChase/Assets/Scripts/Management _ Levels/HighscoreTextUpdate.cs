using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTextUpdate : MonoBehaviour {

	public Text highscore;

	void Start() {
		highscore.text = PlayerPrefs.GetInt ("Highscore", 0).ToString("0");
	}

}
