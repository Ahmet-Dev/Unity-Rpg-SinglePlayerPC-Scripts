using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	
	public string playerName; 	
	[System.Serializable]
	public class Attribute
	{
		public int lv,hp,mp,atk,def,spd,hit;
		public float criticalRate,atkSpd,atkRange,movespd,exp;
	}
	[System.Serializable]
	public class SubAttribute
	{
		public int hp,mp,atk,def,spd,hit;
		public float criticalRate,atkSpd,atkRange,movespd;
	}
	[System.Serializable]
	public class SumAttribute
	{
		public int hp,mp,atk,def,spd,hit;
		public float criticalRate,atkSpd,atkRange,movespd;
	}
	[System.Serializable]
	public class StatusGrowth
	{
		public int hp,mp,atk,def,spd,hit;
		public float criticalRate,atkSpd,atkRange,movespd;
	}
	[System.Serializable]
	public class SumEquip
	{
		public int hp,mp,atk,def,spd,hit;
		public float criticalRate,atkSpd,atkRange,movespd;
	}

	[HideInInspector]
	public int pointCurrent = 0;
	public int maxLv = 99; 
	public int pointPerLv; 
	public int startExp; 
	public float multipleExp; 

	[HideInInspector]
	public int hpMax,mpMax,hpCurrent,mpCurrent;
	
	[HideInInspector]
	public float expMax;
	
	public Attribute status;  
	public SubAttribute statusAdd; 
	public SumAttribute statusCal; 
	public SumEquip statusEquip;
	public StatusGrowth statusGrowth,growthSetting; 
	public float hpRegenTime; 
	public float mpRegenTime; 
	private bool checkStatus;
	
	[HideInInspector]
	public bool alreadyApply = true;
	
	// Use this for initialization
	void Start () {

		expMax = startExp * status.lv *multipleExp;
		Invoke("SettingStatus",0.1f);
		alreadyApply = true;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.F1))
		{
			status.exp += 1000;	
		}
	
		UpdateExp();
		
		if(checkStatus)
		CheckHPMPMax();
		
		if(statusCal.hp <= 0)
		{
			CancelInvoke("RegenerationHP");
			CancelInvoke("RegenerationMP");
		}
	
	}
	public void UpdateStatus()
	{
		hpMax =  status.hp + statusAdd.hp + statusGrowth.hp + statusEquip.hp;
		mpMax = status.mp + statusAdd.mp + statusGrowth.mp + statusEquip.mp;
	}

	void CheckHPMPMax()
	{
		if(statusCal.hp > hpMax)
		{
			statusCal.hp = hpMax;	
		}
		
		if(statusCal.mp > mpMax)
		{
			statusCal.mp = mpMax;	
		}
	}
	
	void RegenerationHP()
	{
		statusCal.hp += 1;
	}
	
	void RegenerationMP()
	{
		statusCal.mp += 1;
	}
	
	public void UpdateExp()
	{
		if(status.lv >= maxLv)
		{
			status.exp = 0;
			expMax = 0;
		}else
		if(status.exp >= expMax)
		{
			SoundManager.instance.PlayingSound("Level_Up");
			Instantiate(GameSetting.Instance.levelUpEffect,new Vector3(transform.position.x,transform.position.y+0.01f,transform.position.z),Quaternion.identity);
			pointCurrent += pointPerLv;
			
			GUI_Menu.instance.statWindow[0].defPoint = pointCurrent;
			
			status.exp -= expMax;
			status.lv++;
			expMax = startExp * status.lv *multipleExp;
			CalculateStatusGrowth();
			UpdateAttribute();
			alreadyApply = false;
			statusCal.hp = hpMax;
			statusCal.mp = mpMax;
		}
	}
	public void UpdateAttribute()
	{	
		checkStatus = false;
		if(statusCal.hp == hpMax)
		statusCal.hp = status.hp + statusAdd.hp + statusGrowth.hp + statusEquip.hp;
		else
		statusCal.hp = statusCal.hp + statusAdd.hp;
		
		if(statusCal.mp == mpMax)
		statusCal.mp = status.mp + statusAdd.mp + statusGrowth.mp + statusEquip.mp;
		else
		statusCal.mp = statusCal.mp + statusAdd.mp;
		statusCal.atk = status.atk + statusAdd.atk + statusGrowth.atk + statusEquip.atk;
		statusCal.def = status.def + statusAdd.def + statusGrowth.def + statusEquip.def;
		statusCal.spd = status.spd + statusAdd.spd + statusGrowth.spd + statusEquip.spd;
		statusCal.hit = status.hit + statusAdd.hit + statusGrowth.hit + statusEquip.hit;
		statusCal.criticalRate = status.criticalRate + statusAdd.criticalRate + statusGrowth.criticalRate + statusEquip.criticalRate;
		statusCal.atkSpd = status.atkSpd + statusAdd.atkSpd + statusGrowth.atkSpd + statusEquip.atkSpd;
		statusCal.atkRange = status.atkRange + statusAdd.atkRange + statusGrowth.atkRange + statusEquip.atkRange;
		statusCal.movespd = status.movespd + statusAdd.movespd + statusGrowth.movespd + statusEquip.movespd;
		UpdateStatus();
		checkStatus = true;
	}

	public void CalculateStatusGrowth()
	{
		ResetStatusGrowth();
		
		if(growthSetting.hp > 0)
		{
			statusGrowth.hp = status.lv * growthSetting.hp;
		}
		if(growthSetting.mp > 0)
		{
			statusGrowth.mp = status.lv * growthSetting.mp;
		}
		if(growthSetting.atk > 0)
		{
			statusGrowth.atk = status.lv * growthSetting.atk;
		}
		if(growthSetting.def > 0)
		{
			statusGrowth.def = status.lv * growthSetting.def;
		}
		if(growthSetting.spd > 0)
		{
			statusGrowth.spd = status.lv * growthSetting.spd;
		}
		if(growthSetting.hit > 0)
		{
			statusGrowth.hit = status.lv * growthSetting.hit;
		}
		if(growthSetting.criticalRate > 0)
		{
			statusGrowth.criticalRate = status.lv * growthSetting.criticalRate;
		}
		if(growthSetting.atkSpd > 0)
		{
			statusGrowth.atkSpd = status.lv * growthSetting.atkSpd;
		}
		if(growthSetting.atkRange > 0)
		{
			statusGrowth.atkRange = status.lv * growthSetting.atkRange;
		}
		if(growthSetting.movespd > 0)
		{
			statusGrowth.movespd = status.lv * growthSetting.movespd;
		}	
	}

	void ResetStatusGrowth()
	{
		statusGrowth.hp = 0;
		statusGrowth.mp = 0;
		statusGrowth.atk = 0;
		statusGrowth.def = 0;
		statusGrowth.spd = 0;
		statusGrowth.hit = 0;
		statusGrowth.criticalRate = 0;
		statusGrowth.atkSpd = 0;
		statusGrowth.atkRange = 0;
		statusGrowth.movespd = 0;
	}
	
	public void StartRegen()
	{
		InvokeRepeating("RegenerationHP",hpRegenTime,hpRegenTime);
		InvokeRepeating("RegenerationMP",mpRegenTime,mpRegenTime);
	}

	public void Save()
	{
		PlayerPrefs.SetString("pName",playerName);
		PlayerPrefs.SetInt("pLv",status.lv);
		PlayerPrefs.SetInt("pHP",status.hp);
		PlayerPrefs.SetInt("pMP",status.mp);
		PlayerPrefs.SetInt("pAtk",status.atk);
		PlayerPrefs.SetInt("pDef",status.def);
		PlayerPrefs.SetInt("pSpd",status.spd);
		PlayerPrefs.SetInt("pHit",status.hit);
		PlayerPrefs.SetFloat("pCriRate",status.criticalRate);
		PlayerPrefs.SetFloat("pAtkSpd",status.atkSpd);
		PlayerPrefs.SetFloat("pAtkRange",status.atkRange);
		PlayerPrefs.SetFloat("pMovespd",status.movespd);
		PlayerPrefs.SetFloat("pExp",status.exp);
		
		PlayerPrefs.SetInt("pStat",pointCurrent);
		PlayerPrefs.SetInt("alreadyApply",alreadyApply ? 1:0);
		
	}
	
	public void Load()
	{
		playerName = PlayerPrefs.GetString("pName",playerName);
		status.lv = PlayerPrefs.GetInt("pLv",status.lv);
		status.hp = PlayerPrefs.GetInt("pHP",status.hp);
		status.mp = PlayerPrefs.GetInt("pMP",status.mp);
		status.atk = PlayerPrefs.GetInt("pAtk",status.atk);
		status.def = PlayerPrefs.GetInt("pDef",status.def);
		status.spd = PlayerPrefs.GetInt("pSpd",status.spd);
		status.hit = PlayerPrefs.GetInt("pHit",status.hit);
		status.criticalRate = PlayerPrefs.GetFloat("pCriRate",status.criticalRate);
		status.atkSpd = PlayerPrefs.GetFloat("pAtkSpd",status.atkSpd);
		status.atkRange = PlayerPrefs.GetFloat("pAtkRange",status.atkRange);
		status.movespd = PlayerPrefs.GetFloat("pMovespd",status.movespd);
		status.exp = PlayerPrefs.GetFloat("pExp",status.exp);
		pointCurrent = PlayerPrefs.GetInt("pStat",pointCurrent);
		alreadyApply = PlayerPrefs.GetInt("alreadyApply") == 1 ? true : false;
		SettingStatusLoad();	
	}

	void SettingStatus()
	{
		alreadyApply = true;
		checkStatus = true;  
		expMax = startExp * status.lv *multipleExp;
		CalculateStatusGrowth();
		UpdateAttribute();
		InvokeRepeating("RegenerationHP",hpRegenTime,hpRegenTime);
		InvokeRepeating("RegenerationMP",mpRegenTime,mpRegenTime);
	}

	void SettingStatusLoad()
	{
		checkStatus = true;  
		expMax = startExp * status.lv *multipleExp;
		CalculateStatusGrowth();
		UpdateAttribute();
	}

}
