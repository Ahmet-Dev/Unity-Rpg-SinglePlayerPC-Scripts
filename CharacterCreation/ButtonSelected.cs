using UnityEngine;
using System.Collections;

public class ButtonSelected : MonoBehaviour {
	
	public string buttonName; 
	
	public Texture2D buttonOkNormal; 
	public Texture2D buttonOkDown; 
	public SelectCharacter selectCharacterScript;//script select character
	public AudioClip sfxButton; 
	
	void OnMouseUp()
	{
		this.gameObject.guiTexture.texture = buttonOkNormal;
		
		if(buttonName == "Next")
		{
			selectCharacterScript.indexHero += 1;
			
			if(selectCharacterScript.indexHero >= selectCharacterScript.hero.Length)
			{
				selectCharacterScript.indexHero = 0;
			}
			
			selectCharacterScript.UpdateHero(selectCharacterScript.indexHero);
			
		}
		
		if(buttonName == "Prev")
		{
			selectCharacterScript.indexHero -= 1;	
			
			if(selectCharacterScript.indexHero < 0)
			{
				selectCharacterScript.indexHero = selectCharacterScript.hero.Length-1;
			}
			
			selectCharacterScript.UpdateHero(selectCharacterScript.indexHero);
		}
		
		
	}
	
	void OnMouseDown()
	{
		this.gameObject.guiTexture.texture = buttonOkDown;
		AudioSource.PlayClipAtPoint(sfxButton,transform.position);	
	}
	

}
