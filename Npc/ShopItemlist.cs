using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopItemlist : MonoBehaviour {
	
	public List<int> itemID = new List<int>();
	
	void Start()
	{
		if(this.gameObject.tag == "Untagged")
			this.gameObject.tag = "Npc_Shop";
	}
	
}


