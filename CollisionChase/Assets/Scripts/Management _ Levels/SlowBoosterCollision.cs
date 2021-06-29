using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Taiote {
	public class SlowBoosterCollision : MonoBehaviour {

		void OnTriggerEnter(Collider other) {
			if (other.transform.tag == "Player") {
				other.transform.GetComponent<PlayerMovement> ().SlowBooster ();
				Destroy (this.gameObject);
			}
		}
	}
}
