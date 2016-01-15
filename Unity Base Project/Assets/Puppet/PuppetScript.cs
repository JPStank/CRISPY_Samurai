using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuppetScript : MonoBehaviour
{
	// New things, added by Dakota 1/13 7:22pm
	public GameObject degubber;
	public float curBalance = 100;
	public float maxBalance = 100;

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
	public State lastState;
	public State curState;
	//public State nextState;
	public PuppetAttackScript attackScript;
	public PuppetGuardScript guardScript;
	public PuppetDodgeScript dodgeScript;
	public PuppetCameraScript camScript;
	public Dictionary<string, float> animTimers;
	public GameObject curTarg;
	public GameObject Targeting_Cube;
	private GameObject Targeting_CubeSpawned = null;
	public GameObject[] badguys;
	public Vector3 targOffset;
	public float targMaxDist;
	public float def_moveSpeed;
	public float moveSpeed;
	public float lockMoveSpeedMod;
	public float def_camSpeed;
	public float camSpeed;
	public float AtkTmrMax;
	public float DgeTmrMax;
	public float GrdTmrMax;
	public bool debugMove = false;
	public bool debugCamera = false;
	public bool debugDodge = false;
	public bool debugGuard = false;
	public bool rockedOn = false;

	private string[,] animTable;
	private bool[,] stateTable;
	private Vector3 moveTest;
	private Vector3 camTest;
	private float debugAngle;
	private int debugDodgeType = 0;
	private float debugDodgeTmr = 0.0f;
	private int debugGrdType = 0;
	private float debugGrdTmr = 0.0f;

	public bool canHit = false;

	void ActivateHit()
	{
		canHit = true;
	}
	void DisableHit()
	{
		canHit = false;
	}
	public void SetHit(bool h)
	{
		canHit = h;
	}


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
		//

		animTimers = new Dictionary<string, float>();
		animTimers["Idle"] = 1.833f;
		animTimers["Walk"] = 1.1f;
		animTimers["WalkRight"] = 1.1f;
		animTimers["WalkLeft"] = 1.167f;
		animTimers["Twerk"] = 15.2f;
		animTimers["Block"] = 1.833f;
		animTimers["BlockUp"] = 1.4f;
		animTimers["BlockUpHit"] = 0.667f;
		animTimers["Reaction"] = 0.967f;
		animTimers["Stab"] = 2.133f;
		animTimers["SlashVert"] = 2.267f;
		animTimers["SlashLTR"] = 1.667f;
		animTimers["SlashRTL"] = 2.033f;

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
		badguys = GameObject.FindGameObjectsWithTag("Enemy");

		lastState = curState = State.IDLE;
		if (targOffset == Vector3.zero)
			targOffset.y = 2.0f;
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
		AtkTmrMax = 1.0f;
		DgeTmrMax = 0.5f;
		GrdTmrMax = 0.2f;

		InitAnimTable();
		InitStateTable();

		moveTest = Vector3.zero;
		moveTest.x = 1.0f;
		moveTest.z = 1.0f;
		camTest = Vector3.zero;
		camTest.x = 1.0f;
		camTest.z = 1.0f;

		debugAngle = 0.0f;

		if (tag == "Player")
			camScript.Initialize(this);
		attackScript.Initialize(this);
		dodgeScript.Initialize(this);
		guardScript.Initialize(this);
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
		animTable[(int)State.IDLE, (int)State.ATK_VERT] =
		animTable[(int)State.MOVING, (int)State.ATK_VERT] =
		animTable[(int)State.ATK_VERT, (int)State.ATK_VERT] =
		animTable[(int)State.ATK_RTL, (int)State.ATK_VERT] =
		animTable[(int)State.ATK_LTR, (int)State.ATK_VERT] =
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


		animTable[(int)State.IDLE, (int)State.IDLE] =
		animTable[(int)State.MOVING, (int)State.MOVING] =
		"Twerk";

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
		if (transform.tag == "Player")
		{
			FindTarg();
			if (rockedOn)
			{
				Vector3 target = curTarg.transform.position;
				target.y = transform.position.y;
				transform.LookAt(target);
			}
		}

		DoDegub();
	}

	// FindTarg()
	// find the most relevant enemy and assign him as our current target.
	private void FindTarg()
	{

		if (badguys != null)
		{
			float dist;
			float curDist = dist = 0x0FFFFFFF;
			foreach (GameObject badguy in badguys)
			{
				curDist = Vector3.SqrMagnitude(badguy.transform.position - transform.position);
				if (curDist < dist)
				{
					curTarg = badguy;
					dist = curDist;
				}
			}
			// only have a target if within distance
			if (dist < targMaxDist)
			{
				if (Targeting_CubeSpawned == null)
				{
					Targeting_CubeSpawned = (GameObject)Instantiate(Targeting_Cube, curTarg.transform.position, Quaternion.identity);
				}
				Targeting_CubeSpawned.transform.position = curTarg.transform.position + targOffset;
				Targeting_CubeSpawned.transform.SetParent(curTarg.transform);
			}
			else 
			{
				if (Targeting_CubeSpawned != null)
				{
					Destroy(Targeting_CubeSpawned);
					Targeting_CubeSpawned = null;
				}
				if (rockedOn)
					ToggleLockon();
			}
		}
	}

	// DoDegub()
	// Does the degubs for the testing on the features
	private void DoDegub()
	{
		if (debugMove || debugCamera)
		{
			debugAngle += Time.deltaTime * 2.0f;
			if (debugAngle >= 2.0f * Mathf.PI)
				debugAngle -= 2.0f * Mathf.PI;
		}
		if (debugMove)
		{
			moveTest.x = Mathf.Cos(debugAngle);
			moveTest.z = Mathf.Sin(debugAngle);

			Move(moveTest);
		}
		if (debugCamera)
		{
			moveTest.x = 1.0f;
			moveTest.y = 0.0f;
			MoveCamera(camTest);
		}
		if (debugDodge)
		{
			debugDodgeTmr += Time.deltaTime;
			if (debugDodgeTmr > DgeTmrMax * 2.0f)
			{
				debugDodgeTmr = 0.0f;
				int res = 1;
				switch (debugDodgeType)
				{
					case 0:
						res = DodgeLeft();
						break;
					case 1:
						res = DodgeForward();
						break;
					case 2:
						res = DodgeRight();
						break;
					case 3:
						res = DodgeRight();
						break;
					case 4:
						res = DodgeBackwards();
						break;
					case 5:
						res = DodgeLeft();
						break;
				}
				debugDodgeType++;
				if (debugDodgeType > 5)
					debugDodgeType = 0;
			}
		}
		if (debugGuard)
		{
			debugGrdTmr += Time.deltaTime;
			int res = 1;
			switch (debugGrdType)
			{
				case 0:
					res = GuardUpwards();
					break;
				case 1:
					res = GuardLeft();
					break;
				case 2:
					res = GuardRight();
					break;
			}

			if (debugGrdTmr > GrdTmrMax * 5.0f)
			{
				debugGrdTmr = 0.0f;
				debugGrdType++;
				if (debugGrdType > 2)
					debugGrdType = 0;
			}
		}

	}


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
			animation.Play("Idle");
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


		lastState = curState;
		curState = _nextState;

		// New things, added by Dakota 1/13 whatever PM
		// Degub Stuff
		if (lastState != curState)
		{
			if (degubber)
				degubber.GetComponent<DebugMonitor>().UpdateText("New State: " + transform.tag + " " + curState.ToString());
		}
		//

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
		_dir.x *= moveSpeed;
		_dir.z *= moveSpeed;


		if (_dir.magnitude > 0.01f && !rockedOn)
		{
			animation.Play("Walk Forward");
		}
		else if (_dir.magnitude > 0.01f && rockedOn)
		{
			if (Mathf.Abs(_dir.x) > Mathf.Abs(_dir.z))
			{
				if (_dir.x < 0.0f)
					animation.Play("Walk Left");
				else if (_dir.x > 0.0f)
					animation.Play("Walk Right");
			}
			else
				animation.Play("Walk Forward");
		}
		else if (animation.isPlaying == false) // only revert to idle if not moving or playing another animation
			animation.Play("Idle");
		// change rotation to match camera Y, translate, then return to actual rotation
		Vector3 oldPos = transform.position;
		Quaternion orgRot = transform.rotation;
		if (tag == "Player")
		{
			Quaternion camRot = camScript.followCam.transform.rotation;
			camRot.x = orgRot.x;
			camRot.z = orgRot.z;
			transform.rotation = camRot;
		}
		transform.Translate(_dir.x, 0.0f, _dir.z);
		transform.rotation = orgRot;

		// the puppet faces the direction it is moving in
		if (!rockedOn)
		{
			Vector3 dir = transform.position - oldPos;
			Vector3 towards = transform.position + dir;
			transform.LookAt(towards);
		}

		return 1;
	}

	public int MoveCamera(Vector3 _dir)
	{
		return camScript.MoveCamera(_dir);
	}

	public int ToggleLockon()
	{
		if (curTarg != null)
		{
			float dist = Vector3.SqrMagnitude(curTarg.transform.position - transform.position);
			if (dist > targMaxDist && !rockedOn)
				return -1;
		}
		else return -1;

		if (Targeting_CubeSpawned != null && !rockedOn)
		{
			if (Targeting_CubeSpawned.GetComponent<Targeting_CubeScript>() != null)
				Targeting_CubeSpawned.GetComponent<Targeting_CubeScript>().scaleSpeed = 10.0f;
		}
		else if (Targeting_CubeSpawned != null)
		{
			if (Targeting_CubeSpawned.GetComponent<Targeting_CubeScript>() != null)
				Targeting_CubeSpawned.GetComponent<Targeting_CubeScript>().scaleSpeed = 1.0f;
		}

		return camScript.ToggleLockon();
	}

	public int SlashVert()
	{
		if (ChangeState(State.ATK_VERT) == 1)
			return attackScript.SlashVert(this);
		else
			return -1;
	}

	public int SlashLTR()
	{
		if (ChangeState(State.ATK_LTR) == 1)
			return attackScript.SlashLTR(this);
		else
			return -1;
	}

	public int SlashRTL()
	{
		if (ChangeState(State.ATK_RTL) == 1)
			return attackScript.SlashRTL(this);
		else
			return -1;
	}

	public int Thrust()
	{
		if (ChangeState(State.ATK_STAB) == 1)
			return attackScript.Thrust(this);
		else
			return -1;
	}

	public int Kick()
	{
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
			return dodgeScript.DodgeLeft(this);
		else
			return -1;
	}

	public int DodgeRight()
	{
		if (ChangeState(State.DGE_RIGHT) == 1)
			return dodgeScript.DodgeRight(this);
		else
			return -1;
	}

	public int DodgeBackwards()
	{
		if (ChangeState(State.DGE_BACK) == 1)
			return dodgeScript.DodgeBackwards(this);
		else
			return -1;
	}

	//Placeholder function, does nothing
	public void ResolveHit(PuppetScript.State otherState)
	{
		//public string[,] animTable;
		/*HOW TO SET UP A 2D ARRAY*/
		/*METHOD1*/
		//animTable = new string[,] { { "00", "01", "02" }, { "10", "11", "12" }, { "20", "21", "22" } };
		/*METHOD2*/
		//animTable = new string[18, 18];
		//animTable[(int)PuppetScript.State.IDLE, (int)PuppetScript.State.IDLE] = "IDLE";

		/*EXAMPLE IMPLEMENTATION*/
		// animation.Play(animTable[(int)curState, (int)otherState]);
		string toPlay = animTable[(int)curState, (int)otherState];
		if (toPlay != null)
		{
			//if (degubber)
			//degubber.GetComponent<DebugMonitor>().UpdateText("New Anim: " + toPlay);

			animation.Play(toPlay);

			if (toPlay == "Idle")
				ChangeState(State.IDLE);
			if (toPlay == "React Front" || toPlay == "React Side")
			{

				curBalance -= 25;
				if (curBalance < 0.0f)
					curBalance = 0.0f;

				if (gameObject.tag == "Enemy")
				{
					PuppetScript playerPuppet = GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>();
					playerPuppet.curBalance += 12.5f;
					if (playerPuppet.curBalance > playerPuppet.maxBalance)
						playerPuppet.curBalance = playerPuppet.maxBalance;
				}

				// New things, added by Dakota 1/13 whatever PM
				canHit = false;
				//
				ChangeState(State.FLINCH);
			}
		}
	}

	public HitBox cube;
	// Sam: activate our attack hitbox
	void Attack()
	{
        if (cube)
		    cube.Attack();
	}
}
