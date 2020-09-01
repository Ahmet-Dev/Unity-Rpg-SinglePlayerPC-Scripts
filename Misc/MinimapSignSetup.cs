using UnityEngine;
using System.Collections;

public class MinimapSignSetup : MonoBehaviour {
	public enum MinimapSignType{Player,Enemy,Boss,Npc,ShopWeapon,ShopPotion,SavePoint,Quest}
	public MinimapSignType signType;
	
	// Use this for initialization
	void Start () {
		
		if(!GameSetting.Instance.hideMinimap)
		{
			TextureSetup();
			this.gameObject.layer = 12;
		}else
		{
			if(this.gameObject.activeSelf == true)
			this.gameObject.SetActive(false);	
		}
		
	}
	void TextureSetup ()
	{
		if(signType == MinimapSignType.Player)
		{
			this.gameObject.GetComponent<Renderer>().material = MinimapSign.Instance.minimapSignMat[0];	
		}else if(signType == MinimapSignType.Enemy)
		{
			this.gameObject.GetComponent<Renderer>().material = MinimapSign.Instance.minimapSignMat[1];	
		}else if(signType == MinimapSignType.Boss)
		{
			this.gameObject.GetComponent<Renderer>().material = MinimapSign.Instance.minimapSignMat[2];	
		}else if(signType == MinimapSignType.Npc)
		{
			this.gameObject.GetComponent<Renderer>().material = MinimapSign.Instance.minimapSignMat[3];	
		}else if(signType == MinimapSignType.ShopWeapon)
		{
			this.gameObject.GetComponent<Renderer>().material = MinimapSign.Instance.minimapSignMat[4];	
		}else if(signType == MinimapSignType.ShopPotion)
		{
			this.gameObject.GetComponent<Renderer>().material = MinimapSign.Instance.minimapSignMat[5];	
		}else if(signType == MinimapSignType.SavePoint)
		{
			this.gameObject.GetComponent<Renderer>().material = MinimapSign.Instance.minimapSignMat[6];	
		}else if(signType == MinimapSignType.Quest)
		{
			this.gameObject.GetComponent<Renderer>().material = MinimapSign.Instance.minimapSignMat[7];	
		}
			
	}
	
}
