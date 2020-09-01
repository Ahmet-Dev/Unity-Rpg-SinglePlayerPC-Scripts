using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour {
	
	public static int zoomLevel; 
	public static MinimapCamera Instance; 
	private int zoomCurrent;
	
	[HideInInspector]
	public Transform Target;
	
	void Start()
	{
		zoomLevel = 3;
		zoomCurrent = zoomLevel;
		Instance = this;
		GameObject hero;
		hero = GameObject.FindGameObjectWithTag("Player");
		Target = hero.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(Target.position.x,transform.position.y,Target.position.z);
		ZoomManage ();
	
	}
	
	void ZoomManage ()
	{
		if(zoomLevel < 0)
		{
			zoomLevel = 0;
		}else if(zoomLevel > 4)
		{
			zoomLevel = 4;	
		}
	}
	
	public void ZoomUpdate()
	{
		if(zoomLevel < zoomCurrent)
		{
			this.GetComponent<Camera>().orthographicSize += 3;
			zoomCurrent = zoomLevel;
		}else
		
		if(zoomLevel > zoomCurrent)
		{
			this.GetComponent<Camera>().orthographicSize -= 3;
			zoomCurrent = zoomLevel;
		}
	}
}
