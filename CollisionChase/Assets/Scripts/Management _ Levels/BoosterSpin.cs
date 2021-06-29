using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterSpin : MonoBehaviour {

	public float rotSpeed = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, transform.position.z * rotSpeed * Time.deltaTime);
	}
}
