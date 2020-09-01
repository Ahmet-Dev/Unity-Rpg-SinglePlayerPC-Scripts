using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
	
	private Vector2 defaultScreenRes; //Screen Resolution
	
	[System.Serializable]
	public class GUISetting
	{
		public Vector2 position;
		public Vector2 size;
		public Texture2D[] texture;
	}
	
	[System.Serializable]
	public class LabelSetting
	{
		public Vector2 position;
		public GUIStyle labelStyle;
		
	}
	
	[System.Serializable]
	public class MinimapSetting
	{
		public Vector2 position;
		public Vector2 size;
		public Texture texture;
		public Material renderMaterial;
	}
	
	[System.Serializable]
	public class ButtonSetting
	{
		public Vector2 position;
		public Vector2 size;
		public GUIStyle buttonStlye;
	}
	
	public GUISetting frameMap,mapNameBar; 
	public MinimapSetting minimap; 
	public LabelSetting mapName; 
	public ButtonSetting zoomInBt,zoomOutBt; 
	
	// Use this for initialization
	void Start () {
		
		defaultScreenRes.x = 1920; 
		defaultScreenRes.y = 1080; 
	
	}
	
	void OnGUI ()
	{
		
		if(!GameSetting.Instance.hideMinimap)
		{
	        ResizeGUIMatrix();
			Graphics.DrawTexture(new Rect(minimap.position.x,minimap.position.y,minimap.size.x ,minimap.size.y),minimap.texture,minimap.renderMaterial);
			GUI.DrawTexture(new Rect(frameMap.position.x,frameMap.position.y,frameMap.size.x,frameMap.size.y),frameMap.texture[0]);
			GUI.DrawTexture(new Rect(mapNameBar.position.x,mapNameBar.position.y,mapNameBar.size.x,mapNameBar.size.y),mapNameBar.texture[0]);		
			TextFilter.DrawOutline(new Rect(mapName.position.x ,mapName.position.y, 1000 , 1000)
				,Application.loadedLevelName,mapName.labelStyle,Color.black,Color.white,2f);
			
			if(GUI.Button(new Rect(zoomInBt.position.x,zoomInBt.position.y,zoomInBt.size.x,zoomInBt.size.y),"",zoomInBt.buttonStlye))
			{
				MinimapCamera.zoomLevel++;
				MinimapCamera.Instance.ZoomUpdate();
			}

			if(GUI.Button(new Rect(zoomOutBt.position.x,zoomOutBt.position.y,zoomOutBt.size.x,zoomOutBt.size.y),"",zoomOutBt.buttonStlye))
			{
				MinimapCamera.zoomLevel--;
				MinimapCamera.Instance.ZoomUpdate();
			}
			
		        GUI.matrix = Matrix4x4.identity;
		}else
		{
			this.enabled = false;
		}
				
	}
	
	void ResizeGUIMatrix()
    {
       Vector2 ratio = new Vector2(Screen.width/defaultScreenRes.x , Screen.height/defaultScreenRes.y );
       Matrix4x4 guiMatrix = Matrix4x4.identity;
       guiMatrix.SetTRS(new Vector3(1, 1, 1), Quaternion.identity, new Vector3(ratio.x, ratio.y, 1));
       GUI.matrix = guiMatrix;
    }
}
