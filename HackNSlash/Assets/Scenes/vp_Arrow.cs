using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class vp_Arrow : MonoBehaviour
{

	protected bool m_AlreadyHitSomething = false;
	public float Force = 100;
	public int Damage = 10;
	public Rigidbody rb;

	/// <summary>
	/// 
	/// </summary>
	void Awake()
	{

		rb.AddForce(transform.forward * Force);

	}


	/// <summary>
	/// 
	/// </summary>
	void OnTriggerEnter(Collider col)
	{

		if (m_AlreadyHitSomething)
			return;

		// we can only damage something upon first collision. this means that
		// as soon as the arrow bounces around against stone walls or whatnot,
		// it has already been disarmed
		m_AlreadyHitSomething = true;

		// see if arrow should stick & do damage
		// TIP: add more logic to this if-statement to identify possible target objects
		// (this is a little brute force but does the trick)
		if (col.GetComponent<Rigidbody>() != null ||
			col.tag == "Enemy" ||
			col.tag == "Terrain" ||
			col.tag == "Wooden" ||
			col.tag == "Pottery") // etc ...
		{
			// attach arrow to target object
			transform.parent = col.transform;

			// disable the arrow's own physics
			Destroy(GetComponent<Rigidbody>()); // unfortunately there is no good way of disabling physics without doing this :/

			// we may want to disable the collider too. if so, wait a sec so
			// all the collision logic has time to finish:
			//vp_Timer.In(1.0f, delegate()	{	if (collider != null)	collider.enabled = false;	});
			StartCoroutine("vp_Timer");
			if (GetComponent<Collider>() != null)
				GetComponent<Collider>().enabled = false;
			// do damage to the target
			// NOTE: your target object must have a vp_DamageHandler script ...
			// ... OR a script with a method called 'Damage' which takes a float argument
			col.GetComponent<Collider>().SendMessageUpwards("ApplyDamage", Damage, SendMessageOptions.DontRequireReceiver);

			// TIP: play a meaty impact sound here

			return;

		}

		// TIP: play a hard surface impact sound here

		// TIP: if we get here without doing damage, remove the arrow and replace it
		// with an arrow pickup of some kind!

	}

	IEnumerator vp_Timer() {
		yield return new WaitForSeconds (1f);
	}
}