using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour {
	
	[HideInInspector]
	public GameObject target;
    public float xSpeed; 
	public float ySpeed; 
	public float yMinLimit; 
	public float yMaxLimit; 
	
    public float scrollSpeed; 
	public float zoomMin;  
	public float zoomMax; 
	private float distance;
	private float distanceLerp;
	private Vector3 position; 
    private bool isActivated;
    private float x;
	private float y;
	private bool setupCamera;

    // Use this for initialization
 
    void Start () {
		if(target == null)
		{
			target = GameObject.FindGameObjectWithTag("Player");
			
			if(target == null)
			{
				Debug.LogWarning("Don't found player tag please change player tag to Player");	
			}
		}
		 Vector3 angles = transform.eulerAngles;
		 x = angles.y;
		 y = angles.x;
				
		CalDistance();
    }

    void LateUpdate () {
 		
		ScrollMouse();
		RotateCamera();
	}
	
	void RotateCamera()
	{
		if (Input.GetMouseButtonDown(1)){
 
			isActivated = true;
 
		} 

		if (Input.GetMouseButtonUp(1))
		{
			isActivated = false;
		} 

	    if (target && isActivated) { 
		  y -= Input.GetAxis("Mouse Y") * ySpeed;
		  x += Input.GetAxis("Mouse X") * xSpeed;
	      y = ClampAngle(y, yMinLimit, yMaxLimit);
	    Quaternion rotation = Quaternion.Euler(y, x, 0);
		Vector3 calPos = new Vector3(0, 0, -distanceLerp);
		position = rotation * calPos + target.transform.position;
		transform.rotation = rotation;
		transform.position = position;
		} else
		{
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 calPos = new Vector3(0, 0, -distanceLerp);
	        position = rotation * calPos + target.transform.position;
	 		transform.rotation = rotation;
	        transform.position = position;
		}
	}

 	void CalDistance()
	{
		distance = zoomMax;
		distanceLerp = distance;
		Quaternion rotation = Quaternion.Euler(y, x, 0);	
		Vector3 calPos = new Vector3(0, 0, -distanceLerp);
	    position = rotation * calPos + target.transform.position;
	    transform.rotation = rotation;
	    transform.position = position;
	}

	void ScrollMouse()
	{
		distanceLerp = Mathf.Lerp(distanceLerp,distance,Time.deltaTime * 5);
		if (Input.GetAxis("Mouse ScrollWheel") != 0 && !GUI_Menu.instance.CheckHoverItemShop() && !GUI_Menu.instance.CheckHoverSkillWindow()) 
			{	
			distance = Vector3.Distance (transform.position , target.transform.position);	
			distance = ScrollLimit(distance - Input.GetAxis("Mouse ScrollWheel")*scrollSpeed, zoomMin, zoomMax);
			}
	}

	float ScrollLimit(float dist, float min, float max)
    {
        if (dist < min)
            dist= min;
        if (dist > max)
            dist= max; 
		return dist;
    }
	
	float ClampAngle(float angle,float min,float max)
	{
		if(angle < -360)
			angle += 360;
		if(angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle,min,max);
	}
}
