using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControllerDebugger : MonoBehaviour
{

    public List<Image> images;

	public Canvas theCanvas;
	public Canvas pauseMenu;
	bool action = true;

    // A    0
    // B    1
    // X    2
    // Y    3
    // RT   4
    // LT   5
    // RB   6
    // LB   7
    // RS   8
    // LS   9
    // R3   10
    // L3   11
    // SEL  12
    // STA  13
    // UP   14
    // DOWN 15
    // LEFT 16
    // RIGHT 17


	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN))
            images[0].color = Color.green;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.UP))
            images[0].color = Color.white;

        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.B, InputChecker.BUTTON_STATE.DOWN))
            images[1].color = Color.red;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.B, InputChecker.BUTTON_STATE.UP))
            images[1].color = Color.white;

        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.X, InputChecker.BUTTON_STATE.DOWN))
            images[2].color = Color.blue;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.X, InputChecker.BUTTON_STATE.UP))
            images[2].color = Color.white;

        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.Y, InputChecker.BUTTON_STATE.DOWN))
            images[3].color = Color.yellow;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.Y, InputChecker.BUTTON_STATE.UP))
            images[3].color = Color.white;

        if (InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.RIGHT) > 0.0f)
            images[4].color = new Color(1.0f, 1.0f - InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.RIGHT), 1.0f, 1.0f);
        else
            images[4].color = Color.white;

        if (InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.LEFT) > 0.0f)
            images[5].color = new Color(1.0f, 1.0f - InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.LEFT), 1.0f, 1.0f);
        else
            images[5].color = Color.white;

        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_R, InputChecker.BUTTON_STATE.DOWN))
            images[6].color = Color.cyan;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_R, InputChecker.BUTTON_STATE.UP))
            images[6].color = Color.white;

        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_L, InputChecker.BUTTON_STATE.DOWN))
            images[7].color = Color.cyan;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_L, InputChecker.BUTTON_STATE.UP))
            images[7].color = Color.white;

        if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.X) != 0.0f || 
            InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.Y) != 0.0f)
        {
            float x = InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.X);
            float y = InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.Y);

            if (x < 0.0f)
                x *= -1.0f;

            if (y < 0.0f)
                y *= -1.0f;

            images[8].color = new Color(1.0f - x, 1.0f - y, 1.0f, 1.0f);
        }
        else
            images[8].color = Color.white;

        if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.X) != 0.0f || 
            InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y) != 0.0f)   
        {
            float x = InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.X);
            float y = InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y);

            if (x < 0.0f)
		        x *= -1.0f;

            if (y < 0.0f)
		        y *= -1.0f;

            images[9].color = new Color(1.0f - x, 1.0f - y, 1.0f, 1.0f);
        }
        else
            images[9].color = Color.white;

        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.RIGHTSTICK_CLICK, InputChecker.BUTTON_STATE.DOWN))
            images[10].color = Color.yellow;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.RIGHTSTICK_CLICK, InputChecker.BUTTON_STATE.UP))
            images[10].color = Color.white;

        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.DOWN))
            images[11].color = Color.yellow;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.UP))
            images[11].color = Color.white;

        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BACK, InputChecker.BUTTON_STATE.DOWN))
            images[12].color = Color.green;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BACK, InputChecker.BUTTON_STATE.UP))
            images[12].color = Color.white;

        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.START, InputChecker.BUTTON_STATE.DOWN))
            images[13].color = Color.green;
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.START, InputChecker.BUTTON_STATE.UP))
            images[13].color = Color.white;


        if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.Y) > 0.0f)
            images[14].color = new Color(1.0f - InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.Y), 1.0f, 1.0f, 1.0f);
        else
            images[14].color = Color.white;

		if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.Y) < 0.0f && !action)
		{
			action = true;
			theCanvas.gameObject.SetActive(!theCanvas.gameObject.activeSelf);
			images[15].color = new Color(1.0f + InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.Y), 1.0f, 1.0f, 1.0f);
		}
		else if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.Y) == 0.0f)
		{
			action = false;
			images[15].color = Color.white;
		}

        if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.X) < 0.0f)
            images[16].color = new Color(1.0f + InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.X), 1.0f, 1.0f, 1.0f);
        else
            images[16].color = Color.white;

        if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.X) > 0.0f)
            images[17].color = new Color(1.0f - InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.DPAD, InputChecker.AXIS.X), 1.0f, 1.0f, 1.0f);
        else
            images[17].color = Color.white;

        //if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.DOWN))
        //    images[13].color = Color.red;
        //else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.UP))
        //    images[13].color = Color.white;

        //if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.DOWN))
        //    images[14].color = Color.red;
        //else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.UP))
        //    images[14].color = Color.white;

        //if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.DOWN))
        //    images[15].color = Color.red;
        //else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.UP))
        //    images[15].color = Color.white;


        
	}
}
