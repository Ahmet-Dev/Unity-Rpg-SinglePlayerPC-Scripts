using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster_Data : MonoBehaviour {

	[System.Serializable]
	public class MonsterData
	{
		public GameObject prefabMonster;
		[HideInInspector]
		public EnemyStatus enemyStatus;
		[HideInInspector]
		public int enemyID;
		
	}
	
	public List<MonsterData> monsterList = new List<MonsterData>();
	
	void Awake()
	{
		//Get enemyID
		for(int i=0;i<monsterList.Count;i++)
		{
			monsterList[i].enemyStatus = monsterList[i].prefabMonster.GetComponent<EnemyStatus>();
			monsterList[i].enemyID = monsterList[i].enemyStatus.enemyID;
		}
	}
		
}
