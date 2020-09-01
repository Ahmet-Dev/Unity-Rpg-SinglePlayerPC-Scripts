using UnityEngine;
using System.Collections;

public class AlphaText : MonoBehaviour {
public float speedFade;
private float count;
	
	// Update is called once per frame
	void Update () {
		count += speedFade * Time.deltaTime;
		guiTexture.color = new Color(0.5f,0.5f,0.5f,Mathf.Sin(count)*0.5f);
	}
}
