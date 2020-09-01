using UnityEngine;
using System.Collections;

public class SelectCharacter : MonoBehaviour {
	
	public GameObject[] hero;  
	public GameObject buttonNext,buttonPrev; 

	[HideInInspector]
	public int indexHero = 0;  
	
	private GameObject[] heroInstance; 
	
	// Use this for initialization
	void Start () {
		
		heroInstance = new GameObject[hero.Length]; //add array size equal hero size
		indexHero = 0; //set default selected hero
		SpawnHero(); //spawn hero to display current selected
		
		if(hero.Length <= 1)
		{
			buttonNext.SetActive(false);
			buttonPrev.SetActive(false);
		}
		
	
	}
	
	//Check show only selected character
	public void UpdateHero(int _indexHero)
	{
		for(int i=0; i<hero.Length;i++)
		{
			//Show only select character
			if(i == _indexHero)
			{
				heroInstance[i].SetActive(true);
			}else
			{
				//Hide Other Character
				heroInstance[i].SetActive(false);
			}		
		}
	}
	
	//Spawn all hero
	public void SpawnHero()
	{
		for(int i=0;i<hero.Length;i++)
		{
			heroInstance[i] = (GameObject)Instantiate(hero[i],transform.position,transform.rotation);
		}
		
		UpdateHero(indexHero);
	}
	
}
