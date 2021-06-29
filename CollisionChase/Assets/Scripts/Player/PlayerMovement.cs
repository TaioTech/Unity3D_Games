using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Taiote {
	public class PlayerMovement : MonoBehaviour {

		// CACHE TYPES
		public Rigidbody rb;
		public PlayerStats stats;


		// FORCES
		public bool addForwardForce;
		private float maxForwardForce;
		private float incrementPace = 1.005f;
		private float startingForce;
		private float forwardForce;
		private float horizontalForce;
		private float jumpForce = 500f;
		// Use this for initialization
		void Start () {
			addForwardForce = true;
			startingForce = stats.startFF;
			forwardForce = startingForce;
			maxForwardForce = stats.maxFF;
			horizontalForce = stats.startHF;
		}

		void Update() {

			if (stats.currentF != forwardForce)
				stats.currentF = forwardForce;

			if (stats.currentH != horizontalForce)
				stats.currentH = horizontalForce;


		}

		// Update is called once per frame
		void FixedUpdate () {
			
			if (addForwardForce) {
				forwardForce *= incrementPace;
				if (forwardForce > maxForwardForce) {
					forwardForce = maxForwardForce;
				}
				//Adding a forward force constantly
				rb.AddForce (0, 0, forwardForce * Time.deltaTime);
			}

			if (Input.GetKey("d")) {
				rb.AddForce (horizontalForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
			}

			if (Input.GetKey("a")) {
				rb.AddForce (-horizontalForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
			}


			if (rb.position.y < -2f) 
				FindObjectOfType<GameManager> ().EndGame ();
		}

		public void SlowBooster() {
			forwardForce = forwardForce / 2;
		}

		public void TurnOffForwardForce() {
			addForwardForce = false;
		}

		public void DampenForwardForce() {
			forwardForce = forwardForce - (forwardForce / stats.dampFPercent);
		}

		public void JumpBoost() {
			rb.AddForce (0, jumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
		}

	}
}