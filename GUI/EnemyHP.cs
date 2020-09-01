using UnityEngine;
using System.Collections;

public class EnemyHP : MonoBehaviour {
	
	private Vector2 defaultScreenRes; 
	public static EnemyHP Instance; 
	public Vector2 posHPbar; 
	public Vector2 sizeHPBar; 
	public Texture2D hpBar; 
	public Texture2D hpCurrent; 
	
	public Vector2 posEnemyName; 
	public Vector2 posHPText; 
	public GUIStyle nameStyle;	
	public GUIStyle hpNumberStyle; 
	
	private bool showHPBar; 
	private float maxHP;	
	private float curHP;	
	private string enemyName = "enemy name";	
	
	// Use this for initialization
	void Start () {
		
		defaultScreenRes.x = 1920; 
		defaultScreenRes.y = 1080; 
		
		Instance = this; 
		
	}
	
	
	 void OnGUI()
     {	
        ResizeGUIMatrix();
		//Enemy HP bar		
		if(showHPBar)
		{
		
		TextFilter.DrawOutline(new Rect(posEnemyName.x ,posEnemyName.y, 1000 , 1000)
			,enemyName,nameStyle,Color.black,Color.white,2f);

		 GUI.BeginGroup(new Rect(posHPbar.x, posHPbar.y,sizeHPBar.x,sizeHPBar.y));
	       GUI.DrawTexture(new Rect(0,0, sizeHPBar.x ,sizeHPBar.y), hpBar);
			
		        GUI.BeginGroup(new Rect(0,0,ConvertHP(sizeHPBar.x,maxHP,curHP),sizeHPBar.y));
				
		         GUI.DrawTexture(new Rect(0,0,sizeHPBar.x,sizeHPBar.y), hpCurrent);
		         GUI.EndGroup();
			
	       GUI.EndGroup();	
		TextFilter.DrawOutline(new Rect(posHPText.x ,posHPText.y, 1000 , 1000)
			,curHP.ToString() + " / " + maxHP.ToString(),hpNumberStyle,Color.black,Color.white,2f);
		}
        GUI.matrix = Matrix4x4.identity;		
    }
	
	void ResizeGUIMatrix()
    {
       Vector2 ratio = new Vector2(Screen.width/defaultScreenRes.x , Screen.height/defaultScreenRes.y );
       Matrix4x4 guiMatrix = Matrix4x4.identity;
       guiMatrix.SetTRS(new Vector3(1, 1, 1), Quaternion.identity, new Vector3(ratio.x, ratio.y, 1));
       GUI.matrix = guiMatrix;
    }
	
	public void ShowHPbar(bool showHP) //Control hp bar
	{
		showHPBar = showHP; 
	}
	
	public void GetHPTarget(float maxHPf, float curHPf,string enemyNamef)
	{
		maxHP = maxHPf;
		curHP = curHPf;
		enemyName = enemyNamef;
	}
	
	float ConvertHP(float maxWidthGUI, float maxHP, float curHP) 
	 {
	  float val = maxWidthGUI/maxHP;
	  float load = curHP*val; 
	  return load;
	 }
}