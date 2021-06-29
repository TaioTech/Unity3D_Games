using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior.Health {
	public class GameMaster : MonoBehaviour {

		public GameObject playerCharacter;
		public GameObject gameSettings;

		private GameObject _pc;
		private PlayerCharacter _pcScript;
		public GameObject _playerSpawnPointPos;

		void Start () {
			

			GameObject go = GameObject.Find (GameSettings.PLAYER_SPAWN_POINT);
			if (go == null) {
				go = new GameObject (GameSettings.PLAYER_SPAWN_POINT);

				go.transform.position = _playerSpawnPointPos.transform.position;
			}

			_pc = Instantiate (playerCharacter, go.transform.position, Quaternion.identity) as GameObject;
			_pc.name = "pc";
			_pcScript = _pc.GetComponent<PlayerCharacter> ();


			//mainCamera.transform.position = new Vector3 (_pc.transform.position.x, _pc.transform.position.y + yOffset, _pc.transform.position.z + zOffset);
			//mainCamera.transform.Rotate (xRotOffset, 0, 0);

			LoadCharacter ();
		}
		
		public void LoadCharacter(){
			GameObject gs = GameObject.Find("__GameSettings");
			if (gs == null) {
				GameObject gs1 = Instantiate (gameSettings, _playerSpawnPointPos.transform.position, Quaternion.identity);
				gs1.name = "__GameSettings";
			}

			GameSettings gsScript = GameObject.Find("__GameSettings").GetComponent<GameSettings> ();

			//loading the character data
			gsScript.LoadCharacterData ();

		}



	}

}