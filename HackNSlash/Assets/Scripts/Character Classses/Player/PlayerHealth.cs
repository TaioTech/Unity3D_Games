using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior.Health {
	public class PlayerHealth : MonoBehaviour {

		public int curHP;
		public int maxHP;

		public int CurHP {
			get { return curHP; }
			set { curHP = value; }
		}

		public int MaxHP {
			get { return maxHP; }
			set { maxHP = value; }
		}
			
	}
}