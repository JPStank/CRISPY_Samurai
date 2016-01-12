using UnityEngine;
using System.Collections;

/**************************************************************
 * File Name: InputChecker.cs
 * Author: Ryan Simmons
 * Date: 11/13/2015
 * Purpose: InputChecker is used to check controller input for
 * four players locally, automatically adjusting for PC or 
 * Android build, as well as checking for Mouse movement in
 * the X, Y, and Scrollwheel. The corresponding InputManager.asset
 * must be used in the project for any of these functions to work.
 * The InputManager.asset is already in the UnityBaseProject this
 * script was provided with. Feel free to modify this script as
 * desired.
***************************************************************/

public static class InputChecker
{
	public enum AXIS { X = 0, Y, SCROLLWHEEL };
	public enum PLAYER_NUMBER { ONE = 1, TWO, THREE, FOUR };
	public enum CONTROLLER_BUTTON { A, B, X, Y, BUMPER_L, BUMPER_R, START, BACK, LEFTSTICK_CLICK, RIGHTSTICK_CLICK };
	public enum JOYSTICK { LEFT, RIGHT, DPAD };
	public enum TRIGGER { LEFT, RIGHT };
	public enum BUTTON_STATE { UP, DOWN };

/******************************************************
 * Function: GetAxis
 * Parameters: 
 *		whichPlayer - Players ONE through FOUR
 *		whichJoystick - Left, Right, or DPAD
 *		whichAxis - X or Y
 * Return:
 *		Returns the float value of the axis desired for
 *		the specific player and joystick 
******************************************************/
	public static float GetAxis(PLAYER_NUMBER whichPlayer, JOYSTICK whichJoystick, AXIS whichAxis)
	{
		float axisValue = 0.0f;

		string inputToCheck = "";

#if UNITY_ANDROID
		if (whichJoystick != JOYSTICK.LEFT)
			inputToCheck += "A_";
#endif

		inputToCheck += "P" + ((int)whichPlayer).ToString() + "_";

		switch (whichJoystick)
		{
			case JOYSTICK.LEFT:
				inputToCheck += "LeftStick_";
				break;
			case JOYSTICK.RIGHT:
				inputToCheck += "RightStick_";
				break;
			case JOYSTICK.DPAD:
				inputToCheck += "DPad_";
				break;
			default:
				break;
		}

		switch (whichAxis)
		{
			case AXIS.X:
				inputToCheck += "X";
				break;
			case AXIS.Y:
				inputToCheck += "Y";
				break;
			default:
				return 0.0f;
		}

		axisValue = Input.GetAxis(inputToCheck);

		return axisValue;
	}

/******************************************************
 * Function: GetTrigger
 * Parameters: 
 *		whichPlayer - Players ONE through FOUR
 *		whichTrigger - Left or Right
 * Return:
 *		Returns the float value of the desired trigger
 *		for the desired player
******************************************************/
	public static float GetTrigger(PLAYER_NUMBER whichPlayer, TRIGGER whichTrigger)
	{
		float triggerValue = 0.0f;

		string inputToCheck = "";

#if UNITY_ANDROID
		inputToCheck += "A_";
#endif

		inputToCheck += "P" + ((int)whichPlayer).ToString() + "_Trigger_";

		switch (whichTrigger)
		{
			case TRIGGER.LEFT:
				inputToCheck += "L";
				break;
			case TRIGGER.RIGHT:
				inputToCheck += "R";
				break;
			default:
				break;
		}
		triggerValue = Input.GetAxis(inputToCheck);

#if UNITY_ANDROID
		inputToCheck.Replace('A', 'F');
		float fireTVValue = Input.GetAxis(inputToCheck);
		if (Mathf.Abs(triggerValue) < Mathf.Abs(fireTVValue))
		{
			triggerValue = fireTVValue;
		}
#endif

		return triggerValue;
	}

/******************************************************
 * Function: GetButton
 * Parameters: 
 *		whichPlayer - Players ONE through FOUR
 *		whichButton - A, B, X, Y, Bumper L, Bumper R,
 *			Leftstick Click, Rightstick Click, Start,
 *			Back
 *		whichState - Up or Down
 * Return:
 *		Returns true or false for the desired state of
 *		the desired button of the desired player
 *	*NOTE* - Any advancement of menus should use A for
 *		confirm and B for cancel as Start and Back are
 *		not Android friendly
******************************************************/
	public static bool GetButton(PLAYER_NUMBER whichPlayer, CONTROLLER_BUTTON whichButton, BUTTON_STATE whichState)
	{
		bool buttonValue = false;

		string inputToCheck = "P" + ((int)whichPlayer).ToString() + "_";

		switch (whichButton)
		{
			case CONTROLLER_BUTTON.A:
				inputToCheck += "Button_A";
				break;
			case CONTROLLER_BUTTON.B:
				inputToCheck += "Button_B";
				break;
			case CONTROLLER_BUTTON.X:
				inputToCheck += "Button_X";
				break;
			case CONTROLLER_BUTTON.Y:
				inputToCheck += "Button_Y";
				break;
			case CONTROLLER_BUTTON.BUMPER_L:
				inputToCheck += "Bumper_L";
				break;
			case CONTROLLER_BUTTON.BUMPER_R:
				inputToCheck += "Bumper_R";
				break;
			case CONTROLLER_BUTTON.START:
				inputToCheck += "Start";
				break;
			case CONTROLLER_BUTTON.BACK:
				inputToCheck += "Back";
				break;
			case CONTROLLER_BUTTON.LEFTSTICK_CLICK:
				inputToCheck += "LeftStick_Click";
				break;
			case CONTROLLER_BUTTON.RIGHTSTICK_CLICK:
				inputToCheck += "RightStick_Click";
				break;
			default:
				break;
		}

		if (whichState == BUTTON_STATE.DOWN)
			buttonValue = Input.GetButtonDown(inputToCheck);
		else if (whichState == BUTTON_STATE.UP)
			buttonValue = Input.GetButtonUp(inputToCheck);

		return buttonValue;

	}

/******************************************************
 * Function: GetMouse
 * Parameters: 
 *		whichAxis - X, Y, Scrollwheel
 * Return:
 *		Returns the float value of the axis desired. 
******************************************************/
	public static float GetMouse(AXIS whichAxis)
	{
		float axisValue = 0.0f;

		string inputToCheck = "Mouse_";

		switch (whichAxis)
		{
			case AXIS.X:
				inputToCheck += "X";
				break;
			case AXIS.Y:
				inputToCheck += "Y";
				break;
			case AXIS.SCROLLWHEEL:
				inputToCheck += "Scrollwheel";
				break;
			default:
				break;
		}

		axisValue = Input.GetAxis(inputToCheck);

		return axisValue;
	}

}
