using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	//public float speed = 2.0f;
	float deadZone = 0.25f, bufferTime = 0.0f, maxTime = 1.0f;
	public PuppetScript puppet;
	public GameObject swordSwish;

	// Use this for initialization
	void Start()
	{
		puppet = gameObject.GetComponent<PuppetScript>();
	}

	// Update is called once per frame
	void Update()
	{
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

		if (InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.RIGHT) > 0.0f)
		{
			if (rHorizontal > deadZone)
			{
				//guard right
				puppet.GuardRight();
			}
			else if (rHorizontal < -deadZone)
			{
				//guard left
				puppet.GuardLeft();
			}
			else if (rVertical < -deadZone)
			{
				//guard top
				puppet.GuardUpwards();
			}
		}
		else
		{
			Vector3 camDir = new Vector3(rHorizontal, -rVertical, 0.0f);
			puppet.MoveCamera(camDir);
		}

		//if (bufferTime <= 0.0f)
		//{
		if(InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_R, InputChecker.BUTTON_STATE.DOWN)
			|| InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.RIGHTSTICK_CLICK, InputChecker.BUTTON_STATE.DOWN))
		{
			//puppet.rockedOn = !puppet.rockedOn;
			puppet.ToggleLockon();
		}

		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BACK, InputChecker.BUTTON_STATE.DOWN))
		{
			Application.LoadLevel("Menu_Scene");
		}

		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.X, InputChecker.BUTTON_STATE.DOWN))
		{
			MailMan.TriggerEvent("test");

			//puppet slash
			if (lHorizontal < -deadZone)
			{
				puppet.SlashRTL();
				//Instantiate(swordSwish, puppet.transform.position, Quaternion.identity);
			}
			else if (lHorizontal > deadZone)
			{
				puppet.SlashLTR();
				//Instantiate(swordSwish, puppet.transform.position, Quaternion.identity);
			}
			else if (lVertical < deadZone)
			{
				puppet.SlashVert();
				//Instantiate(swordSwish, puppet.transform.position, Quaternion.identity);
			}
		}
		else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.Y, InputChecker.BUTTON_STATE.DOWN))
		{
			//puppet thrust
		}
		else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.B, InputChecker.BUTTON_STATE.DOWN))
		{
			//puppet kick
		}
		else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN))
		{
			//puppet dodge
			if (lHorizontal > deadZone)
				puppet.DodgeRight();
			else if (lHorizontal < -deadZone)
				puppet.DodgeLeft();
			else if (lVertical < -deadZone)
				puppet.DodgeForward();
			else
				puppet.DodgeBackwards();
		}
		//}
		//else
		//{
		//	bufferTime -= Time.deltaTime;
		//	if (bufferTime < 0.0f)
		//		bufferTime = 0.0f;
		//}
	}
}
