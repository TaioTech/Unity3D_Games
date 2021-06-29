using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior.AI {

	public static class CommonWizardBehavior 
	{
		public static float searchLife = 20;

		public static float attackLife = 30;

		public static void PatrolBehavior(WizardAI ai) 
		{
			EnemyStats e = ai.enStats;

			if (e.waypoints.Count > 0) 
			{
				if (e.curWaypoint.targetDestination == null) {
					e.curWaypoint = e.waypoints [e.waypointIndex];
					ai.MoveToPosition (e.curWaypoint.targetDestination.position);
				}

				e.curWaypoint = e.waypoints [e.waypointIndex];

				e.waypointDistance = Vector3.Distance (ai.transform.position, e.curWaypoint.targetDestination.position);

				if (e.waypointDistance < 2) 
				{

					e.waitTimeWP += Time.deltaTime * 15;

					if (e.waitTimeWP > e.maxWaitTime) 
					{
						e.waitTimeWP = 0;


						if (e.waypointIndex < e.waypoints.Count - 1) 
						{
							e.waypointIndex++;

						} else {

							e.waypointIndex = 0;

							e.waypoints.Reverse ();

						}
						e.curWaypoint = e.waypoints [e.waypointIndex];
						e.maxWaitTime = e.curWaypoint.waitTime;
						ai.MoveToPosition (e.curWaypoint.targetDestination.position);
					}
				} 
			}
		}

		public static void SearchBehavior(WizardAI ai)
		{
			EnemyStats e = ai.enStats;
			float distanceCheck = Vector3.Distance (ai.transform.position, e.canidatePosition);

			if (distanceCheck < ai.radius) 
			{
				e.waitTimeWP += Time.deltaTime * 15;
				if (e.waitTimeWP > e.maxWaitTime) 
				{
					e.waitTimeWP = 0;
					e.maxWaitTime = Random.Range (1, 3);
					e.canidatePosition = ai.RandomVector3AroundPosition (e.lastKnownPosition);
					ai.MoveToPosition (e.canidatePosition);
				}
			}
		}

		public static void ChaseBehavior(WizardAI ai) {
			EnemyStats e = ai.enStats;

			float distanceCheck = Vector3.Distance (ai.transform.position, e.lastKnownPosition);
			if (distanceCheck < ai.attackRange) {
				ai.StopMoving ();
			} else {
				ai.MoveToPosition(e.lastKnownPosition);
			}
		}

		public static void GetToCoverBehavior(WizardAI ai) {
			EnemyStats e = ai.enStats;
			if (e.targetCover == null) {
				e.targetCover = ai.GetCover (ai.transform.position);

				if (e.targetCover == null) {
					int index = Random.Range (0, 5);
					e.targetCover.position = ai.enStats.coverPositions [index].targetDestination.position;
				}
			}
//			e.coverPositionDistance = Vector3.Distance (ai.transform.position, e.targetCover.position);
//
//			if (e.coverPositionDistance < 1) {
//				ai.RotateTowardsTarget ();
//			} else {
//				ai.MoveToPosition (e.targetCover.position);
//			}
		} 

		public static void InCoverBehavior(WizardAI ai) {
			EnemyStats e = ai.enStats;
			float distanceCheck = Vector3.Distance (ai.transform.position, e.lastKnownPosition);
		}

		public static void InAttackBehavior(WizardAI ai) {

			EnemyStats e = ai.enStats;

			if (e.fireCounter >= e.fireTimer) {
				e.fireCounter = 0;
				ai.AttackTarget ();
			} else {
				e.fireCounter += Time.deltaTime;

			}


		}
	}
		
}