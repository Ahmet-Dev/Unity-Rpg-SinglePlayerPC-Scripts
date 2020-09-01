using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour {
	
	
	public delegate void AnimationHandle();
	public AnimationHandle animationState;
		
	
	[System.Serializable]
	public class AnimationType01
	{
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
	}
	[System.Serializable]
	public class AnimationType02
	{
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
		public bool speedTuning;
	}
	
	[System.Serializable]
	public class AnimationNormalAttack
	{
		public string _name = "Normal Attack";
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
		public float attackTimer = 0.5f;
		public float multipleDamage = 1f;
		public float flichValue;
		public bool speedTuning;
		
		public GameObject attackFX;
		public AudioClip soundFX;
		
	}
	[System.Serializable]
	public class AnimationCritAttack
	{
		public string _name = "Critical Attack";
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
		public float attackTimer = 0.5f;
		public float multipleDamage = 1f;
		public float flichValue;
		public bool speedTuning;
		
		public GameObject attackFX;
		public AudioClip soundFX;
		
	}
	
	[System.Serializable]
	public class AnimationTakeAttack
	{
		public string _name  = "Take Attack";
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
		
	}
	
	[System.Serializable]
	public class AnimationSkill
	{
		public string skillType;
		public int skillIndex;
		public AnimationClip animationSkill;
		public float speedAnimation;
		public float activeTimer;
		public bool speedTuning;
		
	}
	
	public AnimationType01 idle,cast,death; 
	public AnimationType02 move; 
	public List<AnimationNormalAttack> normalAttack; 
	public List<AnimationCritAttack> criticalAttack; 
	public List<AnimationTakeAttack> takeAttack; 
	
	[HideInInspector]
	public AnimationSkill skillSetup;
	[HideInInspector]
	public bool oneCheckDeadReset;

	private HeroController heroController;
	private PlayerStatus playerStatus;
	private PlayerSkill playerSkill;
	[HideInInspector]
	public bool checkAttack;
	
	[HideInInspector]
	public int sizeNAtk=0;
	[HideInInspector]
	public int sizeCritAtk=0;
	[HideInInspector]
	public int sizeTakeDmg=0;
	[HideInInspector]
	public List<bool> showNormalAtkSize = new List<bool>();
	[HideInInspector]
	public List<bool> showCritSize = new List<bool>();
	[HideInInspector]
	public List<bool> showTakeDmgSize = new List<bool>();
	
	// Use this for initialization
	void Start () {
		heroController = this.GetComponent<HeroController>();
		playerStatus = this.GetComponent<PlayerStatus>();
		playerSkill = this.GetComponent<PlayerSkill>();
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(animationState != null){
			animationState();	
		}
	
	}

	public void Idle(){
		GetComponent<Animation>().CrossFade(idle.animation.name);
		GetComponent<Animation>()[idle.animation.name].speed = idle.speedAnimation;
	}

	public void Move(){
		GetComponent<Animation>().Play(move.animation.name);
		
		if(move.speedTuning)  //Enable Speed Tuning
		{
			GetComponent<Animation>()[move.animation.name].speed = (playerStatus.statusCal.movespd/3f)/move.speedAnimation;	
		}else
		{
			GetComponent<Animation>()[move.animation.name].speed = move.speedAnimation;
		}
		
		
	}

	public void Attack()
	{	
		GetComponent<Animation>().Play(normalAttack[heroController.typeAttack].animation.name);
		
		if(normalAttack[heroController.typeAttack].speedTuning)  //Enable Speed Tuning
		{
			GetComponent<Animation>()[normalAttack[heroController.typeAttack].animation.name].speed = (playerStatus.statusCal.atkSpd/100f)/normalAttack[heroController.typeAttack].speedAnimation;	
		}else
		{
			GetComponent<Animation>()[normalAttack[heroController.typeAttack].animation.name].speed = normalAttack[heroController.typeAttack].speedAnimation;
		}
		if(GetComponent<Animation>()[normalAttack[heroController.typeAttack].animation.name].normalizedTime > normalAttack[heroController.typeAttack].attackTimer && !checkAttack)
		{
			EnemyController enemy;
			enemy = heroController.target.GetComponent<EnemyController>();
			enemy.EnemyLockTarget(heroController.gameObject);
			enemy.GetDamage((playerStatus.statusCal.atk) * normalAttack[heroController.typeAttack].multipleDamage ,(playerStatus.statusCal.hit),normalAttack[heroController.typeAttack].flichValue
				,normalAttack[heroController.typeAttack].attackFX,normalAttack[heroController.typeAttack].soundFX);
			checkAttack = true;
		}
			
		if(GetComponent<Animation>()[normalAttack[heroController.typeAttack].animation.name].normalizedTime > 0.9f)
		{
			heroController.ctrlAnimState = HeroController.ControlAnimationState.WaitAttack;
			checkAttack = false;
		}
	}

	public void CriticalAttack()
	{	
		GetComponent<Animation>().Play(criticalAttack[heroController.typeAttack].animation.name);
		
		if(criticalAttack[heroController.typeAttack].speedTuning)  
		{
			GetComponent<Animation>()[criticalAttack[heroController.typeAttack].animation.name].speed = (playerStatus.statusCal.atkSpd/100f)/criticalAttack[heroController.typeAttack].speedAnimation;	
		}else
		{
			GetComponent<Animation>()[criticalAttack[heroController.typeAttack].animation.name].speed = criticalAttack[heroController.typeAttack].speedAnimation;
		}
		if(GetComponent<Animation>()[criticalAttack[heroController.typeAttack].animation.name].normalizedTime > criticalAttack[heroController.typeAttack].attackTimer && !checkAttack)
		{
			EnemyController enemy;
			enemy = heroController.target.GetComponent<EnemyController>();
			enemy.EnemyLockTarget(heroController.gameObject);
			enemy.GetDamage((playerStatus.statusCal.atk) * criticalAttack[heroController.typeAttack].multipleDamage ,10000,criticalAttack[heroController.typeAttack].flichValue
				,criticalAttack[heroController.typeAttack].attackFX,criticalAttack[heroController.typeAttack].soundFX);
			checkAttack = true;
		}
			
		if(GetComponent<Animation>()[criticalAttack[heroController.typeAttack].animation.name].normalizedTime > 0.9f)
		{
			heroController.ctrlAnimState = HeroController.ControlAnimationState.WaitAttack;
			checkAttack = false;
		}
	}

	public void TakeAttack(){
		GetComponent<Animation>().CrossFade(takeAttack[heroController.typeTakeAttack].animation.name);
		GetComponent<Animation>()[takeAttack[heroController.typeTakeAttack].animation.name].speed = takeAttack[heroController.typeTakeAttack].speedAnimation;
		if(GetComponent<Animation>()[takeAttack[heroController.typeTakeAttack].animation.name].normalizedTime > 0.9f)
		{
			if(heroController.target != null)
			{
				heroController.ctrlAnimState = HeroController.ControlAnimationState.WaitAttack;
			}else
			{
				heroController.ctrlAnimState = HeroController.ControlAnimationState.Idle;
			}
		}
	}

	public void Cast()
	{
		GetComponent<Animation>().CrossFade(cast.animation.name);
		GetComponent<Animation>()[cast.animation.name].speed = cast.speedAnimation;
	}
	
	public void Death()
	{
		GetComponent<Animation>().CrossFade(death.animation.name);
		GetComponent<Animation>()[death.animation.name].speed = death.speedAnimation;
		
		if(GetComponent<Animation>()[death.animation.name].normalizedTime > 0.9f && !oneCheckDeadReset)
		{
			heroController.DeadReset();
			oneCheckDeadReset = true;
		}
	}
	
	public void ActiveSkill()
	{
		GetComponent<Animation>().Play(skillSetup.animationSkill.name);
		
		if(skillSetup.speedTuning)  
		{
			GetComponent<Animation>()[skillSetup.animationSkill.name].speed = (playerStatus.statusCal.atkSpd/100f)/skillSetup.speedAnimation;	
		}else
		{
			GetComponent<Animation>()[skillSetup.animationSkill.name].speed = skillSetup.speedAnimation;
		}
		if(GetComponent<Animation>()[skillSetup.animationSkill.name].normalizedTime > skillSetup.activeTimer && !checkAttack)
		{	
			playerSkill.ActiveSkill(skillSetup.skillType,skillSetup.skillIndex);
			checkAttack = true;
		}	
		if(GetComponent<Animation>()[skillSetup.animationSkill.name].normalizedTime > 0.9f)
		{
			heroController.ctrlAnimState = HeroController.ControlAnimationState.WaitAttack;
			checkAttack = false;
		}
	}
}
