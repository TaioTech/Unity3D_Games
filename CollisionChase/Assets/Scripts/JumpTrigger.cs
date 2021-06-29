using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Taiote {
	public class JumpTrigger : MonoBehaviour {

		void OnTriggerEnter(Collider other) {
			if (other.transform.tag == "Player") {
				other.transform.GetComponent<PlayerMovement> ().JumpBoost ();
			}
		}
	}
}
