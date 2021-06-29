using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Behavior.AI {
	public class AIDirector : MonoBehaviour {

		public GameObject enemyPrefab;
		public Transform playerTarget;
		public Transform spawnParent;
		public List<Transform> spawnPositions = new List<Transform>();
		public List<EnemyAI> allEnemies = new List<EnemyAI>();



		bool alert;

		int spawnCycle;

		void Start()
		{
			if (spawnParent == null)
				return;
			Transform[] gos = spawnParent.GetComponentsInChildren<Transform>();
			spawnPositions.AddRange (gos);
			spawnPositions.Remove (spawnParent);
		}

		public void AlertEnemiesCloseBy(EnemyAI ai, Vector3 from)
		{
			
			for (int i = 0; i < allEnemies.Count; i++) {
				if (allEnemies[i] == ai)
						continue;

				if (allEnemies[i].aiState == EnemyAI.AIState.inView)
						continue;

				allEnemies [i].ChangeState (EnemyAI.AIState.inView);
				}
			if (alert)
				return;

			alert = true;

			SpawnAdditionalEnemies ();

		}

		void SpawnAdditionalEnemies () {

			if (enemyPrefab == null)
				return;

			if (playerTarget == null)
				return;

			if (spawnCycle > 5)
				return;
			
				
			StartCoroutine ("SpawnEnemiesInterval");
		}

		IEnumerator SpawnEnemiesInterval() {

			if (spawnCycle < 0) {
				yield return new WaitForSeconds (2);
				int ran = Random.Range (0, spawnPositions.Count);
				GameObject go = Instantiate (enemyPrefab, spawnPositions [ran].position, Quaternion.identity) as GameObject;

				EnemyAI ai = go.GetComponent<EnemyAI> ();
				ai.playerTarget = playerTarget;
				ai.ChangeState (EnemyAI.AIState.inView);
				spawnCycle++;
				SpawnAdditionalEnemies ();
			}


		}

		public static AIDirector singleton;
		void Awake() {
			singleton = this;
		}
	}
}
