using UnityEngine;
using System.Collections;

public class CharacterData : MonoBehaviour {
	
	public static string characterName;
	public static string enableLoadData;
	public static PlayerStatus playerStatus;
	public static Quest_Data questData;
	
	void Start()
	{
		enableLoadData = PlayerPrefs.GetString("Enable Load");
		questData = GameObject.Find("QuestData").GetComponent<Quest_Data>();
		if(enableLoadData == "True" && LoadGameButton.loadData)
		{
			Invoke("LoadData",0.2f);
			enableLoadData = "False";
		}
	}
	
	public static void SaveData()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player");  
		playerStatus = go.GetComponent<PlayerStatus>();
		questData.SaveQuest();
		playerStatus.Save();
		GUI_Menu.instance.Save();
		PlayerPrefs.SetString("Enable Load","True");
		
	}
	public void LoadData()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player");  
		playerStatus = go.GetComponent<PlayerStatus>();
		questData.LoadQuest();
		playerStatus.Load();
		GUI_Menu.instance.Load();
		playerStatus.UpdateAttribute();
		
	}
	
	
}
