using UnityEngine;
using System.Collections;

public class GameSetting : MonoBehaviour {
	
	public static GameSetting Instance; 
	
	public Texture2D cursorNormal; 
	public Texture2D cursorAttack; 
	public Texture2D cursorSkill; 
	public Texture2D cursorPick; 
	public Texture2D cursorNpc; 
	public GameObject areaSkillCursor; 
	[HideInInspector]
	public GameObject areaSkillCursorObj;
	
	public GameObject mousefxNormal; 
	public GameObject mousefxAttack; 
	public GameObject mousefxInteract; 
	public GameObject levelUpEffect; 
	public GameObject castEffect; 
	
	public float deadExpPenalty; 
	
	public float logTimer; 
	public string logSettingNoMP; 
	public string logSettingCantBuy; 
	
	
	public bool hideMinimap; 
	public bool disableFaceRender; 
	
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = Vector2.zero;
	
	// Use this for initialization
	void Awake () {
		
		Instance = this;

	}

	public void SetMouseCursor(int cursorType)
	{
		if(cursorType == 0)
		{
			if(areaSkillCursorObj != null)
				Destroy(areaSkillCursorObj);
			
			Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);
		}
		
		if(cursorType == 1)
		{
			Cursor.SetCursor(cursorAttack, hotSpot, cursorMode);
		}
		
		if(cursorType == 2)
		{
			if(areaSkillCursorObj != null)
				Destroy(areaSkillCursorObj);
			
			Cursor.SetCursor(cursorSkill, hotSpot, cursorMode);
		}
		
		if(cursorType == 3)
		{
			areaSkillCursorObj = (GameObject)Instantiate(areaSkillCursor,transform.position,Quaternion.identity);
		}
		
		if(cursorType == 4)
		{
			if(areaSkillCursorObj != null)
				Destroy(areaSkillCursorObj);
			Cursor.SetCursor(cursorNpc, hotSpot, cursorMode);
		}
		
		if(cursorType == 5)
		{
			if(areaSkillCursorObj != null)
				Destroy(areaSkillCursorObj);
			Cursor.SetCursor(cursorPick, hotSpot, cursorMode);
		}
		
	}
	
}
