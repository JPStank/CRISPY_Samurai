using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class GUI : MonoBehaviour
{
	public Canvas theGUI;
	public Canvas tutorial;
	public Image guardLeft, guardRight, guardTop;
	public Image tally1, tally2, tally3;
	private PuppetScript player;

	public List<string> dances;

	bool action = true;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>();

		dances.Add("Twerk");
		dances.Add("Gangnam Style");
        dances.Add("Robot");
        dances.Add("Thriller 1");
        dances.Add("Thriller 2");
        dances.Add("Thriller 3");
        dances.Add("Thriller 4");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player.curState == PuppetScript.State.GRD_LEFT)
			guardLeft.color = Color.red;
		else
			guardLeft.color = Color.white;

		if (player.curState == PuppetScript.State.GRD_TOP)
			guardTop.color = Color.red;
		else
			guardTop.color = Color.white;

		if (player.curState == PuppetScript.State.GRD_RIGHT)
			guardRight.color = Color.red;
		else
			guardRight.color = Color.white;

		if (player.curTallys == 3)
		{
			tally1.color = Color.white;
			tally2.color = Color.white;
			tally3.color = Color.white;
		}
		else if (player.curTallys == 2)
		{
			tally1.color = Color.white;
			tally2.color = Color.white;
			tally3.color = Color.black;
		}
		else if (player.curTallys == 1)
		{
			tally1.color = Color.white;
			tally2.color = Color.black;
			tally3.color = Color.black;
		}
		else if (player.curTallys == 0)
		{
			tally1.color = Color.black;
			tally2.color = Color.black;
			tally3.color = Color.black;
		}

		//if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.RIGHTSTICK_CLICK, InputChecker.BUTTON_STATE.DOWN) && !action)
		//{
		//	action = true;
		//	theGUI.enabled = !theGUI.enabled;
		//}
		//else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.RIGHTSTICK_CLICK, InputChecker.BUTTON_STATE.UP))
		//{
		//	action = false;
		//}

		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_L, InputChecker.BUTTON_STATE.DOWN))
		{
			//action = true;
			player.ChangeState(PuppetScript.State.DANCE);
			player.animation.Play(dances[Random.Range(0, dances.Count)]);
			//GameObject.FindGameObjectWithTag("Player").GetComponent<Animation>().Play("Twerk");
		}

		if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.X) < 0.0f && !action)
		{
			action = true;
			if (tutorial)
			{
				tutorial.enabled = !tutorial.enabled;
			}
		}
		else if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.X) == 0.0f)
		{
			//tutorial.enabled = false;

			action = false;
		}

	}
}
