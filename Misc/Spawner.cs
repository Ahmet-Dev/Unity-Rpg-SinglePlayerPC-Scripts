using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	public GameObject[] monsterList; 
	
	[HideInInspector]
	public Object[] spawnList;
	
	public float spawnTimer; 
	public int limitSpawn; 
	
	public float areaSpawn; 
	
	public bool ShowArea; 
	public Color areaColor; 

	private Vector3 randomSpawnVector;
	private float randomAngle;
	private int countSpawn;
	
	// Use this for initialization
	void Start () {
		spawnList = new Object[limitSpawn];
		InvokeRepeating("SpawnMonster",spawnTimer,spawnTimer);
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckSpawnLimit();
	}
	
	void CheckSpawnLimit()
	{
		if(countSpawn >= limitSpawn)
		{
			CancelInvoke("SpawnMonster");
			FindMissingList();
		}
	}
	
	void SpawnMonster()
	{
		Object monSpawn = Instantiate(monsterList[Random.Range(0,monsterList.Length)],RandomPostion(),Quaternion.identity);
		
		for(int i=0;i < spawnList.Length;i++)
		{
			if(spawnList[i] == null)
			{
				spawnList[i] = monSpawn;
				break;	
			}
		}
		countSpawn++;
	}

	void FindMissingList()
	{
		for(int i=0;i < spawnList.Length;i++)
		{
			if(spawnList[i] == null)
			{
				Invoke("SpawnMonster",spawnTimer);
				countSpawn--;
			}
		}
	}
	Vector3 RandomPostion()
	{		
		randomAngle = Random.Range(0f,91);
		randomSpawnVector.x = Mathf.Sin(randomAngle) * areaSpawn + transform.position.x;
		randomSpawnVector.z = Mathf.Cos(randomAngle) * areaSpawn + transform.position.z;
		randomSpawnVector.y = transform.position.y;	
		
		return randomSpawnVector;
	}

	void OnDrawGizmosSelected()
	{
		if(!ShowArea)
		{
			Gizmos.color = new Color(0.0f,0.5f,0.5f,0.3f);
			Gizmos.DrawSphere(transform.position,areaSpawn);
		}
	}
	
	void OnDrawGizmos()
	{
		if(ShowArea)
		{
			Gizmos.color = areaColor;
			Gizmos.DrawSphere(transform.position,areaSpawn);
		}
		
	}
	
}
