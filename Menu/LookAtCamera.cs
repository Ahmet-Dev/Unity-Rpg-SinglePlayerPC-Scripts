using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
	
	private Vector3 targetLook;
	
	void Update () {
		
		transform.LookAt(Camera.main.transform.position);
	}
}
