using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuppetScript : MonoBehaviour
{

	public enum State
	{
		IDLE = 0, MOVING, FLINCH, STUN, DEAD,
		ATK_VERT, ATK_LTR, ATK_RTL, ATK_STAB, ATK_KICK,
		PARRY,
		GRD_TOP, GRD_LEFT, GRD_RIGHT,
		DGE_FORWARD, DGE_LEFT, DGE_RIGHT, DGE_BACK
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
	public float moveSpeed;
	public float camSpeed;
	public float AtkTmrMax;
	public float DgeTmrMax;
	public float GrdTmrMax;
	public bool debugMove = false;
	public bool debugCamera = false;
	public bool debugDodge = false;
	public bool debugGuard = false;
	public bool rockedOn = false;

	private Vector3 moveTest;
	private Vector3 camTest;
	private float debugAngle;
	private int debugDodgeType = 0;
	private float debugDodgeTmr = 0.0f;
	private int debugGrdType = 0;
	private float debugGrdTmr = 0.0f;


	// Use this for initialization
	void Start()
	{
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

        //temp = GetComponent<Animator>();
        //Animetor = (Animator)temp;

		lastState = curState = State.IDLE;
		moveSpeed = 10.0f;
		camSpeed = 8.0f;
		AtkTmrMax = 1.0f;
		DgeTmrMax = 0.5f;
		GrdTmrMax = 0.2f;


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

	// Update is called once per frame
	void Update()
	{

		DoDegub();

		//if (tag == "Player")
		//camScript.UpdateCam();

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
		if (attackScript.AtkTmrCur != 0.0f || dodgeScript.DgeTmrCur != 0.0f)
			return -1;
		if (_nextState == State.MOVING && curState != State.IDLE && curState != State.MOVING)
			return -1;
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
		else if ((_nextState == State.GRD_TOP && curState == State.PARRY)
			|| (_nextState == State.GRD_LEFT && curState == State.PARRY)
			|| (_nextState == State.GRD_RIGHT && curState == State.PARRY))
			return -1;

		lastState = curState;
		curState = _nextState;
		return 1;
	}

	// Move Function
	// Moves in direction of _dir.x and _dir.z
	public int Move(Vector3 _dir)
	{
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

		if (_dir.magnitude > 0.01f)
			animation.Play("Walk Forward");
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
		// animation.Play(animTable[(int)curState, (int)otherState]);
	}
}
