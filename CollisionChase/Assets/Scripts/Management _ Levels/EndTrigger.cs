using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Taiote {
	public class EndTrigger : MonoBehaviour {

		public GameManager gameManager;


		void OnTriggerEnter (Collider col) {
			if ( col.transform.tag == "Player")
				gameManager.CompleteLevel ();
		}
	}
}
