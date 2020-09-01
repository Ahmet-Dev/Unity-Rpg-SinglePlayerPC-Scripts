using UnityEngine;
using System.Collections;

public class AreaSkillCursor : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
		this.gameObject.GetComponent<Renderer>().enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit h;
			if(Physics.Raycast(r, out h ,100)){
			
				if(h.collider.tag == "Enemy" || h.collider.tag == "Ground")
				{
					this.gameObject.GetComponent<Renderer>().enabled = true;
					this.transform.position = new Vector3(h.point.x,(h.point.y+0.3f),h.point.z);
				}
			if(Input.GetMouseButtonUp(0) && h.collider.tag == "Ground")
			{
				GameSetting.Instance.SetMouseCursor(0);
				Destroy(this.gameObject);
			}else
			
			if(Input.GetMouseButtonUp(1))
			{
				GameSetting.Instance.SetMouseCursor(0);
				Destroy(this.gameObject);
			}
		}
	}
	public void ConvertSizeSkillArea(float sizeSkill)
	{
		float newSize = sizeSkill / 4;
		this.transform.localScale = new Vector3(newSize,newSize,newSize);
	}
}
