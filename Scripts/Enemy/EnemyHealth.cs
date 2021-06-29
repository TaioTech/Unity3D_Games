using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior.Health;

namespace Behavior.AI {
	public class EnemyHealth : MonoBehaviour {

		public string nameType;
		public int currentHP;
		public int maxHP = 100;
		public PlayerCharacter pc;

		public Animator anim;

		public string NameType {
			get { return nameType; }
			set { nameType = value; }
		}

		public int CurrentHP {
			get { return currentHP; }
			set { currentHP = value; }
		}

		public int MaxHP {
			get { return maxHP; }
			set { maxHP = value; }
		}

		void Awake() {
			currentHP = maxHP;
			anim = GetComponent<Animator> ();
		}	

		public void OnCollisionEnter(Collision other) {
			
			if (other.collider.transform.tag == ("Arrow"))
				ApplyDamage (pc.ArrowDamage);
			
		}

		public void OnCollisionExit(Collision other) {
			if (other.collider.tag == ("Fist")) {
				ApplyDamage (pc.FistDamage);
				other.collider.enabled = false;
				StartCoroutine (RenableObject (other.collider));
			}
			if (other.collider.transform.tag == ("Sword")) {
				ApplyDamage (pc.SwordDamage);
				other.collider.enabled = false;
			}
				StartCoroutine (RenableObject (other.collider));
			if (other.collider.transform.tag == ("Katana")) {
				ApplyDamage (pc.KatanaDamage);
				other.collider.enabled = false;
				StartCoroutine (RenableObject (other.collider));
			}
		}

		public void ApplyDamage (float damageDealt)
		{
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isWalking", false);
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isIdle", false);
			anim.SetBool ("isDead", false);
			anim.SetBool ("isDamaged", true);
			currentHP -= (int) damageDealt;
		}

		IEnumerator RenableObject(Collider obj) {
			yield return new WaitForSeconds (.5f);
			obj.enabled = true;
		}
			
			
	}
}