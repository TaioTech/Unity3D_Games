using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Taiote {
	public class FloorTicker : MonoBehaviour {

		public GameManager manager;
		public Text floorText;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			floorText.text = manager.currentFloor.ToString ();
		}
	}
}