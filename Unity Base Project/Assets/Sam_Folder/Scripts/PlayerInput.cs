using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public float speed = 2.0f;
	float deadZone = 0.25f, bufferTime = 0.0f, maxTime = 1.0f;
	public PuppetScript puppet;
	GameObject test;

	[SerializeField]
	Camera myCamera;

	// Use this for initialization
	void Start()
	{
		//test = this.gameObject;
		puppet = gameObject.GetComponent<PuppetScript>();
	}

	// Update is called once per frame
	void Update()
	{
		HandleInput();
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
			if(rHorizontal > deadZone)
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
			Vector3 camDir = new Vector3(rHorizontal, rVertical, 0.0f);
			puppet.MoveCamera(camDir);
		}

		//if (bufferTime <= 0.0f)
		//{

			if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.X, InputChecker.BUTTON_STATE.DOWN))
			{
				//puppet slash
				if (lHorizontal < -deadZone)
				{
					puppet.SlashRTL();
					Debug.Log("RIGHT SLASH!");
				}
				else if (lHorizontal > deadZone)
				{
					puppet.SlashLTR();
					Debug.Log("LEFT SLASH!");
				}
				else if (lVertical < deadZone)
				{
					puppet.SlashVert();
					Debug.Log("TOP SLASH!");
				}

				//bufferTime = maxTime;
			}
			else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.Y, InputChecker.BUTTON_STATE.DOWN))
			{
				//puppet thrust
				Debug.Log("THRUST!");
				//bufferTime = maxTime;
			}
			else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.B, InputChecker.BUTTON_STATE.DOWN))
			{
				//puppet kick
				Debug.Log("KICK!");
				//bufferTime = maxTime;
			}
			else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN))
			{
				//puppet dodge
				Debug.Log("DODGE!");
				if (lHorizontal > deadZone)
					puppet.DodgeRight();
				else if (lHorizontal < -deadZone)
					puppet.DodgeLeft();
				else if (lVertical < -deadZone)
					puppet.DodgeForward();
				else
					puppet.DodgeBackwards();
				//bufferTime = maxTime;
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
