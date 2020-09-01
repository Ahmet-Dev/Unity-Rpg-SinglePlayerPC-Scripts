using UnityEngine;
using System.Collections;

public class NewGameButton : MonoBehaviour {
	
	public string loadSceneName; 
	public Texture2D buttonNormal,buttonDown; 
	public AudioClip buttonSfx;
	
	
	void OnMouseUp()
	{
		this.guiTexture.texture = buttonNormal;
		Invoke("LoadScene",0.1f);
	}
	
	void OnMouseDown()
	{
		if(buttonSfx != null)
			AudioSource.PlayClipAtPoint(buttonSfx,transform.position);
		
		this.guiTexture.texture = buttonDown;
	}
		
	void LoadScene()
	{
		Application.LoadLevel(loadSceneName);;	
	}
}
