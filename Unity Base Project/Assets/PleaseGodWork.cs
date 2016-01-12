using UnityEngine;
using System.Collections;

public class PleaseGodWork : MonoBehaviour 
{

	public Animator attackAnimations;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// TODO: Substitue these arbritary inputs to the actual actions
		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN) ||
			Input.GetKeyDown(KeyCode.LeftArrow))
		{
			attackAnimations.Play("Left Slash");
		}
		else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.B, InputChecker.BUTTON_STATE.DOWN) ||
			Input.GetKeyDown(KeyCode.RightArrow))
		{
			attackAnimations.Play("Right Slash");
		}
		else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.Y, InputChecker.BUTTON_STATE.DOWN) ||
		Input.GetKeyDown(KeyCode.DownArrow))
		{
			attackAnimations.Play("Down Slash");
		}
		else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.X, InputChecker.BUTTON_STATE.DOWN) ||
		Input.GetKeyDown(KeyCode.UpArrow))
		{
			transform.Rotate(Vector3.up, 180.0f);
		}
	}
}
