using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Behavior.AI;

namespace Behavior.Health {
	public class PlayerCharacter : BaseCharacter {

		//User Stats
		private int level = 1;
		private float expToLevel;
		private float curExp;
		private const float EXP_MULTIPLIER = 1.15f;
		public int arrowDamage = 25;
		public int swordDamage = 15;
		public int katanaDamage = 25;
		public int fistDamage = 3;

		public PlayerHealth ph;
		public GameManager gm;
		public VitalBar vitals;
		public Animator anim;
		public bool playerDeath = false;

		public int Level {
			get { return level; }
			set { level = value; }
		}

		public float CurExp {
			get { return curExp; }
			set { curExp = value; }
		}

		public float ExpToLevel {
			get { return expToLevel; }
			set { expToLevel = value; }
		}

		public int SwordDamage {
			get { return swordDamage; }
			set { swordDamage = value; }
		}

		public int ArrowDamage {
			get { return arrowDamage; }
			set { arrowDamage = value; }
		}

		public int KatanaDamage {
			get { return katanaDamage; }
			set { katanaDamage = value; }
		}

		public int FistDamage {
			get { return fistDamage; }
			set { fistDamage = value; }
		}
	
		private EnemyHealth enemyReference;
	

		//tool-tip requirements
		public bool hoverOverActive;
		private string hoverName;
		private string hoverHealth;
		private string hoverMaxHealth;
		public GUIStyle hoverStyles = new GUIStyle();


		private float checkCursor = 0;
		private float cursorFrames = 25;

		void Awake() {
			anim = GetComponent<Animator> ();
			ph = GetComponent<PlayerHealth> ();
			expToLevel = 50;
		}

		void Update() {
			
			GetEnemyStatsForGUI ();

			CheckForLevelUp ();

			if (ph.CurHP <= 0) {
				ph.CurHP = 0;
				gm.Invoke ("Restart", 3f);
				anim.SetBool ("isDead", true);
				playerDeath = true;
			}

			if (checkCursor > cursorFrames) {
				checkCursor = 0;
				if (Cursor.lockState != CursorLockMode.Locked) {
					Cursor.lockState = CursorLockMode.Locked;
				}
			} else {
				checkCursor++;
			}


		}

		void OnGUI () {
			hoverStyles.normal.textColor = Color.red;
			hoverStyles.alignment = TextAnchor.UpperCenter;
			hoverStyles.fontSize = 22;
			if (hoverOverActive) {
				GUI.Label (new Rect (Input.mousePosition.x - 25, Screen.height - Input.mousePosition.y - 75, 100, 50), "" + hoverName + 
					System.Environment.NewLine + "Health: " + hoverHealth + " / " + hoverMaxHealth, hoverStyles);
			}
		}

		private void GetEnemyStatsForGUI()
		{
			//TOOL TIP POPUP DISPLAY

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 10000)) {
				if (hit.transform.tag == "Enemy") {
					hoverOverActive = true;
					hoverName = hit.transform.GetComponent<EnemyHealth> ().nameType;
					hoverHealth = hit.transform.GetComponent<EnemyHealth> ().currentHP.ToString();
					hoverMaxHealth = hit.transform.GetComponent<EnemyHealth> ().maxHP.ToString ();
				} else {
					hoverOverActive = false;
				}
			}
		}
			
		private void CheckForLevelUp() {
			if (curExp >= expToLevel) {
				LevelUp ();		
			}
		}

		private void LevelUp() {
			Debug.Log ("LEVEL UP!");
			level++;
			Debug.Log ("Current level is now: " + level);
			expToLevel *= EXP_MULTIPLIER;
			Debug.Log ("Current experience is at: " + curExp);
			Debug.Log ("Experience to next level is: " + expToLevel);
		}


	}
}
