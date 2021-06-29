using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Taiote {
	public class CameraTracker : MonoBehaviour {

		public GameObject player;
		public Camera mc;

		private Vector3 pos;
		public float distanceSetBack;
		public float distanceSetHeight;

		// Use this for initialization
		void Start () {
			mc = GetComponent<Camera> ();
			player = GameObject.FindGameObjectWithTag ("Player");

		}
		
		// Update is called once per frame
		void Update () {
			pos = new Vector3 (player.transform.position.x, player.transform.position.y + distanceSetHeight, player.transform.position.z - distanceSetBack);
			mc.transform.position = pos;
		}
	}
}
