using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Quest_Data : MonoBehaviour {
	
	public enum QuestCondition {Hunting,Collect};
	
	[System.Serializable]
	public class QuestSetting
	{
		public string questName;
		public int questID;
		[Multiline]
		public string questDetail;
		public QuestCondition questCondition;
		public Vector2 idCondition;
		public Vector2 itemIDReward;
		public bool repeatQuest;
		
		[HideInInspector]
		public int killCount;
		
		[HideInInspector]
		public int questState = 0;
		
		[HideInInspector]
		public bool isStart;
	}
	
	public List<QuestSetting> questSetting = new List<QuestSetting>();
	
	
	//Editor Variable
	[HideInInspector]
	public int sizeQuest=0;
	[HideInInspector]
	public List<bool> showSizeQuest = new List<bool>();

	
	public void StartQuest(int id)
	{
		for(int i = 0;i<questSetting.Count;i++)
		{
			if(id == questSetting[i].questID)
			{
				questSetting[i].isStart = true;
				questSetting[i].questState = 1;
			}
		}
	}
	
	public void SaveQuest()
	{
		for(int i = 0;i<questSetting.Count;i++)
		{
			PlayerPrefs.SetInt("qKillCount"+i,questSetting[i].killCount);
			PlayerPrefs.SetInt("qQuestState"+i,questSetting[i].questState);
			PlayerPrefs.SetInt("qIsStart"+i,questSetting[i].isStart ? 1:0);
		}
	}
	
	public void LoadQuest()
	{
		for(int i = 0;i<questSetting.Count;i++)
		{
			questSetting[i].killCount = PlayerPrefs.GetInt("qKillCount"+i,questSetting[i].killCount);
			questSetting[i].questState = PlayerPrefs.GetInt("qQuestState"+i,questSetting[i].questState);
			questSetting[i].isStart = PlayerPrefs.GetInt("qIsStart"+i) == 1 ? true : false;
		}
		
	}
	
}
