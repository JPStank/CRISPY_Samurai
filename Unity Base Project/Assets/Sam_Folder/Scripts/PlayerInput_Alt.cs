using UnityEngine;
using System.Collections;

public class PlayerInput_Alt : MonoBehaviour
{
	//public float speed = 2.0f;
	enum ATTACKS { NONE = 0, VERT, LTR, RTL }
	ATTACKS lastAttack;

	float deadZone = 0.25f, bufferTime = 0.0f, maxTime = 0.5f;
	//	maxStam, curStam, regenRate = 1.0f;
	public PuppetScript puppet;
	//public GameObject swordSwish;
	public GameObject guard;
	//public Image staminaBar;

	MyStance stance;

	// Use this for initialization
	void Start()
	{
		//curStam = maxStam = 5.0f;
		puppet = gameObject.GetComponent<PuppetScript>();
		lastAttack = ATTACKS.NONE;
		if (guard)
			stance = guard.GetComponent<MyStance>();
	}

	// Update is called once per frame
	void Update()
	{
		//if (curStam < maxStam)
		//{
		//	curStam += Time.deltaTime * regenRate;
		//	if (curStam > maxStam)
		//		curStam = maxStam;
		//}
		//staminaBar.fillAmount = curStam / maxStam;
		HandleInput();
		//stuff
	}

	void HandleInput()
	{
		float lHorizontal, lVertical, rHorizontal, rVertical;

		lHorizontal = InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.X);
		lVertical = InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y);
		rHorizontal = InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.X);
		rVertical = InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.Y);

		Vector3 dir = new Vector3(lHorizontal, 0.0f, -lVertical);
		puppet.Move(dir);

		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.DOWN)
			&& !puppet.rockedOn)
		{
			puppet.ResetCamera();
		}

		if (InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.RIGHT) > 0.0f)
		{
			SetLastNone();

			if (guard)
				guard.SetActive(true);
			//bufferTime = 0.0f;
			//if (Input.GetButton("P1_Button_Y"))
			//	puppet.GuardUpwards();
			//else if (Input.GetButton("P1_Button_X"))
			//	puppet.GuardLeft();
			//else if (Input.GetButton("P1_Button_B"))
			//	puppet.GuardRight();

			if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.X, InputChecker.BUTTON_STATE.HELD))
			{
				puppet.GuardLeft();
				if (stance)
				{
					stance.SetLeftRed();
					stance.ClearRight();
					stance.ClearTop();
				}
			}
			else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.B, InputChecker.BUTTON_STATE.HELD))
			{
				puppet.GuardRight();
				if (stance)
				{
					stance.SetRightRed();
					stance.ClearLeft();
					stance.ClearTop();
				}
			}
			else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.Y, InputChecker.BUTTON_STATE.HELD))
			{
				puppet.GuardUpwards();
				if (stance)
				{
					stance.SetTopRed();
					stance.ClearLeft();
					stance.ClearRight();
				}
			}
			else
			{
				if (stance)
					stance.Clear();
			}
		}
		//else
		//{
		//}
		else
		{
			if (guard)
				guard.SetActive(false);
			//if (bufferTime <= 0.0f)
			//{
			if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_R, InputChecker.BUTTON_STATE.DOWN))
			{
				//puppet.rockedOn = !puppet.rockedOn;
				puppet.ToggleLockon();
			}

			if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BACK, InputChecker.BUTTON_STATE.DOWN))
			{
				if (Application.loadedLevelName != "Hub_Scene")
					Application.LoadLevel("Hub_Scene");
				else
					Application.LoadLevel("Menu_Scene");
			}

			if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.X, InputChecker.BUTTON_STATE.DOWN))
			{
				//if (bufferTime >= 0.0f /*&& curStam >= 1.0f*/)
				//{
				//	//curStam -= 1.0f;
				//	//if (curStam < 0.0f)
				//	//	curStam = 0.0f;
				//	//int x = 5; //for debug
				//	switch (lastAttack)
				//	{
				//		case ATTACKS.NONE:
				//			puppet.SlashRTL();
				//			break;
				//		case ATTACKS.VERT:
				//			puppet.SlashRTL();
				//			break;
				//		case ATTACKS.LTR:
				//			puppet.SlashVert();
				//			break;
				//		case ATTACKS.RTL:
				//			puppet.SlashLTR();
				//			break;
				//		default:
				//			break;
				//	}
				//}
				//bufferTime = maxTime;

				puppet.SlashLTR();
			}
			else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.Y, InputChecker.BUTTON_STATE.DOWN))
			{
				puppet.SlashVert();
			}
			else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.B, InputChecker.BUTTON_STATE.DOWN))
			{
				puppet.SlashRTL();
			}
			else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN))
			{
				//puppet dodge
				//if (curStam >= 2.0f)
				//{
				//curStam -= 2.0f;
				//if (curStam < 0.0f)
				//	curStam = 0.0f;

				if (lHorizontal > deadZone)
					puppet.DodgeRight();
				else if (lHorizontal < -deadZone)
					puppet.DodgeLeft();
				else if (lVertical < -deadZone)
					puppet.DodgeForward();
				else
					puppet.DodgeBackwards();
				//}
			}
			//}
			else
			{
				bufferTime -= Time.deltaTime;
				if (bufferTime <= 0.0f)
				{
					bufferTime = 0.0f;
					lastAttack = ATTACKS.NONE;
				}
			}
		}

		Vector3 camDir = new Vector3(rHorizontal, -rVertical, 0.0f);
		puppet.MoveCamera(camDir);
	}

	// Sam: ugly interface for animations
	void SetLastLTR()
	{
		lastAttack = ATTACKS.LTR;
	}

	void SetLastRTL()
	{
		lastAttack = ATTACKS.RTL;
	}

	void SetLastVert()
	{
		lastAttack = ATTACKS.VERT;
	}

	void SetLastNone()
	{
		lastAttack = ATTACKS.NONE;
	}
}
