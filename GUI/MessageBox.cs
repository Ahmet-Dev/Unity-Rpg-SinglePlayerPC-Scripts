using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour {

	private Vector2 defaultScreenRes; //Screen Resolution
	
	[System.Serializable]
	public class GUISetting
	{
		public Vector2 position;
		public Vector2 size;
		public Texture2D texture;
	}
	
	[System.Serializable]
	public class PositionSetting
	{
		public Vector2 position;
		public Vector2 size;
	}
	
	[System.Serializable]
	public class LabelSetting
	{
		public Vector2 position;
		public GUIStyle style;
		public bool enableStroke;
		public Color strokeColor;
	}
	
	public GUISetting messageBox,nameTag,nextIcon,faceFrame;
	public PositionSetting face;
	public LabelSetting nameTagSetting,MessageBoxSetting;
	
	[Multiline]
	public static string nameTagStatic,messageStatic;
	public static Texture2D faceStatic;
	
	public static bool showMessageBox,showNameTag,showFace;
	
	// Use this for initialization
	void Start () {
		
		defaultScreenRes.x = 1920; 
		defaultScreenRes.y = 1080; 
	}
	
	void OnGUI () {

        ResizeGUIMatrix();
		
		if(showMessageBox)
		{
			GUI.DrawTexture(new Rect(messageBox.position.x,messageBox.position.y,messageBox.size.x,messageBox.size.y),messageBox.texture);
		
			GUI.DrawTexture(new Rect(nextIcon.position.x,nextIcon.position.y,nextIcon.size.x,nextIcon.size.y),nextIcon.texture);

			if(MessageBoxSetting.enableStroke)
			TextFilter.DrawOutline(new Rect(MessageBoxSetting.position.x ,MessageBoxSetting.position.y, 1000 , 1000)
				,messageStatic,MessageBoxSetting.style,MessageBoxSetting.strokeColor,MessageBoxSetting.style.normal.textColor,2f);
			else
				GUI.Label(new Rect(MessageBoxSetting.position.x ,MessageBoxSetting.position.y, 1000 , 1000),messageStatic,MessageBoxSetting.style);
			
		}
		
		if(showNameTag)
		{
			GUI.DrawTexture(new Rect(nameTag.position.x,nameTag.position.y,nameTag.size.x,nameTag.size.y),nameTag.texture);
			
			if(nameTagSetting.enableStroke)
			TextFilter.DrawOutline(new Rect(nameTagSetting.position.x ,nameTagSetting.position.y, 1000 , 1000)
				,nameTagStatic,nameTagSetting.style,nameTagSetting.strokeColor,nameTagSetting.style.normal.textColor,2f);
			else
				GUI.Label(new Rect(nameTagSetting.position.x ,nameTagSetting.position.y, 1000 , 1000),nameTagStatic,nameTagSetting.style);	
		}
		
		if(showFace)
		{
				
			GUI.DrawTexture(new Rect(face.position.x,face.position.y,face.size.x,face.size.y),faceStatic);	
			GUI.DrawTexture(new Rect(faceFrame.position.x,faceFrame.position.y,faceFrame.size.x,faceFrame.size.y),faceFrame.texture);
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
