using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Taiote {
	public class ScoreTicker : MonoBehaviour {

		public Transform player;
		public Text textScore;
		public float scoreInt;
		public GameManager manager;
		public int experience;
		private string scoreText;
		private bool keepCounting;
		private float scoreOffset;

		// Use this for initialization
		void Start () {
			keepCounting = true;
			manager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
			scoreOffset = PlayerPrefs.GetInt ("Experience", 0);
			scoreText = (scoreOffset + player.position.z).ToString ("0");
			textScore.text = scoreText;
		}

		// Update is called once per frame
		void Update () {


			player = GameObject.FindGameObjectWithTag ("Player").transform;
			scoreOffset = PlayerPrefs.GetInt ("Experience", 0);

			if (keepCounting) {
				
				scoreInt = player.position.z;
				experience = (int)(scoreInt + scoreOffset);
				scoreText = (scoreOffset + scoreInt).ToString ("0");
				textScore.text = scoreText;
			}
		}

		public void StopCounting() {
			keepCounting = false;
		}

		public void StartCountingAgain(float startingPoint) {
			keepCounting = true;
			scoreOffset = startingPoint;
		}

		public void ResetScoreOffset() {
			scoreOffset = 0;
		}
	}
}