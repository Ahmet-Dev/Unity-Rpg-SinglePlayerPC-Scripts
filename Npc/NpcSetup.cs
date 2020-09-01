using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcSetup : MonoBehaviour {
	
	public enum NameTagSetup {NpcName,PlayerName,Other};
	public enum NpcType {RegularNpc,ShopKeeper,QuestNpc,SaveNpc};
	public enum FaceSetup {NpcFace,PlayerFace,Other};
	
	public enum QuestNpcState{StartQuest,InProgress,Complete,AfterComplete};
	
	public NpcType npcType;
	
	public string npcName;
	public Texture2D npcFace;
	
	private bool startCountDialogBox;
	
	[System.Serializable]
	public class MessageBoxSetting
	{
		public bool enableNameTag = true;
		public NameTagSetup nameTagSetup;
		public string otherNameTag;
		public bool enableFace = true;
		public FaceSetup faceSetup;
		public Texture2D otherFace;
		[Multiline]
		public string message;
		
	}
	
	
	public List<MessageBoxSetting> dialogSetting = new List<MessageBoxSetting>();
	
	public int questID;
	public QuestNpcState questNpcState;
	
	public List<MessageBoxSetting> dialogQuest = new List<MessageBoxSetting>();
	public List<MessageBoxSetting> dialogQuestInProgress = new List<MessageBoxSetting>();
	public List<MessageBoxSetting> dialogQuestComplete = new List<MessageBoxSetting>();
	public List<MessageBoxSetting> dialogQuestAfterComplete = new List<MessageBoxSetting>();
	
	private HeroController playerControl;
	private int indexDialog;
	private int indexCurrentQuest;
	private bool disableMovePlayer;
	private Quest_Data questData;
	private QuestWindow questWindow;
	public GUI_Menu inventory;
	private GameObject player;
	
	
	public static bool resetMessageBox;
	
	public static bool disableNext;

	[HideInInspector]
	public int sizeDialog=0;
	[HideInInspector]
	public int sizeDialogQuest=0;
	[HideInInspector]
	public int sizeDialogQuestInProgress=0;
	[HideInInspector]
	public int sizeDialogQuestComplete=0;
	[HideInInspector]
	public int sizeDialogQuestAfterComplete=0;
	[HideInInspector]
	public List<bool> showSizeDialog = new List<bool>();
	[HideInInspector]
	public List<bool> showSizeDialogQuest = new List<bool>();
	[HideInInspector]
	public List<bool> showSizeDialogQuestInProgress = new List<bool>();
	[HideInInspector]
	public List<bool> showSizeDialogQuestComplete = new List<bool>();
	[HideInInspector]
	public List<bool> showSizeDialogQuestAfterComplete = new List<bool>();
	

	// Use this for initialization
	void Start () {

		if(this.tag != "Npc")
			this.tag = "Npc";
		
		if(this.gameObject.layer != 13)
			this.gameObject.layer = 13;
		
		indexDialog = 0;
		
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>();
		
		if(npcType == NpcType.QuestNpc)
		{
			questData = GameObject.Find("QuestData").GetComponent<Quest_Data>();
			questWindow = GameObject.Find("GUI Manager/QuestWindow").GetComponent<QuestWindow>();
			player = GameObject.FindGameObjectWithTag("Player");
			inventory = player.transform.Find("Inventory").GetComponent<GUI_Menu>();
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(npcType == NpcType.RegularNpc || npcType == NpcType.ShopKeeper || npcType == NpcType.SaveNpc)
			NpcDetect();
		else if(npcType == NpcType.QuestNpc)
			QuestNpcDetect();
	}
	
	void NpcDetect()
	{
		if(disableMovePlayer)
			DisableMove();
		
		if(startCountDialogBox)
		{
			SetupDialogBox(indexDialog);
			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				
				SoundManager.instance.PlayingSound("Next_Dialog");
				
				if(indexDialog < dialogSetting.Count-1)
				{
					indexDialog++;
				}else
				{
					if(npcType == NpcType.ShopKeeper || npcType == NpcType.SaveNpc)
						CallNextCommand();
					
					indexDialog = 0;
					startCountDialogBox = false;
					MessageBox.showMessageBox = false;
					MessageBox.showNameTag = false;
					MessageBox.showFace = false;
					Invoke("EnableMovePlayer",0.3f);
				}
				
				
			}
			
		}	
	}
	
	void QuestNpcDetect()
	{
		if(disableMovePlayer)
			DisableMove();
		
		if(startCountDialogBox)
		{
			if(questNpcState == QuestNpcState.StartQuest)
				SetupDialogBoxQuest(indexDialog,dialogQuest);
			else if(questNpcState == QuestNpcState.InProgress)
				SetupDialogBoxQuest(indexDialog,dialogQuestInProgress);
			else if(questNpcState == QuestNpcState.Complete)
				SetupDialogBoxQuest(indexDialog,dialogQuestComplete);
			else if(questNpcState == QuestNpcState.AfterComplete)
				SetupDialogBoxQuest(indexDialog,dialogQuestAfterComplete);
			 
			
			if(questNpcState == QuestNpcState.StartQuest)
				{
					if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || resetMessageBox)
					{
						if(!QuestWindow.enableWindow)
						SoundManager.instance.PlayingSound("Next_Dialog");
					
						if(indexDialog < dialogQuest.Count-1)
						{
							indexDialog++;
							
							if(indexDialog == dialogQuest.Count-1)
							{
								QuestWindow.enableWindow = true;
								QuestWindow.enableButtonAccept = true;
								disableNext = true;
								questWindow.SetupQuestWindow(questID);
							}
								
						
						}else if(!disableNext)
						{	
							disableNext = false;
							indexDialog = 0;
							startCountDialogBox = false;
							MessageBox.showMessageBox = false;
							MessageBox.showNameTag = false;
							MessageBox.showFace = false;
							resetMessageBox = false;
							Invoke("EnableMovePlayer",0.3f);
						}
					}
				}else
			
			
			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				
				SoundManager.instance.PlayingSound("Next_Dialog");
				
				if(questNpcState == QuestNpcState.InProgress)
				{
					if(indexDialog < dialogQuestInProgress.Count-1)
					{
						indexDialog++;
					}else
					{				
						indexDialog = 0;
						startCountDialogBox = false;
						MessageBox.showMessageBox = false;
						MessageBox.showNameTag = false;
						MessageBox.showFace = false;
						QuestWindow.enableWindow = false;
						Invoke("EnableMovePlayer",0.3f);
					}
				}
					
					
				else if(questNpcState == QuestNpcState.Complete)
				{
					if(indexDialog < dialogQuestComplete.Count-1)
					{
						indexDialog++;
					}else
					{
						if(!questData.questSetting[indexCurrentQuest].repeatQuest)
						{
							questData.questSetting[indexCurrentQuest].questState = 3;
						}else
						{
							questData.questSetting[indexCurrentQuest].questState = 0;
						}
						
						GiveReward(questID);
						indexDialog = 0;
						startCountDialogBox = false;
						MessageBox.showMessageBox = false;
						MessageBox.showNameTag = false;
						MessageBox.showFace = false;
						Invoke("EnableMovePlayer",0.3f);
					}
				}
					
					
				else if(questNpcState == QuestNpcState.AfterComplete)
				{
					if(indexDialog < dialogQuestAfterComplete.Count-1)
					{
						indexDialog++;
					}else
					{				
						indexDialog = 0;
						startCountDialogBox = false;
						MessageBox.showMessageBox = false;
						MessageBox.showNameTag = false;
						MessageBox.showFace = false;
						Invoke("EnableMovePlayer",0.3f);
					}
				}
									
			}
			
		}	
	}
	
	public void DisableMove()
	{
		playerControl.dontMove = true;
		playerControl.dontClick = true;
	}
	
	
	public void CallDialogBox()
	{
		startCountDialogBox = true;
		MessageBox.showMessageBox = true;
		disableMovePlayer = true;
		
	}
	
	void EnableMovePlayer()
	{
		disableMovePlayer = false;
		playerControl.ResetState();
	}
	
	void SetupDialogBox(int i)
	{
		if(dialogSetting[i].enableNameTag)
		{
			if(dialogSetting[i].nameTagSetup == NameTagSetup.PlayerName)
				MessageBox.nameTagStatic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().playerName;
			else if(dialogSetting[i].nameTagSetup == NameTagSetup.NpcName)
				MessageBox.nameTagStatic = npcName;
			else if(dialogSetting[i].nameTagSetup == NameTagSetup.Other)
				MessageBox.nameTagStatic = dialogSetting[i].otherNameTag;
			
			MessageBox.showNameTag = true;
		}else
		{
			MessageBox.showNameTag = false;
		}
		
		if(dialogSetting[i].enableFace)
		{
			if(dialogSetting[i].faceSetup == FaceSetup.PlayerFace)
				MessageBox.faceStatic = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>().heroImage;
			else if(dialogSetting[i].faceSetup == FaceSetup.NpcFace)
				MessageBox.faceStatic = npcFace;
			else if(dialogSetting[i].faceSetup == FaceSetup.Other)
				MessageBox.faceStatic = dialogSetting[i].otherFace;
			
			MessageBox.showFace = true;
		}else
		{
			MessageBox.showFace = false;
		}
		
		
		MessageBox.messageStatic = dialogSetting[i].message;
		
	}
	
	void SetupDialogBoxQuest(int i,List<MessageBoxSetting> mBoxQuest)
	{
		if(mBoxQuest[i].enableNameTag)
		{
			if(mBoxQuest[i].nameTagSetup == NameTagSetup.PlayerName)
				MessageBox.nameTagStatic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().playerName;
			else if(mBoxQuest[i].nameTagSetup == NameTagSetup.NpcName)
				MessageBox.nameTagStatic = npcName;
			else if(mBoxQuest[i].nameTagSetup == NameTagSetup.Other)
				MessageBox.nameTagStatic = mBoxQuest[i].otherNameTag;
			
			MessageBox.showNameTag = true;
		}else
		{
			MessageBox.showNameTag = false;
		}
		
		if(mBoxQuest[i].enableFace)
		{
			if(mBoxQuest[i].faceSetup == FaceSetup.PlayerFace)
				MessageBox.faceStatic = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>().heroImage;
			else if(mBoxQuest[i].faceSetup == FaceSetup.NpcFace)
				MessageBox.faceStatic = npcFace;
			else if(mBoxQuest[i].faceSetup == FaceSetup.Other)
				MessageBox.faceStatic = mBoxQuest[i].otherFace;
			
			MessageBox.showFace = true;
		}else
		{
			MessageBox.showFace = false;
		}
		
		
		MessageBox.messageStatic = mBoxQuest[i].message;
		
	}
	
	void CallNextCommand()
	{
		if(npcType == NpcType.ShopKeeper)
		{
			GUI_Menu.instance.CallShop(this.gameObject);
			
		}else if(npcType == NpcType.QuestNpc)
		{
			
		}else if(npcType == NpcType.SaveNpc)
		{
			GUI_Menu.instance.CallSaveWindow(this.gameObject);
		} 
	}
	
	public void SetupDialogQuest(int id)
	{
		for(int i=0;i<questData.questSetting.Count;i++)
		{
			if(questID == questData.questSetting[i].questID)
			{
				
				if(questData.questSetting[i].questState == 1)
					CheckConditionQuest(id);
				
				if(questData.questSetting[i].questState == 0)
				{
					questNpcState = QuestNpcState.StartQuest;
					
					if(indexDialog == dialogQuest.Count-1)
					{
						QuestWindow.enableWindow = true;
						QuestWindow.enableButtonAccept = true;
						disableNext = true;
						questWindow.SetupQuestWindow(questID);
					}
					
				}else if(questData.questSetting[i].questState == 1)
				{
					questNpcState = QuestNpcState.InProgress;
					questWindow.SetupQuestWindow(questID);
					QuestWindow.enableWindow = true;
				}else if(questData.questSetting[i].questState == 2)
				{
					questNpcState = QuestNpcState.Complete;
				}else if(questData.questSetting[i].questState == 3)
				{
					questNpcState = QuestNpcState.AfterComplete;
				}
				
				indexCurrentQuest = i;
				
				break;	
			}
		}
	}
	
	void CheckConditionQuest(int id)
	{
		for(int i=0;i<questData.questSetting.Count;i++)
		{
			if(questID == questData.questSetting[i].questID)
			{
				if(questData.questSetting[i].questCondition == Quest_Data.QuestCondition.Hunting)
				{
					if(questData.questSetting[i].killCount >= (int)questData.questSetting[i].idCondition.y)
					{
						questData.questSetting[i].killCount = 0;
						questData.questSetting[i].questState = 2;
					}
				}else
				{
					if(CheckConditionItem((int)questData.questSetting[i].idCondition.x,(int)questData.questSetting[i].idCondition.y))
					{
						questData.questSetting[i].killCount = 0;
						questData.questSetting[i].questState = 2;
					}
				}
				
				
				break;	
			}
		}
	}
	
	bool CheckConditionItem(int id,int amout)
	{
		for(int i =0;i<inventory.bag_item.Count;i++)
		{
			if(inventory.bag_item[i].item_id == id && inventory.bag_item[i].item_amount >= amout)
			{
				inventory.bag_item[i].item_amount -= amout;
				
				if(inventory.bag_item[i].item_amount <= 0)
				{
					inventory.bag_item[i].item_id = 0;	
				}
				
				return true;
			}
		}
		
		return false;
	}
	
	void GiveReward(int id)
	{
		for(int i=0;i<questData.questSetting.Count;i++)
		{
			if(questID == questData.questSetting[i].questID)
			{
				inventory.GiveItem((int)questData.questSetting[i].itemIDReward.x,(int)questData.questSetting[i].itemIDReward.y);
				Debug.Log("Give");
				break;	
			}
		}
	}
}
