using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Behavior.Health;

namespace Behavior.AI {
	public class WizardAI : MonoBehaviour {
		//physicals on body
		public Transform playerTarget;
		public PlayerCharacter pc;
		private PlayerHealth eph;
		public GameManager gm;
		public Animator anim;
		public EnemyStats enStats = new EnemyStats();
		public string AIType;
		public AIStats aiStats;
		[HideInInspector]
		public NavMeshAgent agent;
		//self-created objects
		public AIState aiState;
		public AIState targetState;
		//keeping track of directions
		Vector3 direction;
		Vector3 rotDirection;

		public GameObject laserBeam;
		//public Transform firePoint;
		//Stats
		public float rotSpeed = 5;
		public float lineOfSightLength = 15;
		public float radius = 30;
		public float maxDistance = 40;
		public float attackRange = 8.5f;
		public float meleeRange = 2.5f;
		public int baseDamage = 15;
		public int expDropped;
		public float hitPercentage = 75f;

		//conditionals
		bool isInAngle = false;
		bool isClear = false;
		public bool goToCover = false;
		public bool inCover = false;
		public bool overrideCover = false;

		public EnemyHealth enemyHealth;

		delegate void EveryFrame();
		EveryFrame everyFrame;
		delegate void LateFrame();
		LateFrame lateFrame;
		delegate void LateLateFrame();
		LateLateFrame llateFrame;	


		int lFrame = 4;
		int lFrame_counter = 0;

		int llFrame = 19;
		int llf_counter = 0;

		// Use this for initialization
		void Start () {
			anim = GetComponent<Animator> ();
			pc = playerTarget.GetComponent<PlayerCharacter> ();
			gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
			eph = playerTarget.GetComponent<PlayerCharacter> ().ph;
			agent = GetComponent<NavMeshAgent> ();
			enemyHealth = GetComponent<EnemyHealth> ();
			aiState = AIState.inPatrol;
			ChangeState (AIState.inPatrol);
		}


		// Update is called once per frame
		void Update () {

			if (enemyHealth.CurrentHP <= 0) {
				enemyHealth.CurrentHP = 0;
				pc.CurExp += expDropped;
				StartCoroutine(Dead());

			}

			MonitorStates ();
			if (everyFrame != null) {
				everyFrame ();
			}
			lFrame_counter++;
			if (lFrame_counter > lFrame) {
				if (lateFrame != null)
					lateFrame ();
				lFrame_counter = 0;
			}

			llf_counter++;
			if (llf_counter > llFrame) {
				if (llateFrame != null)
					llateFrame ();

				llf_counter = 0;
			}
		}

		void MonitorStates() {		//runs on every frame

			switch (aiState) {
			case AIState.inPatrol:
				if (enStats.distanceFromTarget < lineOfSightLength && isClear && isInAngle) {

					ChangeState (AIState.inView);
				}
				if (enStats.distanceFromTarget < radius) {

					ChangeState (AIState.inPatrolRadius);
				}

				break;
			case AIState.inPatrolRadius:
				if (maxDistance < enStats.distanceFromTarget) {

					ChangeState (AIState.inPatrol);
				}
				if ((enStats.distanceFromTarget < lineOfSightLength && enStats.distanceFromTarget > attackRange) && isClear && isInAngle) {

					ChangeState (AIState.inView);
				}
				else if (enStats.distanceFromTarget < attackRange && isClear && isInAngle) {

					ChangeState (AIState.inAttack);
				}
				//				if (goToCover) {
				//					
				//					ChangeState (AIState.getToCover);
				//				}
				break;
			case AIState.inView:
				if (enStats.distanceFromTarget > radius) {

					ChangeState (AIState.inPatrol);
				}
				if (!isInAngle || !isClear) {
					ChangeState (AIState.inSearch);
				}
				if (enStats.distanceFromTarget < attackRange) {
					ChangeState (AIState.inAttack);
				}
				//				if (chaseMode) {
				//					
				//					ChangeState (AIState.inChase);
				//				}
				//				if (goToCover) {
				//
				//					ChangeState (AIState.getToCover);
				//				}
				break;
			case AIState.inChase:
				if (!isInAngle || !isClear) {

					ChangeState (AIState.inSearch);
				}
				if (enStats.distanceFromTarget > radius) {

					ChangeState (AIState.inPatrol);
				}
				if (enStats.distanceFromTarget < attackRange && isInAngle && isClear) {
					ChangeState (AIState.inAttack);
				}
				break;
			case AIState.inSearch:
				if (enStats.distanceFromTarget < attackRange && isInAngle && isClear) {
					ChangeState (AIState.inAttack);
				}
				else if (isInAngle && isClear) {
					ChangeState (AIState.inChase);
				}

				if (enStats.distanceFromTarget > radius) {

					ChangeState (AIState.inPatrol);
				}

				break;
				//			case AIState.getToCover:
				//				if (canSee && overrideCover) {
				//					
				//					ChangeState (AIState.inView);
				//				}
				//				if (inCover) {
				//					
				//					ChangeState (AIState.inCover);
				//				}
				//				break;
				//			case AIState.inCover:
				//				if (enStats.distanceFromTarget > lineOfSightLength) {
				//
				//					ChangeState (AIState.inSearch);
				//				}
				//				if (enStats.distanceFromTarget < lineOfSightLength) {
				//					
				//					ChangeState (AIState.inAttack);
				//				}
				//				break;
			case AIState.inAttack:
				if (enStats.distanceFromTarget > attackRange) {
					ChangeState (AIState.inChase);
				}
				if (!isInAngle && !isClear) {
					ChangeState (AIState.inSearch);
				}
				if (enStats.distanceFromTarget > maxDistance) {
					ChangeState (AIState.inPatrol);
				}
				//				if (goToCover) {
				//					ChangeState (AIState.getToCover);
				//				}
				break;
			default:
				break;
			}
		}


		public void ChangeState(AIState targetState) {
			//changeSt = true;
			aiState = targetState;
			everyFrame = null;
			lateFrame = null;
			llateFrame = null;

			switch (targetState) {
			case AIState.inPatrol:
				lateFrame = DefaultBehaviors;

				break;
			case AIState.inPatrolRadius:
				lateFrame = InRadiusBehaviors;

				//everyFrame = InRadiusBehaviors;
				break;
			case AIState.inView:
				lateFrame = InViewBehaviorsSecondary;
				everyFrame = InViewBehaviors;
				enStats.lastKnownPosition = playerTarget.position;
				StopMoving ();

				//AIDirector.singleton.AlertEnemiesCloseBy (this, transform.position);
				break;
			case AIState.inSearch:

				lateFrame = InSearchBehaviors;
				enStats.lastKnownPosition = playerTarget.position;
				enStats.canidatePosition = enStats.lastKnownPosition;
				float bhlifeOffset = Random.Range (-4, 4);
				enStats.maxBehaviorLife = CommonBehavior.searchLife + bhlifeOffset;
				WaypointsBase wp = new WaypointsBase ();
				enStats.curWaypoint = wp;
				break;
			case AIState.inChase:

				lateFrame = MonitorTargetPosition;
				everyFrame = InChaseBehaviors;
				enStats.lastKnownPosition = playerTarget.position;
				enStats.canidatePosition = enStats.lastKnownPosition;
				break;
			case AIState.getToCover:
				everyFrame = GetToCoverBehaviors;
				break;
			case AIState.inCover:
				enStats.lastKnownPosition = playerTarget.position;
				enStats.canidatePosition = enStats.lastKnownPosition;
				everyFrame = InCoverBehaviors;
				break;
			case AIState.inAttack:

				everyFrame = InAttackBehaviors;
				llateFrame = InAttackBehaviorsSecondary;
				//lateFrame = MonitorTargetPosition;
				break;
			default:
				break;

			}
		}


		public void AttackTarget() {
			Vector3 origin = transform.position;
			Debug.DrawRay (origin, rotDirection * meleeRange);
			RaycastHit hit;
			if (Physics.Raycast (origin, rotDirection, out hit, meleeRange) && hit.collider.transform.tag == "Player") {
				anim.SetBool ("isAttacking", true);
				anim.SetBool ("isWalking", false);
				anim.SetBool ("isRunning", false);
				anim.SetBool ("isIdleNormal", false);
				anim.SetBool ("isIdleCombat", false);
				anim.SetBool ("isDead", false);
				anim.SetBool ("isDamaged", false);


					
				Debug.Log ("hit Player");
				int range = Random.Range (0, 101);
				if (range <= hitPercentage) {
					Debug.Log ("Wizard struck!");
					eph.CurHP -= baseDamage;
					Messenger<int, int>.Broadcast ("player health update", playerTarget.GetComponent<PlayerCharacter> ().ph.CurHP,
						playerTarget.GetComponent<PlayerCharacter> ().ph.MaxHP);
				} else {
					Debug.Log ("Wizard missed!");
				}




				//ShootLaser ();
			}
		}

		public void MonitorBehaviorLife() {
			enStats.behaviorLife += Time.deltaTime;
			if (enStats.behaviorLife > enStats.maxBehaviorLife) {
				enStats.behaviorLife = 0;
				ChangeState (AIState.inPatrol);
			}

		}

		public void MoveToPosition(Vector3 targetPosition) {

			agent.isStopped = false;
			agent.SetDestination (targetPosition);
		}

		public Vector3 RandomVector3AroundPosition(Vector3 targetPosition)
		{
			float offsetX = Random.Range (-3, 3);
			float offsetZ = Random.Range (-3, 3);

			Vector3 originPos = targetPosition;
			originPos.x += offsetX;
			originPos.z += offsetZ;


			NavMeshHit hit;

			if (NavMesh.SamplePosition (originPos, out hit, 5, NavMesh.AllAreas)) {
				return hit.position;
			}

			return targetPosition;
		}

		public void StopMoving() {

			agent.isStopped = true;
		}

		private void DefaultBehaviors()
		{
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isWalking", false);
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isIdleNormal", true);
			anim.SetBool ("isIdleCombat", false);
			anim.SetBool ("isDead", false);
			anim.SetBool ("isDamaged", false);
			DistanceCheck (playerTarget);
			FindDirection (playerTarget);
			AngleCheck ();
			IsClearView (playerTarget);
			CommonWizardBehavior.PatrolBehavior (this);
		}

		private void InRadiusBehaviors() 
		{
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isWalking", false);
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isIdleNormal", false);
			anim.SetBool ("isIdleCombat", true);
			anim.SetBool ("isDead", false);
			anim.SetBool ("isDamaged", false);
			DistanceCheck (playerTarget);
			FindDirection (playerTarget);
			AngleCheck ();
			IsClearView (playerTarget);
			CommonWizardBehavior.PatrolBehavior (this);
		}

		private void InViewBehaviors() 
		{
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isWalking", true);
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isIdleNormal", false);
			anim.SetBool ("isIdleCombat", false);
			anim.SetBool ("isDead", false);
			anim.SetBool ("isDamaged", false);
			DistanceCheck (playerTarget);
			FindDirection (playerTarget);
			AngleCheck ();
			IsClearView (playerTarget);
			RotateTowardsTarget ();
			//getting attacked soon to come
			//			if ( )
			//				CommonBehavior.GetToCoverBehavior (this);
			//			else if (!canSee)
			//				CommonBehavior.SearchBehavior (this);
		}

		private void InViewBehaviorsSecondary() {
			MonitorTargetPosition ();
		}

		private void InSearchBehaviors() {
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isWalking", true);
			anim.SetBool ("isRunning", false);	
			anim.SetBool ("isIdleNormal", false);
			anim.SetBool ("isIdleCombat", false);
			anim.SetBool ("isDead", false);
			anim.SetBool ("isDamaged", false);
			DistanceCheck (playerTarget);
			FindDirection (playerTarget);
			AngleCheck ();
			IsClearView (playerTarget);
			CommonWizardBehavior.SearchBehavior (this);
		}

		private void InChaseBehaviors() {
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isWalking", false);
			anim.SetBool ("isRunning", true);	
			anim.SetBool ("isIdleNormal", false);
			anim.SetBool ("isIdleCombat", false);
			anim.SetBool ("isDead", false);
			anim.SetBool ("isDamaged", false);
			DistanceCheck (playerTarget);
			AngleCheck ();
			IsClearView (playerTarget);
			FindDirection (playerTarget);
			RotateTowardsTarget ();
			CommonWizardBehavior.ChaseBehavior (this);
		}

		private void GetToCoverBehaviors() {
			CommonWizardBehavior.GetToCoverBehavior (this);
			RotateTowardsTarget ();
		}

		private void InCoverBehaviors() {
			FindDirection (playerTarget);
			RotateTowardsTarget ();

			CommonWizardBehavior.InCoverBehavior (this);
		}
			
		private void InAttackBehaviors() {
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isWalking", false);
			anim.SetBool ("isRunning", true);	
			anim.SetBool ("isIdleNormal", false);
			anim.SetBool ("isIdleCombat", false);
			anim.SetBool ("isDead", false);
			anim.SetBool ("isDamaged", false);
			DistanceCheck (playerTarget);
			FindDirection (playerTarget);
			AngleCheck ();
			IsClearView (playerTarget);
			RotateTowardsTarget ();
			CommonWizardBehavior.InAttackBehavior (this);
		}

		private void InAttackBehaviorsSecondary() {
			MoveToPosition (playerTarget.position);
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isWalking", false);
			anim.SetBool ("isRunning", false);	
			anim.SetBool ("isIdleNormal", false);
			anim.SetBool ("isIdleCombat", true);
			anim.SetBool ("isDead", false);
			anim.SetBool ("isDamaged", false);
		}

		private void IsClearView(Transform target) {
			RaycastHit hit;
			Vector3 origin = transform.position;
			Debug.DrawRay (origin, rotDirection * lineOfSightLength);
			if (Physics.Raycast (origin, rotDirection, out hit, lineOfSightLength)) {
				//Debug.Log (hit.transform.tag);
				if (hit.transform.tag == "Player") {
					isClear = true;
				} else
					isClear = false;
			}
		}

		private void AngleCheck() {
			if (rotDirection == Vector3.zero)
				rotDirection = transform.forward;

			float angle = Vector3.Angle (transform.forward, rotDirection);
			if (angle < 70)
				isInAngle = true;
			else
				isInAngle = false;
		}

		public void FindDirection (Transform target) {
			direction = target.position - transform.position;
			rotDirection = direction;
			rotDirection.y = 0;
		}

		public void DistanceCheck(Transform target) {
			enStats.distanceFromTarget = Vector3.Distance (transform.position, target.position);
		}

		public void RotateTowardsTarget() {
			agent.isStopped = false;
			if (rotDirection == Vector3.zero) {
				rotDirection = transform.forward;
			}
			Quaternion targetRotation = Quaternion.LookRotation (rotDirection);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
		}

		public void MonitorTargetPosition() {
			enStats.lastKnownPosition = playerTarget.position;
			MoveToPosition (enStats.lastKnownPosition);
		}

		public IEnumerator Dead() {
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isWalking", false);
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isIdleNormal", false);
			anim.SetBool ("isIdleCombat", false);
			anim.SetBool ("isDead", true);
			anim.SetBool ("isDamaged", false);
			yield return new WaitForSeconds (4f);
			Destroy (this.gameObject);

		}

		public Transform GetCover(Vector3 fromMyPos)
		{

			Transform r = null;
			float minDist = Mathf.Infinity;

			for (int i = 0; i < enStats.coverPositions.Count; i++) {
				float distanceFrom = Vector3.Distance (fromMyPos, enStats.coverPositions [i].targetDestination.position);

				Vector3 direction = fromMyPos - enStats.coverPositions [i].targetDestination.position;
				direction.y = 0;

				if (distanceFrom < minDist) {
					minDist = distanceFrom;
					enStats.curCoverposition = enStats.coverPositions [i];
					r = enStats.coverPositions[i].targetDestination;
				}

			}
			return r;
		}

		public void SetStats () {
			if (AIType == "wizard") {
				aiStats = AIStats.wizard;
				baseDamage = 15;
				expDropped = 50;
			} 
		}

		//		public void ShootLaser() {
		//			
		//			for (int i = 0; i < 1; i++) {
		//				GameObject laser = Instantiate (laserBeam, firePoint.position + firePoint.forward, transform.rotation);
		//				Debug.Log ("Shot laser at target. ShootLaser is called.");
		//				Destroy (laser, 3f);
		//			}
		//		}


		public enum AIState {
			inPatrol,
			inPatrolRadius,
			inView,
			inSearch,
			inChase,
			getToCover,
			inCover,
			inAttack
		}

		public enum AIStats {
			wizard
		}
	}
}