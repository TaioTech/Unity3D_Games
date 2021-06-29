using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Taiote {
	public class PlayerCollision : MonoBehaviour {

		public PlayerMovement movement;
		public PlayerStats stats;
		public GameManager manager;

		void OnCollisionEnter(Collision collisionInfo) {
			
			if (collisionInfo.collider.tag == "Obstacle") {
				
				stats.health--;
				PlayerPrefs.SetInt ("Health", stats.health);
				movement.DampenForwardForce();

				if (stats.health <= 0) {
					manager.restartLevel = true;
					FindObjectOfType<GameManager> ().EndGame ();
				} else {
					manager.restartLevel = false;
				}


			}
		}
	}
}

