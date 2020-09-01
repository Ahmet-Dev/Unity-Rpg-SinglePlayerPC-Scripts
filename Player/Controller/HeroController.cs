using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ClassType{None,Swordman,Archer,Mage}

[RequireComponent (typeof (CharacterController))]
[RequireComponent (typeof (AnimationManager))]
[RequireComponent (typeof (PlayerStatus))]
[RequireComponent (typeof (PlayerSkill))]
public class HeroController : MonoBehaviour {
	
	public enum ControlAnimationState {Idle,Move,WaitAttack,Attack,Cast,ActiveSkill,TakeAtk,Death}; 
	public Texture2D heroImage;
	public ClassType classType; 
	public GameObject target;  
	public GameObject targetHP;
	public GameObject targetMoveTo;
	[SerializeField]
	public List<GameObject> modelMesh;  
	public Color colorTakeDamage;	
	
	public ControlAnimationState ctrlAnimState; 
	public bool autoAttack;
	private AnimationManager animationManager;
	private PlayerStatus playerStatus;
	private PlayerSkill playerSkill;
	private GUI_Menu guiMenu;
	private float delayAttack = 100;	
	private Vector3 destinationPosition;		
	private float destinationDistance;			
	private Vector3 movedir;
	private CharacterController controller;
	private float moveSpeed;						
	private Vector3 ctargetPos;					
	private Vector3 targetPos;					
	private Quaternion targetRotation;			
	private bool checkCritical;				
	private bool onceAttack;				
	private float flinchValue;				
	private Color[] defaultColor;			
	private bool getSkillTarget;             
	private bool alreadyLockSkill;			
	public bool useSkill;              
	public bool useFreeSkill;                  
	public Vector3 freePosSkill;			
        
	[HideInInspector]
	public float skillRange;                
	[HideInInspector]
	public int castid;					
	[HideInInspector]
	public GameObject DeadSpawnPoint;    
	[HideInInspector]
	public int typeAttack;					
	[HideInInspector]
	public int typeTakeAttack;			
	
	public bool dontMove;
	public bool dontClick;     
	
	private bool oneShotOpenDeadWindow;
	
	public int layerActiveGround = 11;
	public int layerActiveItem = 10;
	public int layerActiveEnemy = 9;
	public int layerActiveNpc = 13;
	
	[HideInInspector]
	public int sizeMesh;
	
	// Use this for initialization
	void Start () {		
		
		layerActiveGround = 11;
		layerActiveItem = 10;
		layerActiveEnemy = 9;
		destinationPosition = this.transform.position;
		animationManager = this.GetComponent<AnimationManager>();
		playerSkill = this.GetComponent<PlayerSkill>();
		playerStatus = this.GetComponent<PlayerStatus>();
		controller = this.GetComponent<CharacterController>();
		guiMenu = GameObject.Find("Inventory").GetComponent<GUI_Menu>();
		flinchValue = 100; 
		delayAttack = 100; 	
		defaultColor = new Color[modelMesh.Count];
		DeadSpawnPoint = GameObject.FindGameObjectWithTag("SpawnHero");
		 SetDefualtColor();	
	}
	// Update is called once per frame
	void Update () {
		TargetLock();
		HeroAnimationState();
		if(ctrlAnimState != ControlAnimationState.Death && ctrlAnimState != ControlAnimationState.Cast && ctrlAnimState != ControlAnimationState.ActiveSkill && dontMove == false)
		{
			ClickToMove();
			CancelSkill();
		}else if(dontMove == true){
			ctrlAnimState = ControlAnimationState.Idle;
		}
	}
	
	void CancelSkill()
	{
		//Ray to enemy
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit h;

		if(Input.GetMouseButtonDown(1) && useSkill && ctrlAnimState != ControlAnimationState.Death)
		{
			playerSkill.oneShotResetTarget = false;
			useFreeSkill = false;
			useSkill = false;
			GameSetting.Instance.SetMouseCursor(0);
			castid = 0;
			skillRange = 0;
		}
		
		if(Physics.Raycast(r, out h ,100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem | 1 << layerActiveNpc)){
			
			if(h.collider != null)
			{
				if(h.collider.tag == "Ground" && !useFreeSkill){
					
					if(Input.GetMouseButtonDown(0) && useSkill && ctrlAnimState != ControlAnimationState.Death)
					{
						playerSkill.oneShotResetTarget = false;
						useSkill = false;
						GameSetting.Instance.SetMouseCursor(0);
						castid = 0;
						skillRange = 0;
					}
				}
				
				if(h.collider.tag == "Enemy"){
					
					if(Input.GetMouseButtonDown(0) && useSkill && ctrlAnimState != ControlAnimationState.Death)
					{
						GameSetting.Instance.SetMouseCursor(0);
					}
				}	
			}
		}
		
	}

	void HeroAnimationState() {
		
		if(ctrlAnimState == ControlAnimationState.Idle)
		{
			animationManager.animationState = animationManager.Idle;
		}
		
		if(ctrlAnimState == ControlAnimationState.Move)
		{
			animationManager.animationState = animationManager.Move;
		}
		
		if(ctrlAnimState == ControlAnimationState.WaitAttack)
		{
			animationManager.animationState = animationManager.Idle;
			WaitAttack();

		}
		if(ctrlAnimState == ControlAnimationState.Attack)
		{
			if(target)
			{
				LookAtTarget(target.transform.position);
			
				if(checkCritical)
				{
					animationManager.animationState = animationManager.CriticalAttack;
					delayAttack = 100;
					onceAttack = false;
				}else if(!checkCritical)
				{
					animationManager.animationState = animationManager.Attack;
					delayAttack = 100;
					onceAttack = false;
				}
			}else
			{
				ctrlAnimState = ControlAnimationState.Idle;	
			}
			
			
		}
			
		if(ctrlAnimState == ControlAnimationState.TakeAtk)
		{
			animationManager.animationState = animationManager.TakeAttack;

		}
		
		if(ctrlAnimState == ControlAnimationState.Cast)
		{
			playerSkill.CastSkill(playerSkill.FindSkillType(castid),playerSkill.FindSkillIndex(castid));
			
			animationManager.animationState = animationManager.Cast;
		}
		
		if(ctrlAnimState == ControlAnimationState.ActiveSkill)
		{
			animationManager.animationState = animationManager.ActiveSkill;
		}
		
		if(ctrlAnimState == ControlAnimationState.Death)
		{
			animationManager.animationState = animationManager.Death;
		}
	}

	void WaitAttack()
	{		
		if(delayAttack > 0)
		{
			delayAttack -= Time.deltaTime * playerStatus.statusCal.atkSpd;	
		}else if(delayAttack <= 0)
		{
			checkCritical = CriticalCal(playerStatus.statusCal.criticalRate);
				
			if(checkCritical)
			{
				typeAttack = Random.Range(0,animationManager.criticalAttack.Count);
				animationManager.checkAttack = false;
			}else if(!checkCritical)
			{
				typeAttack = Random.Range(0,animationManager.normalAttack.Count);
				animationManager.checkAttack = false;
			}
			
			if(autoAttack)
			{
				ctrlAnimState = ControlAnimationState.Attack;
			}else
			{
				if(onceAttack)
				{
					ctrlAnimState = ControlAnimationState.Attack;
				}
			}
				
		}
	}
	
	void TargetLock()
	{
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit h;
		if(target == null)
		{
			if(Physics.Raycast(r, out h ,100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem | 1 << layerActiveNpc)){
			
				if(h.collider != null)
				{
					if(h.collider.tag == "Enemy"){
						targetHP = h.collider.gameObject;
						
						if(!useSkill)
						GameSetting.Instance.SetMouseCursor(1);
									
					}else if(h.collider.tag == "Ground"){
						targetHP = null;
						
						if(!useSkill)
						GameSetting.Instance.SetMouseCursor(0);
				
					}else if(h.collider.tag == "Npc_Shop"){
						targetHP = null;
						
						if(!useSkill)
						GameSetting.Instance.SetMouseCursor(4);
				
					}else if(h.collider.tag == "Item"){
						targetHP = null;
						
						if(!useSkill)
						GameSetting.Instance.SetMouseCursor(5);
				
					}
				}
			}
			
		}else
		{
			
			if(Physics.Raycast(r, out h ,100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem | 1 << layerActiveNpc)){
			
				if(h.collider != null)
				{
					if(h.collider.tag == "Ground"){
						if(!useSkill)
						GameSetting.Instance.SetMouseCursor(0);
					}
					
					if(h.collider.tag == "Enemy"){
						if(!useSkill)
						GameSetting.Instance.SetMouseCursor(1);
					}
				}
			}
			if(Input.GetMouseButtonDown(0))
			{
				if(Physics.Raycast(r, out h ,100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem | 1 << layerActiveNpc)){
			
				if(h.collider != null)
				{
					if(h.collider.tag == "Enemy"){
						targetHP = h.collider.gameObject;
							
									
					}
						
					if(h.collider.tag == "Ground"){
						targetHP = null;	
									
					}
				}
			}
			}
			
		}
		if(targetHP)
		{
			EnemyStatus enemyStatus;
			EnemyController enemyControl;
			enemyStatus = targetHP.GetComponent<EnemyStatus>();
			enemyControl = targetHP.GetComponent<EnemyController>();
			
			EnemyHP.Instance.ShowHPbar(true);
			EnemyHP.Instance.GetHPTarget(enemyControl.defaultHP,enemyStatus.status.hp,enemyStatus.enemyName);
			
		}else if(!targetHP)
		{
			EnemyHP.Instance.ShowHPbar(false);
		}
	}

	void ClickToMove()
	{
		if(useFreeSkill && useSkill && getSkillTarget)
		{
			destinationDistance = Vector3.Distance(destinationPosition, this.transform.position); 
				
			if(destinationDistance < skillRange){	
				if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
				{
					ctrlAnimState = ControlAnimationState.Cast;
					playerSkill.canCast = true;
					getSkillTarget = false;
					}
					LookAtTarget(freePosSkill);
					moveSpeed = 0;
				}
				else if(destinationDistance > skillRange ){		
					if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
					ctrlAnimState = ControlAnimationState.Move;
					moveSpeed = playerStatus.statusCal.movespd;
				}
		}else
		{
			if(target != null && !useSkill)
			{
				destinationDistance = Vector3.Distance(target.transform.position, this.transform.position); 
				
				if(destinationDistance <= playerStatus.statusCal.atkRange){		
					if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
					ctrlAnimState = ControlAnimationState.WaitAttack;
					moveSpeed = 0;
					
					LookAtTarget(target.transform.position);
				}
				else if(destinationDistance > playerStatus.statusCal.atkRange ){			
					LookAtTarget(target.transform.position);
					if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle || ctrlAnimState == ControlAnimationState.WaitAttack)
					ctrlAnimState = ControlAnimationState.Move;
					moveSpeed = playerStatus.statusCal.movespd;
				}
				
				
			}else
				
			if(target != null && useSkill) 
			{
				destinationDistance = Vector3.Distance(target.transform.position, this.transform.position); 			
				if(destinationDistance <= skillRange){		
					if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
					{
						ctrlAnimState = ControlAnimationState.Cast;
						playerSkill.canCast = true;
					}
					LookAtTarget(target.transform.position);
					moveSpeed = 0;
				}
				else if(destinationDistance > skillRange ){			
					LookAtTarget(target.transform.position);
					if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle || ctrlAnimState == ControlAnimationState.WaitAttack)
					ctrlAnimState = ControlAnimationState.Move;
					moveSpeed = playerStatus.statusCal.movespd;
				}
				
				
			}else
				
			if(target == null && targetMoveTo != null)
			{	
				destinationDistance = Vector3.Distance(targetMoveTo.transform.position, this.transform.position); 
				
				if(destinationDistance <= 2f){
					if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
					ctrlAnimState = ControlAnimationState.Idle;
					moveSpeed = 0;
					InteractObject();	
				}
				else if(destinationDistance > 2f ){			
					LookAtTarget(targetMoveTo.transform.position);
					//if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
					ctrlAnimState = ControlAnimationState.Move;
					moveSpeed = playerStatus.statusCal.movespd;
				}
			}else
			
			if(target == null && targetMoveTo == null) 
			{
				destinationDistance = Vector3.Distance(destinationPosition, this.transform.position); 	
				if(destinationDistance < .5f){		
					if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
					ctrlAnimState = ControlAnimationState.Idle;
					moveSpeed = 0;
				}
				else if(destinationDistance > .5f ){	
					if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
					ctrlAnimState = ControlAnimationState.Move;
					moveSpeed = playerStatus.statusCal.movespd;
				}
			}
		}
		destinationDistance = Vector3.Distance(destinationPosition, this.transform.position);
		if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl==0 && dontClick == false) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitdist;
			if(!autoAttack)
			{
				onceAttack = true;
			}
			
			if(Physics.Raycast(ray, out hitdist,100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem | 1 << layerActiveNpc)) {
				
				if(hitdist.collider.tag != "Player")
				{
					Vector3 targetPoint = Vector3.zero;
					targetPoint.x = hitdist.point.x;
					targetPoint.y = transform.position.y;
					targetPoint.z = hitdist.point.z;
					destinationPosition = hitdist.point;
					targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
					
					if(alreadyLockSkill)
					{
						playerSkill.oneShotResetTarget = false;
						ResetOldCast();
						useFreeSkill = false;
						useSkill = false;
						getSkillTarget = false;
						alreadyLockSkill = false;
					}
					
					if(useFreeSkill && !alreadyLockSkill)
					{
						freePosSkill = destinationPosition;
						getSkillTarget = true;
						alreadyLockSkill = true;
					}
					
				}
			}
			
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit h;
			if(Physics.Raycast(r, out h ,100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem | 1 << layerActiveNpc)){
				if(h.collider.tag == "Ground"){
					if(ctrlAnimState != ControlAnimationState.Attack)
					{
						target = null;
						targetMoveTo = null;
					}
					Instantiate(GameSetting.Instance.mousefxNormal,new Vector3(h.point.x,h.point.y+0.02f,h.point.z),h.collider.transform.rotation);	
				}else if(h.collider.tag == "Enemy"){
					if(ctrlAnimState != ControlAnimationState.Attack)
					{
						target = h.collider.gameObject;
						targetMoveTo = null;
					}
					GameObject go = (GameObject)Instantiate(GameSetting.Instance.mousefxAttack,new Vector3(h.collider.transform.position.x,h.collider.transform.position.y+0.02f,h.collider.transform.position.z),Quaternion.identity);
					go.transform.parent = target.transform;
				}else if(h.collider.tag == "Npc" || h.collider.tag == "Item")
				{
					if(ctrlAnimState != ControlAnimationState.Attack)
					{
						target = null;
						targetMoveTo = h.collider.gameObject;
					}
					GameObject go = (GameObject)Instantiate(GameSetting.Instance.mousefxInteract,new Vector3(h.collider.transform.position.x,h.collider.transform.position.y+0.02f,h.collider.transform.position.z),Quaternion.identity);
					//go.transform.parent = targetMoveTo.transform;
				}
			}
	
		}

		else if (Input.GetMouseButton(0) && GUIUtility.hotControl==0 && dontClick == false) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitdist;
			if (Physics.Raycast(ray, out hitdist, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem | 1 << layerActiveNpc)) {
				if(hitdist.collider.tag != "Player")
				{
				Vector3 targetPoint = Vector3.zero;//hitdist.point;
				targetPoint.x = hitdist.point.x;
				targetPoint.y = transform.position.y;
				targetPoint.z = hitdist.point.z;
				destinationPosition = hitdist.point;
				targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
				}	
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
			if(ctrlAnimState != ControlAnimationState.Attack)
			ctrlAnimState = ControlAnimationState.Idle;
			moveSpeed = 0;	
		}
		
		if(Input.GetMouseButton(0) && target && dontClick == false)
		{
			if(!autoAttack)
			{
				onceAttack = true;
			}
		}
		
		if(ctrlAnimState == ControlAnimationState.Move){
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation,targetRotation,Time.deltaTime *25);
			if(controller.isGrounded){
				movedir = Vector3.zero;
				movedir = transform.TransformDirection(Vector3.forward*moveSpeed);
			}
		}else
		{
			movedir = Vector3.Lerp(movedir,Vector3.zero,Time.deltaTime * 10);	
		
		}
		movedir.y -= 20 * Time.deltaTime;
		controller.Move(movedir * Time.deltaTime);	
	}	
	
	void InteractObject()
	{
		if(targetMoveTo.tag == "Npc")
		{
			if(targetMoveTo.GetComponent<NpcSetup>().npcType == NpcSetup.NpcType.QuestNpc)
				targetMoveTo.GetComponent<NpcSetup>().SetupDialogQuest(targetMoveTo.GetComponent<NpcSetup>().questID);
				
			targetMoveTo.GetComponent<NpcSetup>().CallDialogBox();

		}else if(targetMoveTo.tag == "Item")
		{
			guiMenu.PickupItem(targetMoveTo);
			
		}
		ResetState();
		targetMoveTo = null;
		
	}

	void LookAtTarget(Vector3 _targetPos)
	{
		targetPos.x = _targetPos.x;
		targetPos.y = this.transform.position.y;
		targetPos.z = _targetPos.z;
		this.transform.LookAt(targetPos);
	}

	bool CriticalCal(float criticalStat)
	{
		float calCritical = criticalStat - Random.Range(0,101f);
		
		if(calCritical > 0)
		{
			return true;
		}else
		{
			return false; 
		}
	}
	public void ResetState()
	{
		moveSpeed = 0;
		movedir = Vector3.zero;	
		destinationDistance = 0;
		destinationPosition = this.transform.position;
		target= null;
		ctrlAnimState = ControlAnimationState.Idle;
		alreadyLockSkill = false;
		Invoke("resetCheckAttack",0.1f);
	}
	
	public void ResetBeforeCast()
	{
		moveSpeed = 0;
		movedir = Vector3.zero;	
		destinationDistance = 0;
		destinationPosition = this.transform.position;
		target= null;
		alreadyLockSkill = false;
		Invoke("resetCheckAttack",0.1f);
	}
	
	void ResetMove()
	{
		moveSpeed = 0;
		movedir = Vector3.zero;	
		destinationDistance = 0;
		destinationPosition = this.transform.position;
	}
	
	public void ResetAttack()
	{
		target = null;	
	}
	
	public void DeadReset()
	{
		useSkill = false;
		GameSetting.Instance.SetMouseCursor(0);
		castid = 0;
		skillRange = 0;
		moveSpeed = 0;
		movedir = Vector3.zero;	
		destinationDistance = 0;
		destinationPosition = this.transform.position;
		target= null;
		if(!oneShotOpenDeadWindow)
		{
			Invoke("OpenDeadWindow",0.5f);
			oneShotOpenDeadWindow = true;	
		}	
	}
	
	void OpenDeadWindow()
	{
		if(!DeadWindow.enableWindow)
		DeadWindow.enableWindow = true;
	}
	
	void resetCheckAttack()
	{
		animationManager.checkAttack = false;	
	}
	
	public void GetDamage(float targetAttack,float targetHit,float flinchRate,GameObject atkEffect,AudioClip atksfx)
	{
		targetHit += Random.Range(-10,30);
		
		if(playerStatus.statusCal.spd - targetHit > 0) 
		{
			InitTextDamage(Color.white,"Miss");
			SoundManager.instance.PlayingSound("Attack_Miss");
			
		}else
		{
			int damage = Mathf.FloorToInt((targetAttack - playerStatus.statusCal.def) * Random.Range(0.8f,1.2f));
			
			if(damage <= 5)
			{
				damage = Random.Range(1,11); 
			}
			if(atksfx)
			AudioSource.PlayClipAtPoint(atksfx,transform.position);
			if(atkEffect)
			Instantiate(atkEffect,transform.position,Quaternion.identity);
			InitTextDamage(Color.red,damage.ToString());
			playerStatus.statusCal.hp -= damage;
			GetDamageColorReset();
			if(playerStatus.statusCal.hp <= 0)
			{	
				playerSkill.CastBreak();
				playerStatus.statusCal.hp = 0;
				ctrlAnimState = ControlAnimationState.Death;
			}else
			{
				flinchValue -= flinchRate;
				
				if(flinchValue <= 0)
				{
					if(ctrlAnimState == ControlAnimationState.Cast || ctrlAnimState == ControlAnimationState.ActiveSkill)
						playerSkill.CastBreak();
						
					ctrlAnimState = ControlAnimationState.TakeAtk;
					flinchValue = 100;
					playerSkill.oneShotResetTarget = false;
				}
				
			}
		}
	}
	
	public void InitTextDamage(Color colorText,string damageGet){
		GameObject loadPref = (GameObject)Resources.Load("TextDamage");
		GameObject go = (GameObject)Instantiate(loadPref, transform.position  + (Vector3.up*1.0f), Quaternion.identity);
		go.GetComponentInChildren<TextDamage>().SetDamage(damageGet, colorText);
	}
	
	void GetDamageColorReset()
	{
		int index = 0;
		while(index < modelMesh.Count){
			modelMesh[index].GetComponent<Renderer>().material.color = defaultColor[index];
			index++;
		}
		
		StartCoroutine(GetDamageColor(0.2f));
	}
	
	void SetDefualtColor()
	{
		int index = 0;
		while(index < modelMesh.Count){
			defaultColor[index] = modelMesh[index].GetComponent<Renderer>().material.color;
			index++;
		}
	}
	
	private IEnumerator GetDamageColor(float time){
		//if take damage material monster will change to setting color
		int index = 0;
		Color[] colorDef = new Color[modelMesh.Count];
		while(index < modelMesh.Count){
			colorDef[index] = modelMesh[index].GetComponent<Renderer>().material.color;
			modelMesh[index].GetComponent<Renderer>().material.color = colorTakeDamage;
			index++;
		}
		yield return new WaitForSeconds(time);
		index = 0;
		while(index < modelMesh.Count){
			modelMesh[index].GetComponent<Renderer>().material.color = colorDef[index];
			index++;
		}
		yield return 0;
		StopCoroutine("GetDamageColor");
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{	
		if(hit.gameObject.tag == "Collider")
		{
			ResetMove();	
		}
	}	
	
	public void GetCastID(int caseID){
		
		if(ctrlAnimState != ControlAnimationState.Cast && ctrlAnimState != ControlAnimationState.ActiveSkill && ctrlAnimState != ControlAnimationState.Death && 
			ctrlAnimState != ControlAnimationState.Attack)
		{
			castid = caseID;
			ctrlAnimState = ControlAnimationState.Cast;
		}
		
	}
	
	public void ResetOldCast()
	{
		useSkill = false;
		useFreeSkill = false;
		GameSetting.Instance.SetMouseCursor(0);
	}
	
	public void Reborn()
	{
		ctrlAnimState = ControlAnimationState.Idle;
		playerStatus.statusCal.hp = playerStatus.hpMax/2;
		playerStatus.statusCal.mp = playerStatus.mpMax/2;
		playerStatus.status.exp -= (playerStatus.status.exp / GameSetting.Instance.deadExpPenalty); 
		if(playerStatus.status.exp < 0)
		{
			playerStatus.status.exp = 0;	
		}

		playerStatus.StartRegen();
		transform.position = DeadSpawnPoint.transform.position;
		moveSpeed = 0;
		movedir = Vector3.zero;	
		destinationDistance = 0;
		destinationPosition = this.transform.position;
		target= null;
		alreadyLockSkill = false;
		Invoke("resetCheckAttack",0.1f);
		animationManager.oneCheckDeadReset = false;
		oneShotOpenDeadWindow = false;
		DeadWindow.enableWindow = false;
	}
}
