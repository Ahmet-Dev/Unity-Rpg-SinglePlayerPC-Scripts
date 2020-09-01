using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour {
	
	public string enemyName;
	public int enemyID;
	
	[System.Serializable]
	public class Attribute
	{
		public int hp,mp,atk,def,spd,hit;
		public float criticalRate,atkSpd,atkRange,movespd;
	}
	
	public Attribute status;
	
	public int expGive;
	
}
