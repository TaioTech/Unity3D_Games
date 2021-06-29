using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior.Health;

namespace Behavior.AI {
	[System.Serializable]
	public class EnemyStats {


		public List<WaypointsBase> waypoints = new List<WaypointsBase> ();

		public WaypointsBase curWaypoint;

		public List<CoverpointsBase> coverPositions = new List<CoverpointsBase> ();

		public CoverpointsBase curCoverposition;

		//[HideInInspector]
		public int waypointIndex;
		[HideInInspector]
		public float waypointDistance;
		[HideInInspector]
		public float waitTimeWP;
		[HideInInspector]
		public float maxWaitTime;

//		public int coverPositionIndex;
//		[HideInInspector]
//		public float coverPositionDistance;
//		[HideInInspector]
//		public float waitTimeCP;
//		[HideInInspector]
//		public float maxCoverWaitTime;

		//[HideInInspector]
		public float behaviorLife;
		//[HideInInspector]
		public float maxBehaviorLife;
		[HideInInspector]
		public float distanceFromTarget;
		[HideInInspector]
		public Vector3 lastKnownPosition;
		[HideInInspector]
		public Vector3 canidatePosition;

		public Transform targetCover;

		public float fireTimer = 1.4f;

		public float fireCounter = 0;

		public bool canFire = false;

		public static EnemyStats singleton;
		void Awake()
		{
			singleton = this;
		}
	}
}