using UnityEngine;
using System.Collections;

public class DeadWindow : MonoBehaviour {
	
	[System.Serializable]
	public class GUISetting
	{
		public Vector2 position;
		public Vector2 size;
		public Texture2D texture;
	}
	
	[System.Serializable]
	public class ButtonSetting
	{
		public Vector2 position;
		public Vector2 size;
		public GUIStyle buttonStlye;
	}
	private Vector2 defaultScreenRes; 
	
	public GUISetting cautionWindow; 
	public ButtonSetting buttonReturn,buttonQuit; 
	public string sceneQuitGame; 
	
	//Private variable
	private HeroController controller;
	
	public static bool enableWindow; 
	
	// Use this for initialization
	void Start () {
		defaultScreenRes.x = 1920; 
		defaultScreenRes.y = 1080; 
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");  
		controller = go.GetComponent<HeroController>();
	}
	
	void OnGUI()
	{
        ResizeGUIMatrix();
		if(enableWindow)
		{
			//draw window
			GUI.DrawTexture(new Rect(cautionWindow.position.x,cautionWindow.position.y,cautionWindow.size.x,cautionWindow.size.y),cautionWindow.texture);
		
			//when click return
			if(GUI.Button(new Rect(buttonReturn.position.x,buttonReturn.position.y,buttonReturn.size.x,buttonReturn.size.y),"",buttonReturn.buttonStlye))
			{
				controller.Reborn();
			}	
			//when click quit game
			if(GUI.Button(new Rect(buttonQuit.position.x,buttonQuit.position.y,buttonQuit.size.x,buttonQuit.size.y),"",buttonQuit.buttonStlye))
			{
				Invoke("LoadScene",0.3f);
			}
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
	
	void LoadScene()
	{
		Application.LoadLevel(sceneQuitGame);
		enableWindow = false;
	} 
}
