using UnityEngine;
using System.Collections;

public class ButtonOK : MonoBehaviour {
	
	public string loadSceneName;   
	
	public Texture2D buttonOkNormal;  
	public Texture2D buttonOkDown;   
	public Entername enterNameScript; 
	public SelectCharacter selectCharacterScript; 
	public AudioClip sfxButton; 
	
	void OnMouseUp()
	{
		this.gameObject.guiTexture.texture = buttonOkNormal;
		PlayerPrefs.SetString("pName",enterNameScript.defaultName);
		PlayerPrefs.SetInt("pSelect",selectCharacterScript.indexHero);		
		SpawnPointHero.enableLoadData = true;
		Invoke("LoadScene",0.1f);
	}
	
	void OnMouseDown()
	{
		this.gameObject.guiTexture.texture = buttonOkDown;
		if(sfxButton != null)
		AudioSource.PlayClipAtPoint(sfxButton,transform.position);
		
	}
	
	void LoadScene()
	{
		Application.LoadLevel(loadSceneName);;	
	}
}
