using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Behavior.Health {
	public class VitalBar : MonoBehaviour {

		public int maxHP;
		public int currentHP;
		private Slider _display;

		public int CurrentHP {
			get { return currentHP; }
			set { currentHP = value; }
		}

		public int MaxHP {
			get { return maxHP; }
			set { maxHP = value; }
		}


		void Awake() {
			
			_display = GameObject.Find ("VitalBar").GetComponent<Slider> ();
		}


		// Use this for initialization
		void Start () {
		//	_isPlayerHealthBar = true;

			_display = gameObject.GetComponent<Slider>();
			_display.value = currentHP;
			_display.maxValue = maxHP;
			OnEnable ();
		}
		
		// Update is called once per frame
		void Update () {
		}

		public void OnEnable() {
				Messenger<int,int>.AddListener ("player health update", OnChangeHealth);
		}

		public void OnDisable() {
				Messenger<int, int>.RemoveListener ("player health update", OnChangeHealth);
		}

		public void OnChangeHealth(int curHealth, int maxHealth) {
			//Debug.Log ("We heard an event: curHealth = " + curHealth + " - maxHealth = " + maxHealth);

			_display.value = curHealth;
			_display.maxValue = maxHealth;
		}
			
	}
}