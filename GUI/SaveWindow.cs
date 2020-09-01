using UnityEngine;
using System.Collections;

public class SaveWindow : MonoBehaviour {
	
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
	public ButtonSetting buttonSave,buttonCancel; 	
	public HeroController controller; 	
	public static bool enableWindow; 	
	// Use this for initialization
	void Start () {
		enableWindow = false;
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
			if(!controller.dontMove)
				controller.dontMove = true;
			GUI.DrawTexture(new Rect(cautionWindow.position.x,cautionWindow.position.y,cautionWindow.size.x,cautionWindow.size.y),cautionWindow.texture);
			if(GUI.Button(new Rect(buttonSave.position.x,buttonSave.position.y,buttonSave.size.x,buttonSave.size.y),"",buttonSave.buttonStlye))
			{
				CharacterData.SaveData();
				enableWindow = false;
			}
			if(GUI.Button(new Rect(buttonCancel.position.x,buttonCancel.position.y,buttonCancel.size.x,buttonCancel.size.y),"",buttonCancel.buttonStlye))
			{
				controller.dontMove = false;
				enableWindow = false;
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
	
}
