using UnityEngine;
using System.Collections;

public class DisableFaceCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		if(GameSetting.Instance.disableFaceRender)
		{
			this.gameObject.SetActive(false);
		}
	
	}
	
}
