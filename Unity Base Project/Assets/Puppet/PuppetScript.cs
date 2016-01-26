using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuppetScript : MonoBehaviour
{
	// modifiers to animation speeds
	public struct AttackAnimationMods
	{
		public float windup;
		public float swing;
		public float recover;
		public AttackAnimationMods(float _init)
		{
			windup = swing = recover = _init;
		}
	}

	public GameObject painEffect;
	public GameObject bloodHit;
	public GameObject bloodPool;
	public GameObject swordSwish;

	// New things, added by Dakota 1/13 7:22pm
	public GameObject degubber;
	public float curTallys = 3;
	public float maxTallys = 3;
    public float currZen = 10;
    public float maxZen = 10;

	public enum State
	{
		IDLE = 0, MOVING, FLINCH, STUN, DEAD,
		ATK_VERT, ATK_LTR, ATK_RTL, ATK_STAB, ATK_KICK,
		PARRY,
		GRD_TOP, GRD_LEFT, GRD_RIGHT,
		DGE_FORWARD, DGE_LEFT, DGE_RIGHT, DGE_BACK, DANCE, NUMSTATES
	}

	public enum Dir
	{
		FORWARD = 0, RIGHT, BACKWARD, LEFT
	}
	// Easy indexing into AttackAnimationMods array!
	public enum AttackModType
	{
		VERT = 0, LTR, RTL,
		KICK, STAB, NUM_MODTYPES
	}
	public State lastState;
	public State curState;
	public PuppetAttackScript attackScript;
	public PuppetGuardScript guardScript;
	public PuppetDodgeScript dodgeScript;
	public PuppetCameraScript camScript;
	public PuppetResolveHitScript rhScript;
	public CharacterController controller;
	public GameObject curTarg;
	public GameObject Targeting_Cube;
	public Armor armor;
	private GameObject Targeting_CubeSpawned = null;
	public List<GameObject> badguys;
	public Vector3 targOffset;
	public Vector3 nextDir;
	public AttackAnimationMods[] AnimMods;
	public PlayerInput_Alt Input_AltScript;
	public PlayerInput InputScript;
	public State NextAttack; // for combos!
	//public float AtkTmrMax;
	public float DgeTmrMax;
	public float GrdTmrMax;
	public float targMaxDist;
	public float def_moveSpeed;
	public float moveSpeed;
	public float lockMoveSpeedMod;
	public float def_camSpeed;
	public float camSpeed;
	public bool rockedOn = false;
	public bool godMode = false;
	public MaterialFlash flashScript = null; // Josh: talk to the renderer
	public string[,] animTable;

	private bool[,] stateTable;
	private Quaternion orgRot;
	private Vector3 moveTest;
	private Vector3 camTest;
	private Vector3 oldPos;
	#region old degubstuffs
	//private float debugAngle;
	//private int debugDodgeType = 0;
	//private float debugDodgeTmr = 0.0f;
	//private int debugGrdType = 0;
	//private float debugGrdTmr = 0.0f;
	//private bool debugMove = false;
	//private bool debugCamera = false;
	//private bool debugDodge = false;
	//private bool debugGuard = false;
	#endregion



	// Use this for initialization
	void Start()
	{
		animation["New Down Slash"].time = 0.36667f;
		animation["New Left Slash"].time = 0.6f;
		animation["New Right Slash"].time = 0.63333f;
		// New things, added by Dakota 1/13 whatever PM
		// Needed a reference to the player in the meat script to decrement balance
		BloodyBag[] meats = gameObject.GetComponentsInChildren<BloodyBag>();

		for (int i = 0; i < meats.Length; i++)
			meats[i].player = gameObject;


		Object temp = GetComponent<PuppetAttackScript>();
		if (attackScript == null)
		{
			attackScript = (PuppetAttackScript)temp;
		}

		if (guardScript == null)
		{
			temp = GetComponent<PuppetGuardScript>();
			guardScript = (PuppetGuardScript)temp;
		}

		if (dodgeScript == null)
		{
			temp = GetComponent<PuppetDodgeScript>();
			dodgeScript = (PuppetDodgeScript)temp;
		}

		if (camScript == null)
		{
			temp = GetComponent<PuppetCameraScript>();
			camScript = (PuppetCameraScript)temp;
		}
		if (rhScript == null)
		{
			temp = GetComponent<PuppetResolveHitScript>();
			rhScript = (PuppetResolveHitScript)temp;
		}
		if (controller == null)
		{
			temp = GetComponent<CharacterController>();
			controller = (CharacterController)temp;
		}
		if (flashScript == null)
		{
			temp = GetComponentInChildren<MaterialFlash>();
			flashScript = (MaterialFlash)temp;
		}
		//badguys = GameObject.FindGameObjectsWithTag("Enemy");

		lastState = curState = State.IDLE;
		if (targOffset == Vector3.zero)
			targOffset.y = 3.0f;
		if (targMaxDist == 0.0f)
			targMaxDist = 200.0f;
		if (moveSpeed == 0.0f)
			moveSpeed = 5.0f;
		def_moveSpeed = moveSpeed;
		if (lockMoveSpeedMod == 0.0f)
			lockMoveSpeedMod = 0.3f;
		if (camSpeed == 0.0f)
			camSpeed = 8.0f;
		def_camSpeed = camSpeed;
		//if (AtkTmrMax = 0.0f)
		//AtkTmrMax = 1.0f;
		if (AnimMods == null)
		{
			if (tag == "Player")
			{
				InitAnimTable();
				InitAnimMods();
			}
			else if (tag == "Enemy")
			{
				InitAnimTableEnemy();
				InitAnimModsEnemy();
			}
		}
		NextAttack = State.IDLE; // cant set to null =(

		if (Input_AltScript == null)
			Input_AltScript = GetComponent<PlayerInput_Alt>();
		if (InputScript == null)
			InputScript = GetComponent<PlayerInput>();

		if (DgeTmrMax == 0.0f)
			DgeTmrMax = 0.5f;
		if (GrdTmrMax == 0.0f)
			GrdTmrMax = 0.2f;

		InitStateTable();

		moveTest = Vector3.zero;
		moveTest.x = 1.0f;
		moveTest.z = 1.0f;
		camTest = Vector3.zero;
		camTest.x = 1.0f;
		camTest.z = 1.0f;

		//debugAngle = 0.0f;

		if (tag == "Player")
			camScript.Initialize(this);
		attackScript.Initialize(this);
		dodgeScript.Initialize(this);
		guardScript.Initialize(this);
		rhScript.Initialize(this);
	}

	void InitAnimMods()
	{
		AnimMods = new AttackAnimationMods[(int)AttackModType.NUM_MODTYPES]{
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f},
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f},
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f},
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f},
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f}
		};

			AnimMods[(int)AttackModType.LTR].windup = 2.0f;
			AnimMods[(int)AttackModType.LTR].swing = 2.5f;
			AnimMods[(int)AttackModType.LTR].recover = 2.0f;
			AnimMods[(int)AttackModType.RTL].windup = 2.0f;
			AnimMods[(int)AttackModType.RTL].swing = 2.0f;
			AnimMods[(int)AttackModType.RTL].recover = 3.0f;
			AnimMods[(int)AttackModType.VERT].windup = 2.0f;
			AnimMods[(int)AttackModType.VERT].swing = 2.8f;
			AnimMods[(int)AttackModType.VERT].recover = 3.0f;
			AnimMods[(int)AttackModType.KICK].windup = 2.0f;
			//AnimMods[(int)AttackModType.KICK].swing = 0.1f;
			AnimMods[(int)AttackModType.KICK].recover = 2.0f;
			AnimMods[(int)AttackModType.STAB].windup = 1.0f;
			AnimMods[(int)AttackModType.STAB].swing = 3.0f;
			AnimMods[(int)AttackModType.STAB].recover = 2.0f;
	}

	void InitAnimModsEnemy()
	{
		AnimMods = new AttackAnimationMods[(int)AttackModType.NUM_MODTYPES]{
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f},
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f},
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f},
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f},
			new AttackAnimationMods{windup = 1.0f, swing = 1.0f, recover = 1.0f}
		};

		//AnimMods[(int)AttackModType.LTR].windup = 1.0f;
		//AnimMods[(int)AttackModType.LTR].swing = 0.2f;
		//AnimMods[(int)AttackModType.LTR].recover = 1.0f;
		//AnimMods[(int)AttackModType.RTL].windup = 1.0f;
		//AnimMods[(int)AttackModType.RTL].swing = 0.2f;
		//AnimMods[(int)AttackModType.RTL].recover = 1.0f;

	}

	void InitAnimTable()
	{
		animTable = new string[(int)State.NUMSTATES, (int)State.NUMSTATES];
		animTable[(int)State.ATK_VERT, (int)State.GRD_TOP] =
		animTable[(int)State.ATK_RTL, (int)State.GRD_LEFT] =
		animTable[(int)State.ATK_LTR, (int)State.GRD_RIGHT] =
		"Idle";
		animTable[(int)State.GRD_TOP, (int)State.ATK_VERT] =
		animTable[(int)State.GRD_LEFT, (int)State.ATK_RTL] =
		animTable[(int)State.GRD_RIGHT, (int)State.ATK_LTR] =
		"Block Up Hit";
		animTable[(int)State.GRD_TOP, (int)State.ATK_RTL] =
		animTable[(int)State.GRD_TOP, (int)State.ATK_LTR] =
		animTable[(int)State.GRD_TOP, (int)State.ATK_STAB] =
		animTable[(int)State.IDLE, (int)State.ATK_VERT] =
		animTable[(int)State.MOVING, (int)State.ATK_VERT] =
		animTable[(int)State.ATK_VERT, (int)State.ATK_VERT] =
		animTable[(int)State.ATK_RTL, (int)State.ATK_VERT] =
		animTable[(int)State.ATK_LTR, (int)State.ATK_VERT] =
		animTable[(int)State.IDLE, (int)State.ATK_STAB] =
		animTable[(int)State.MOVING, (int)State.ATK_STAB] =
		animTable[(int)State.ATK_VERT, (int)State.ATK_STAB] =
		animTable[(int)State.ATK_RTL, (int)State.ATK_STAB] =
		animTable[(int)State.ATK_LTR, (int)State.ATK_STAB] =
		"React Front";
		animTable[(int)State.GRD_LEFT, (int)State.ATK_VERT] =
		animTable[(int)State.GRD_LEFT, (int)State.ATK_LTR] =
		animTable[(int)State.GRD_RIGHT, (int)State.ATK_VERT] =
		animTable[(int)State.GRD_RIGHT, (int)State.ATK_RTL] =
		animTable[(int)State.IDLE, (int)State.ATK_RTL] =
		animTable[(int)State.MOVING, (int)State.ATK_RTL] =
		animTable[(int)State.IDLE, (int)State.ATK_LTR] =
		animTable[(int)State.MOVING, (int)State.ATK_LTR] =
		animTable[(int)State.ATK_VERT, (int)State.ATK_RTL] =
		animTable[(int)State.ATK_VERT, (int)State.ATK_LTR] =
		animTable[(int)State.ATK_RTL, (int)State.ATK_RTL] =
		animTable[(int)State.ATK_RTL, (int)State.ATK_LTR] =
		animTable[(int)State.ATK_LTR, (int)State.ATK_RTL] =
		animTable[(int)State.ATK_LTR, (int)State.ATK_LTR] =
		"React Side";

		#region Very Unimportant Things
		animTable[(int)State.IDLE, (int)State.IDLE] =
		animTable[(int)State.MOVING, (int)State.MOVING] =
		"Twerk";
		#endregion

	}
	void InitAnimTableEnemy()
	{
		animTable = new string[(int)State.NUMSTATES, (int)State.NUMSTATES];
		//animTable[(int)State.ATK_VERT, (int)State.GRD_TOP] =
		//animTable[(int)State.ATK_RTL, (int)State.GRD_LEFT] =
		//animTable[(int)State.ATK_LTR, (int)State.GRD_RIGHT] =
		//"Idle";
		animTable[(int)State.GRD_TOP, (int)State.ATK_VERT] =
		animTable[(int)State.GRD_LEFT, (int)State.ATK_RTL] =
		animTable[(int)State.GRD_RIGHT, (int)State.ATK_LTR] =
		"Block Up Hit";
		animTable[(int)State.GRD_LEFT, (int)State.ATK_VERT] =
		animTable[(int)State.GRD_RIGHT, (int)State.ATK_VERT] =
		animTable[(int)State.GRD_TOP, (int)State.ATK_STAB] =
		animTable[(int)State.IDLE, (int)State.ATK_VERT] =
		animTable[(int)State.MOVING, (int)State.ATK_VERT] =
			//animTableEnemy[(int)State.ATK_VERT, (int)State.ATK_VERT] =
			//animTableEnemy[(int)State.ATK_RTL, (int)State.ATK_VERT] =
			//animTableEnemy[(int)State.ATK_LTR, (int)State.ATK_VERT] =
		animTable[(int)State.IDLE, (int)State.ATK_STAB] =
		animTable[(int)State.MOVING, (int)State.ATK_STAB] =
		animTable[(int)State.ATK_VERT, (int)State.ATK_STAB] =
		animTable[(int)State.ATK_RTL, (int)State.ATK_STAB] =
		animTable[(int)State.ATK_LTR, (int)State.ATK_STAB] =
		"React Front";
		animTable[(int)State.GRD_TOP, (int)State.ATK_RTL] =
		animTable[(int)State.GRD_TOP, (int)State.ATK_LTR] =
		animTable[(int)State.GRD_LEFT, (int)State.ATK_LTR] =
		animTable[(int)State.GRD_RIGHT, (int)State.ATK_RTL] =
		animTable[(int)State.IDLE, (int)State.ATK_RTL] =
		animTable[(int)State.MOVING, (int)State.ATK_RTL] =
		animTable[(int)State.IDLE, (int)State.ATK_LTR] =
		animTable[(int)State.MOVING, (int)State.ATK_LTR] =
			//animTable[(int)State.ATK_VERT, (int)State.ATK_RTL] =
			//animTable[(int)State.ATK_VERT, (int)State.ATK_LTR] =
			//animTable[(int)State.ATK_RTL, (int)State.ATK_RTL] =
			//animTable[(int)State.ATK_RTL, (int)State.ATK_LTR] =
			//animTable[(int)State.ATK_LTR, (int)State.ATK_RTL] =
			//animTable[(int)State.ATK_LTR, (int)State.ATK_LTR] =
		"React Side";

		#region Very Unimportant Things
		animTable[(int)State.IDLE, (int)State.IDLE] =
		animTable[(int)State.MOVING, (int)State.MOVING] =
		"Twerk";
		#endregion

	}
	void InitStateTable()
	{
		stateTable = new bool[(int)State.NUMSTATES, (int)State.NUMSTATES];

		// move into idle from almost any other state
		stateTable[(int)State.MOVING, (int)State.IDLE] =
		stateTable[(int)State.FLINCH, (int)State.IDLE] =
		stateTable[(int)State.STUN, (int)State.IDLE] =
		stateTable[(int)State.ATK_VERT, (int)State.IDLE] =
		stateTable[(int)State.ATK_LTR, (int)State.IDLE] =
		stateTable[(int)State.ATK_RTL, (int)State.IDLE] =
		stateTable[(int)State.ATK_STAB, (int)State.IDLE] =
		stateTable[(int)State.ATK_KICK, (int)State.IDLE] =
		stateTable[(int)State.PARRY, (int)State.IDLE] =
		stateTable[(int)State.GRD_TOP, (int)State.IDLE] =
		stateTable[(int)State.GRD_LEFT, (int)State.IDLE] =
		stateTable[(int)State.GRD_RIGHT, (int)State.IDLE] =
		stateTable[(int)State.DGE_FORWARD, (int)State.IDLE] =
		stateTable[(int)State.DGE_LEFT, (int)State.IDLE] =
		stateTable[(int)State.DGE_RIGHT, (int)State.IDLE] =
		stateTable[(int)State.DGE_BACK, (int)State.IDLE] =
			//stateTable[(int)State.DANCE, (int)State.IDLE] =
			true;
		// a call to go into moving will only succeed under rare circumstances
		stateTable[(int)State.IDLE, (int)State.MOVING] =
		stateTable[(int)State.MOVING, (int)State.MOVING] =
		stateTable[(int)State.DANCE, (int)State.MOVING] =
			true;
		// flinches and stuns can happen at any time
		stateTable[(int)State.IDLE, (int)State.FLINCH] =
		stateTable[(int)State.MOVING, (int)State.FLINCH] =
		stateTable[(int)State.FLINCH, (int)State.FLINCH] =
		stateTable[(int)State.STUN, (int)State.FLINCH] =
		stateTable[(int)State.DEAD, (int)State.FLINCH] =
		stateTable[(int)State.ATK_VERT, (int)State.FLINCH] =
		stateTable[(int)State.ATK_LTR, (int)State.FLINCH] =
		stateTable[(int)State.ATK_RTL, (int)State.FLINCH] =
		stateTable[(int)State.ATK_STAB, (int)State.FLINCH] =
		stateTable[(int)State.ATK_KICK, (int)State.FLINCH] =
		stateTable[(int)State.PARRY, (int)State.FLINCH] =
		stateTable[(int)State.GRD_TOP, (int)State.FLINCH] =
		stateTable[(int)State.GRD_LEFT, (int)State.FLINCH] =
		stateTable[(int)State.GRD_RIGHT, (int)State.FLINCH] =
		stateTable[(int)State.DGE_FORWARD, (int)State.FLINCH] =
		stateTable[(int)State.DGE_LEFT, (int)State.FLINCH] =
		stateTable[(int)State.DGE_RIGHT, (int)State.FLINCH] =
		stateTable[(int)State.DGE_BACK, (int)State.FLINCH] =
		stateTable[(int)State.DANCE, (int)State.FLINCH] =
			true;
		stateTable[(int)State.IDLE, (int)State.STUN] =
		stateTable[(int)State.MOVING, (int)State.STUN] =
		stateTable[(int)State.FLINCH, (int)State.STUN] =
		stateTable[(int)State.STUN, (int)State.STUN] =
		stateTable[(int)State.DEAD, (int)State.STUN] =
		stateTable[(int)State.ATK_VERT, (int)State.STUN] =
		stateTable[(int)State.ATK_LTR, (int)State.STUN] =
		stateTable[(int)State.ATK_RTL, (int)State.STUN] =
		stateTable[(int)State.ATK_STAB, (int)State.STUN] =
		stateTable[(int)State.ATK_KICK, (int)State.STUN] =
		stateTable[(int)State.PARRY, (int)State.STUN] =
		stateTable[(int)State.GRD_TOP, (int)State.STUN] =
		stateTable[(int)State.GRD_LEFT, (int)State.STUN] =
		stateTable[(int)State.GRD_RIGHT, (int)State.STUN] =
		stateTable[(int)State.DGE_FORWARD, (int)State.STUN] =
		stateTable[(int)State.DGE_LEFT, (int)State.STUN] =
		stateTable[(int)State.DGE_RIGHT, (int)State.STUN] =
		stateTable[(int)State.DGE_BACK, (int)State.STUN] =
		stateTable[(int)State.DANCE, (int)State.STUN] =
			true;

		// die from any state!
		stateTable[(int)State.IDLE, (int)State.DEAD] =
		stateTable[(int)State.MOVING, (int)State.DEAD] =
		stateTable[(int)State.FLINCH, (int)State.DEAD] =
		stateTable[(int)State.STUN, (int)State.DEAD] =
		stateTable[(int)State.DEAD, (int)State.DEAD] =
		stateTable[(int)State.ATK_VERT, (int)State.DEAD] =
		stateTable[(int)State.ATK_LTR, (int)State.DEAD] =
		stateTable[(int)State.ATK_RTL, (int)State.DEAD] =
		stateTable[(int)State.ATK_STAB, (int)State.DEAD] =
		stateTable[(int)State.ATK_KICK, (int)State.DEAD] =
		stateTable[(int)State.PARRY, (int)State.DEAD] =
		stateTable[(int)State.GRD_TOP, (int)State.DEAD] =
		stateTable[(int)State.GRD_LEFT, (int)State.DEAD] =
		stateTable[(int)State.GRD_RIGHT, (int)State.DEAD] =
		stateTable[(int)State.DGE_FORWARD, (int)State.DEAD] =
		stateTable[(int)State.DGE_LEFT, (int)State.DEAD] =
		stateTable[(int)State.DGE_RIGHT, (int)State.DEAD] =
		stateTable[(int)State.DGE_BACK, (int)State.DEAD] =
		stateTable[(int)State.DANCE, (int)State.DEAD] =
			true;
		// can only attack from a few states
		stateTable[(int)State.IDLE, (int)State.ATK_VERT] =
		stateTable[(int)State.MOVING, (int)State.ATK_VERT] =
		stateTable[(int)State.DANCE, (int)State.ATK_VERT] =
			true;
		stateTable[(int)State.IDLE, (int)State.ATK_LTR] =
		stateTable[(int)State.MOVING, (int)State.ATK_LTR] =
		stateTable[(int)State.DANCE, (int)State.ATK_LTR] =
			true;
		stateTable[(int)State.IDLE, (int)State.ATK_RTL] =
		stateTable[(int)State.MOVING, (int)State.ATK_RTL] =
		stateTable[(int)State.DANCE, (int)State.ATK_RTL] =
			true;
		stateTable[(int)State.IDLE, (int)State.ATK_STAB] =
		stateTable[(int)State.MOVING, (int)State.ATK_STAB] =
		stateTable[(int)State.DANCE, (int)State.ATK_STAB] =
			true;
		stateTable[(int)State.IDLE, (int)State.ATK_KICK] =
		stateTable[(int)State.MOVING, (int)State.ATK_KICK] =
		stateTable[(int)State.DANCE, (int)State.ATK_KICK] =
			true;

		// can only parry from a few states
		stateTable[(int)State.IDLE, (int)State.PARRY] =
		stateTable[(int)State.MOVING, (int)State.PARRY] =
		stateTable[(int)State.DANCE, (int)State.PARRY] =
			true;

		// can guard from other guards and parries as well as necessary base states
		stateTable[(int)State.IDLE, (int)State.GRD_TOP] =
		stateTable[(int)State.MOVING, (int)State.GRD_TOP] =
			//stateTable[(int)State.PARRY, (int)State.GRD_TOP] =
		stateTable[(int)State.GRD_TOP, (int)State.GRD_TOP] =
		stateTable[(int)State.GRD_LEFT, (int)State.GRD_TOP] =
		stateTable[(int)State.GRD_RIGHT, (int)State.GRD_TOP] =
		stateTable[(int)State.DANCE, (int)State.GRD_TOP] =
			true;
		stateTable[(int)State.IDLE, (int)State.GRD_LEFT] =
		stateTable[(int)State.MOVING, (int)State.GRD_LEFT] =
			//stateTable[(int)State.PARRY, (int)State.GRD_LEFT] =
		stateTable[(int)State.GRD_TOP, (int)State.GRD_LEFT] =
		stateTable[(int)State.GRD_LEFT, (int)State.GRD_LEFT] =
		stateTable[(int)State.GRD_RIGHT, (int)State.GRD_LEFT] =
		stateTable[(int)State.DANCE, (int)State.GRD_LEFT] =
			true;
		stateTable[(int)State.IDLE, (int)State.GRD_RIGHT] =
		stateTable[(int)State.MOVING, (int)State.GRD_RIGHT] =
			//stateTable[(int)State.PARRY, (int)State.GRD_RIGHT] =
		stateTable[(int)State.GRD_TOP, (int)State.GRD_RIGHT] =
		stateTable[(int)State.GRD_LEFT, (int)State.GRD_RIGHT] =
		stateTable[(int)State.GRD_RIGHT, (int)State.GRD_RIGHT] =
		stateTable[(int)State.DANCE, (int)State.GRD_RIGHT] =
			true;

		stateTable[(int)State.IDLE, (int)State.DGE_FORWARD] =
		stateTable[(int)State.MOVING, (int)State.DGE_FORWARD] =
		stateTable[(int)State.DANCE, (int)State.DGE_FORWARD] =
			true;
		stateTable[(int)State.IDLE, (int)State.DGE_LEFT] =
		stateTable[(int)State.MOVING, (int)State.DGE_LEFT] =
		stateTable[(int)State.DANCE, (int)State.DGE_LEFT] =
			true;
		stateTable[(int)State.IDLE, (int)State.DGE_RIGHT] =
		stateTable[(int)State.MOVING, (int)State.DGE_RIGHT] =
		stateTable[(int)State.DANCE, (int)State.DGE_RIGHT] =
			true;
		stateTable[(int)State.IDLE, (int)State.DGE_BACK] =
		stateTable[(int)State.MOVING, (int)State.DGE_BACK] =
		stateTable[(int)State.DANCE, (int)State.DGE_BACK] =
			true;

		// dance muthafucka
		stateTable[(int)State.IDLE, (int)State.DANCE] =
			true;

	}

	// Update is called once per frame
	void Update()
	{
		if (curState == State.FLINCH && animation.isPlaying == false)
		{
			ChangeState(State.IDLE);
		}

		// only search for targets if we are the player.
		if (tag == "Player" && curState != State.DEAD)
		{
			FindTarg();
			if (rockedOn && curTarg != null)
			{
				Vector3 target = curTarg.transform.position;
				target.y = transform.position.y;
				transform.LookAt(target);
			}
		}

	}

	// FindTarg()
	// find the most relevant enemy and assign him as our current target.
	private void FindTarg()
	{
		badguys.Clear();
		GameObject[] tempbadguys = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject tempguy in tempbadguys)
		{
			if (tempguy.GetComponent<PuppetScript>().curState != State.DEAD)
				badguys.Add(tempguy);
			else if (tempguy == curTarg)
			{
				if (Targeting_CubeSpawned != null)
				{
					Destroy(Targeting_CubeSpawned.gameObject);
					DestroyImmediate(Targeting_CubeSpawned.gameObject);
					Destroy(Targeting_CubeSpawned.gameObject);
					DestroyImmediate(Targeting_CubeSpawned.gameObject);
					Destroy(Targeting_CubeSpawned.gameObject);
					DestroyImmediate(Targeting_CubeSpawned.gameObject);
					Destroy(Targeting_CubeSpawned.gameObject);
					DestroyImmediate(Targeting_CubeSpawned.gameObject);
					Targeting_CubeSpawned = null;
				}
				if (rockedOn)
					ToggleLockon();
			}
		}
		//curTarg = null;
		if (badguys.Count > 0)
		{
			float dist;
			float curDist = dist = 0x0FFFFFFF;
			foreach (GameObject badguy in badguys)
			{
				curDist = Vector3.SqrMagnitude(badguy.transform.position - transform.position);
				if (curDist < dist && !rockedOn)
				{
					//if (!rockedOn)
					curTarg = badguy;
					dist = curDist;
				}
			}
			// only have a target if within distance
			if (curTarg != null)
			{
				if (dist < targMaxDist)
				{
					if (Targeting_CubeSpawned == null)
					{
						Targeting_CubeSpawned = (GameObject)Instantiate(Targeting_Cube, curTarg.transform.position, Quaternion.identity);
					}
					Targeting_CubeSpawned.transform.position = curTarg.transform.position + targOffset;
					Targeting_CubeSpawned.transform.SetParent(curTarg.transform);
				}
				else if (!rockedOn)
				{
					if (Targeting_CubeSpawned != null)
					{
						Destroy(Targeting_CubeSpawned.gameObject);
						Destroy(Targeting_CubeSpawned.gameObject); // apparently the solution.
						Destroy(Targeting_CubeSpawned.gameObject);
						Destroy(Targeting_CubeSpawned.gameObject);
						Destroy(Targeting_CubeSpawned.gameObject);
						Destroy(Targeting_CubeSpawned.gameObject);
						Destroy(Targeting_CubeSpawned.gameObject);
						Destroy(Targeting_CubeSpawned.gameObject);
						Targeting_CubeSpawned = null;
					}
					if (rockedOn)
						ToggleLockon();
				}
			}
		}
		else // no badguys that are alive
		{
			if (Targeting_CubeSpawned != null)
			{
				Destroy(Targeting_CubeSpawned);
				Destroy(Targeting_CubeSpawned.gameObject); // apparently the solution.
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Targeting_CubeSpawned = null;
			}
			if (rockedOn)
				ToggleLockon();
		}
	}

	#region old DoDegub() stuffs
	// DoDegub()
	// Does the degubs for the testing on the features
	//private void DoDegub()
	//{
	//	if (debugMove || debugCamera)
	//	{
	//		debugAngle += Time.deltaTime * 2.0f;
	//		if (debugAngle >= 2.0f * Mathf.PI)
	//			debugAngle -= 2.0f * Mathf.PI;
	//	}
	//	if (debugMove)
	//	{
	//		moveTest.x = Mathf.Cos(debugAngle);
	//		moveTest.z = Mathf.Sin(debugAngle);

	//		Move(moveTest);
	//	}
	//	if (debugCamera)
	//	{
	//		moveTest.x = 1.0f;
	//		moveTest.y = 0.0f;
	//		MoveCamera(camTest);
	//	}
	//	if (debugDodge)
	//	{
	//		debugDodgeTmr += Time.deltaTime;
	//		//if (debugDodgeTmr > DgeTmrMax * 2.0f)
	//		{
	//			debugDodgeTmr = 0.0f;
	//			int res = 1;
	//			switch (debugDodgeType)
	//			{
	//				case 0:
	//					res = DodgeLeft();
	//					break;
	//				case 1:
	//					res = DodgeForward();
	//					break;
	//				case 2:
	//					res = DodgeRight();
	//					break;
	//				case 3:
	//					res = DodgeRight();
	//					break;
	//				case 4:
	//					res = DodgeBackwards();
	//					break;
	//				case 5:
	//					res = DodgeLeft();
	//					break;
	//			}
	//			debugDodgeType++;
	//			if (debugDodgeType > 5)
	//				debugDodgeType = 0;
	//		}
	//	}
	//	if (debugGuard)
	//	{
	//		debugGrdTmr += Time.deltaTime;
	//		int res = 1;
	//		switch (debugGrdType)
	//		{
	//			case 0:
	//				res = GuardUpwards();
	//				break;
	//			case 1:
	//				res = GuardLeft();
	//				break;
	//			case 2:
	//				res = GuardRight();
	//				break;
	//		}

	//		//if (debugGrdTmr > GrdTmrMax * 5.0f)
	//		{
	//			debugGrdTmr = 0.0f;
	//			debugGrdType++;
	//			if (debugGrdType > 2)
	//				debugGrdType = 0;
	//		}
	//	}

	//}
	#endregion


	// Change State Function
	// returns -1 on failure
	// returns 1 on success
	public int ChangeState(State _nextState)
	{
		//if (attackScript.AtkTmrCur != 0.0f || dodgeScript.DgeTmrCur != 0.0f)
		//	return -1;
		if (stateTable[(int)curState, (int)_nextState] == false)
			return -1;
		if (_nextState == State.IDLE)
			animation.CrossFade("Idle", 0.1f);
		if ((_nextState == State.GRD_TOP && curState != State.PARRY
			&& curState != State.GRD_TOP && curState != State.GRD_LEFT && curState != State.GRD_RIGHT)
			|| (_nextState == State.GRD_LEFT && curState != State.PARRY
			&& curState != State.GRD_TOP && curState != State.GRD_LEFT && curState != State.GRD_RIGHT)
			|| (_nextState == State.GRD_RIGHT && curState != State.PARRY
			&& curState != State.GRD_TOP && curState != State.GRD_LEFT && curState != State.GRD_RIGHT))
		{
			lastState = curState;
			curState = State.PARRY;
			return 1;
		}

		if (_nextState == State.DEAD)
		{
			if (Targeting_CubeSpawned != null)
			{
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject); // apparently the solution.
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Destroy(Targeting_CubeSpawned.gameObject);
				Targeting_CubeSpawned = null;
			}
			if (rockedOn)
				ToggleLockon();
		}

		//if (_nextState == State.ATK_VERT
		//	|| _nextState == State.ATK_LTR
		//	|| _nextState == State.ATK_RTL
		//	|| _nextState == State.ATK_STAB
		//	|| _nextState == State.ATK_KICK)
		//{
		//	NextAttack = _nextState;
		//}
		//else
		//	NextAttack = State.IDLE; // cant set to null =(

		lastState = curState;
		curState = _nextState;

		// New things, added by Dakota 1/13 whatever PM
		// Degub Stuff
		if (lastState != curState && degubber != null)
			degubber.GetComponent<DebugMonitor>().UpdateText("New State: " + transform.tag + " " + curState.ToString());

		return 1;
	}

	// Move Function
	// Moves in direction of _dir.x and _dir.z
	public int Move(Vector3 _dir)
	{
		if (_dir == Vector3.zero && curState == State.DANCE)
			return -1;
		if (_dir == Vector3.zero && (curState == State.MOVING || curState == State.IDLE))
			return ChangeState(State.IDLE);
		if (ChangeState(State.MOVING) == -1)
			return -1;

		// error check the input
		if (_dir.x > 1.0f)
			_dir.x = 1.0f;
		else if (_dir.x < -1.0f)
			_dir.x = -1.0f;

		if (_dir.z > 1.0f)
			_dir.z = 1.0f;
		else if (_dir.z < -1.0f)
			_dir.z = -1.0f;

		// scale the input to time and our speed
		_dir.x *= Time.deltaTime;
		_dir.z *= Time.deltaTime;
		//_dir.x *= moveSpeed;
		//_dir.z *= moveSpeed;


		if (_dir.magnitude > 0.01f && !rockedOn)
		{
			animation.CrossFade("Walk Forward");
		}
		else if (_dir.magnitude > 0.01f && rockedOn)
		{
			if (Mathf.Abs(_dir.x) > Mathf.Abs(_dir.z))
			{
				if (_dir.x < 0.0f)
                    animation.CrossFade("Walk Left");
				else if (_dir.x > 0.0f)
                    animation.CrossFade("Walk Right");
			}
			else
                animation.CrossFade("Walk Forward");
		}
		else if (animation.isPlaying == false) // only revert to idle if not moving or playing another animation
			animation.Play("Idle");
		// change rotation to match camera Y, translate, then return to actual rotation
		//Vector3 oldPos = transform.position;
		orgRot = transform.rotation;
		if (tag == "Player")
		{
			Quaternion camRot = camScript.followCam.transform.rotation;
			camRot.x = orgRot.x;
			camRot.z = orgRot.z;
			transform.rotation = camRot;
		}
		// cant collide with this
		if (oldPos != transform.position)
			oldPos = transform.position;
		transform.Translate(new Vector3(_dir.x, 0.0f, _dir.z) * moveSpeed);
		nextDir.x = _dir.x;
		nextDir.y = 0.0f;
		nextDir.z = _dir.z;
		//controller.Move(nextDir);

		// will collide but presents a whole bunch of new issues
		//Vector3 vel = _dir;
		//vel += _dir.x * camScript.followCam.transform.right;
		//vel += _dir.z * camScript.followCam.transform.forward;
		//vel.y = rigidbody.velocity.y;
		//vel.Normalize();
		//vel *= moveSpeed;
		//rigidbody.velocity = vel;

		transform.rotation = orgRot;
		// the puppet faces the direction it is moving in
		if (!rockedOn)
		{
			Vector3 dir = transform.position - oldPos;
			dir.y = 0.0f;
			//Vector3 towards = transform.position + new Vector3(_dir.x, 0.0f, _dir.z);
			Vector3 towards = transform.position + dir;
			towards.y = transform.position.y;
			transform.LookAt(towards);
		}

		return 1;
	}

	public int MoveCamera(Vector3 _dir)
	{
		return camScript.MoveCamera(_dir);
	}

	public int ResetCamera()
	{
		if (rockedOn)
			return -1;
		else
			return camScript.ResetCamera();
	}

	public int ToggleLockon()
	{
		if (curState == State.DEAD)
			return -1;
		if (curTarg != null)
		{
			float dist = Vector3.SqrMagnitude(curTarg.transform.position - transform.position);
			if (dist > targMaxDist && !rockedOn)
				return -1;
		}
		else if (rockedOn == false) // cant lock on if we don't have a target
			return -1;

		if (Targeting_CubeSpawned != null && !rockedOn)
		{
			Targeting_CubeSpawned.GetComponent<Targeting_CubeScript>().scaleSpeed = 10.0f;
		}
		else if (Targeting_CubeSpawned != null)
		{
			Targeting_CubeSpawned.GetComponent<Targeting_CubeScript>().scaleSpeed = 1.0f;
		}

		return camScript.ToggleLockon();
	}

	public int SlashVert()
	{
		NextAttack = State.ATK_VERT;
		if (ChangeState(State.ATK_VERT) == 1)
			return attackScript.SlashVert(this);
		else
			return -1;
	}

	public int SlashLTR()
	{
		NextAttack = State.ATK_LTR;
		if (ChangeState(State.ATK_LTR) == 1)
			return attackScript.SlashLTR(this);
		else
			return -1;
	}

	public int SlashRTL()
	{
		NextAttack = State.ATK_RTL;
		if (ChangeState(State.ATK_RTL) == 1)
			return attackScript.SlashRTL(this);
		else
			return -1;
	}

	public int Thrust()
	{
		NextAttack = State.ATK_STAB;
        if (gameObject.tag == "Player")
        {
            if (currZen - 2.5f < 0.0f)
            {
                return 1;
            }
        }
		if (ChangeState(State.ATK_STAB) == 1)
			return attackScript.Thrust(this);
		else
			return -1;
	}

	public int Kick()
	{
		NextAttack = State.ATK_KICK;
		if (ChangeState(State.ATK_KICK) == 1)
			return attackScript.Kick(this);
		else
			return -1;
	}

	public int GuardUpwards()
	{
		if (ChangeState(State.GRD_TOP) == 1)
			return guardScript.GuardUpwards(this);
		else
			return -1;
	}

	public int GuardLeft()
	{
		if (ChangeState(State.GRD_LEFT) == 1)
			return guardScript.GuardLeft(this);
		else
			return -1;
	}

	public int GuardRight()
	{
		if (ChangeState(State.GRD_RIGHT) == 1)
			return guardScript.GuardRight(this);
		else
			return -1;
	}

	public int DodgeForward()
	{
		if (ChangeState(State.DGE_FORWARD) == 1)
			return dodgeScript.DodgeForward(this);
		else
			return -1;
	}

	public int DodgeLeft()
	{
		if (ChangeState(State.DGE_LEFT) == 1)
			if (rockedOn)
				return dodgeScript.DodgeLeft(this);
			else
				return dodgeScript.DodgeForward(this);
		else
			return -1;
	}

	public int DodgeRight()
	{
		if (ChangeState(State.DGE_RIGHT) == 1)
			if (rockedOn)
				return dodgeScript.DodgeRight(this);
			else
				return dodgeScript.DodgeForward(this);
		else
			return -1;
	}

	public int DodgeBackwards()
	{
		if (ChangeState(State.DGE_BACK) == 1)
			if (rockedOn)
				return dodgeScript.DodgeBackwards(this);
			else
				return dodgeScript.DodgeForward(this);
		else
			return -1;
	}

	// Resolve collisions when an attack is made.
	// returns 1 on success, -1 on failure.
	public int ResolveHit(GameObject _otherObject)
	{
		//public string[,] animTable;
		/*HOW TO SET UP A 2D ARRAY*/
		/*METHOD1*/
		//animTable = new string[,] { { "00", "01", "02" }, { "10", "11", "12" }, { "20", "21", "22" } };
		/*METHOD2*/
		//animTable = new string[18, 18];
		//animTable[(int)PuppetScript.State.IDLE, (int)PuppetScript.State.IDLE] = "IDLE";
		if (rhScript != null)
			return rhScript.ResolveHit(_otherObject);
		else
			return -1;
	}

	// Cube!!!
	public HitBox cube;
	// Sam: activate our attack hitbox
	void Attack() // Defender of the polyverse!
	{// Cube!!!
		if (cube) // defeat the dreaded cone!
			cube.Attack(); // Uses three-dimensional pictures!
	}// Cube.


	public void PlaySwish()
	{
		Instantiate(swordSwish, transform.position, Quaternion.identity);
	}

	// IsState() functions
	// checks the multiple states for various actions so we can do so in a single line.
	public bool IsAttackState()
	{
		return (curState == State.ATK_KICK
			|| curState == State.ATK_LTR
			|| curState == State.ATK_RTL
			|| curState == State.ATK_STAB
			|| curState == State.ATK_VERT);
	}

	public bool IsDodgeState()
	{
		return (curState == State.DGE_BACK
			|| curState == State.DGE_FORWARD
			|| curState == State.DGE_RIGHT);
	}

	public bool IsGuardState()
	{
		return (curState == State.GRD_LEFT
			|| curState == State.GRD_RIGHT
			|| curState == State.GRD_TOP);
	}

	// SetWindupMod(), SetSwingMod(), SetRecoverMod() functions
	// Animations call these functions at the correct time in order to adjust animation speed.
	public void SetWindupMod(AttackModType _atkType)
	{
		if (Input_AltScript != null) // no combos
			attackScript.attackSpeed = AnimMods[(int)_atkType].windup;
		else if (InputScript != null)
		{
			if (NextAttack != State.IDLE)
			{
				switch (NextAttack)
				{
					case State.ATK_VERT:
						{
							//attackScript.attackSpeed = 2.0f;
							animation["Down Slash"].time = 0.22f;
							NextAttack = State.IDLE;
							break;
						}
					case State.ATK_LTR:
						{
							//attackScript.attackSpeed = 2.0f;
							animation["Right Slash"].time = 0.21f;
							NextAttack = State.IDLE;
							break;
						}
					case State.ATK_RTL:
						{
							//attackScript.attackSpeed = 1000.0f;
							//NextAttack = State.IDLE;
							break;
						}
				}
			}
		}
	}
	public void SetSwingMod(AttackModType _atkType)
	{
		attackScript.attackSpeed = AnimMods[(int)_atkType].swing;
	}
	public void SetRecoverMod(AttackModType _atkType)
	{
		if (Input_AltScript != null) // no combos
			attackScript.attackSpeed = AnimMods[(int)_atkType].recover;
		else if (InputScript != null)
		{
			if (NextAttack != State.IDLE)
			{
				switch (NextAttack)
				{
					case State.ATK_VERT:
						{
							//ChangeState(State.IDLE);
							//SlashVert();
							attackScript.attackSpeed = AnimMods[(int)_atkType].recover;
							break;
						}
					case State.ATK_LTR:
						{
							ChangeState(State.IDLE);
							SlashVert();
							break;
						}
					case State.ATK_RTL:
						{
							ChangeState(State.IDLE);
							SlashLTR();
							break;
						}
					case State.ATK_STAB:
						{
							ChangeState(State.IDLE);
							Thrust();
							break;
						}
					case State.ATK_KICK:
						{
							ChangeState(State.IDLE);
							Kick();
							break;
						}
				}
			}
		}
	}

	public void Death()
	{
		Win_Loss theThing = GameObject.Find("WinLoss").GetComponent<Win_Loss>();
		if (theThing != null)
		{
			if (gameObject.name == "Boss Enemy") //quick n dirty
				theThing.YouWin();
			if (gameObject.tag == "Enemy")
				theThing.Decrement();
			else if (gameObject.tag == "Player")
				theThing.YouLose();
		}
	}
}
