using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Behavior.AI {
	public class LaserBeam : MonoBehaviour {



		public float speed = 5f;
		public Slider playerHealthbar;
		// Use this for initialization
		void Start () {
			playerHealthbar = FindObjectOfType<Slider> ();
		}

		// Update is called once per frame
		void Update () {
			transform.position += Time.deltaTime * speed * transform.forward;

		}
		void OnTriggerEnter(Collider col) {
			if(col.tag.Equals("Player")) {
				Destroy(this.gameObject);
				Debug.Log("enemy HIT player!");
				playerHealthbar.value -= 10;
			}
		}
	}
}
