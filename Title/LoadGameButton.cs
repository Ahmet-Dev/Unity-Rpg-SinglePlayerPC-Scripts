using UnityEngine;
using System.Collections;

public class LoadGameButton : MonoBehaviour {
	
	
	public string loadSceneName; 
	public Texture2D buttonNormal,buttonDown,buttonDisable; 
	public AudioClip buttonSfx; 
	private string checkData;
	public static bool loadData;
	
	void Start()
	{
		checkData = PlayerPrefs.GetString("Enable Load");
		
		if(checkData == "True")
		{
			this.guiTexture.texture = buttonNormal;
		}else
		{
			this.guiTexture.texture = buttonDisable;
		}
	}
	
	void OnMouseUp()
	{
		if(checkData == "True")
		{
			this.guiTexture.texture = buttonNormal;
			Invoke("LoadScene",0.1f);
		}
	}
	
	void OnMouseDown()
	{
		if(checkData == "True")
		{
			if(buttonSfx != null)
			AudioSource.PlayClipAtPoint(buttonSfx,transform.position);	
			this.guiTexture.texture = buttonDown;
			loadData = true;
		}
	}
		
	void LoadScene()
	{
		Application.LoadLevel(loadSceneName);;	
	}
}
