using UnityEngine;
using System.Collections;

public class ControlTitleScene : MonoBehaviour {
	
	public Transform targetPoint; 
	public AudioSource bgm; 
	public GameObject titleText,pressStart,buttonNew,buttonLoad,whiteScreen; 
	public AudioClip buttonSound; 
	private int titlePattern = 0;
	private float alpha = 0.5f;

	// Use this for initialization
	void Start () {
		alpha = 0.5f;
		whiteScreen.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, 3 * Time.deltaTime);		
		if(titlePattern == 0)
		{
			if(alpha > 0)
			{
				alpha -= Time.deltaTime * 0.2f;
    			whiteScreen.guiTexture.color = new Color(.5f,.5f,.5f, alpha);
			}else
			{
				bgm.Play();	
			}
			
			if(transform.position.z >= -27.0f)
			{
				titlePattern = 1;
				alpha = 0;
				titleText.SetActive(true);
			}	
		}
		
		if(titlePattern == 1)
		{
			if(alpha < 0.5f)
			{
				alpha += Time.deltaTime * 0.5f;
    			titleText.guiTexture.color = new Color(.5f,.5f,.5f, alpha);
			}else
			{
				titlePattern = 2;
				pressStart.SetActive(true);	
			}
		}
		
		if(titlePattern == 2)
		{
			if(Input.anyKey)
			{
				pressStart.SetActive(false);
				if(buttonSound != null)
				{
					AudioSource.PlayClipAtPoint(buttonSound,transform.position);	
				}
				titlePattern = 3;
			}
		}
		
		if(titlePattern == 3)
		{
			buttonNew.SetActive(true);
			buttonLoad.SetActive(true);
		}
		
	}
}
