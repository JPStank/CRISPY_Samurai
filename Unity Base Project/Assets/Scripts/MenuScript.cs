using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour
{

    //float transitionTime = 2.0f;

    int selectedButton = 0;

    public int buttonMax;

    public List<Button> buttons;

    bool action = true;

	// Use this for initialization
	void Start ()
    {
        //SwitchLevel temp = GetComponent<SwitchLevel>();
        //if (temp)
        //{
        //    temp.EnableCountdown(false, false);
        //}
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y) == 0)
        {
            action = false;
        }
        if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y) < 0 && !action)
        {
            selectedButton--;
            if (selectedButton < 0)
            {
                selectedButton = buttonMax - 1;
            }
            action = true;
        }
        else if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y) > 0 && !action)
        {
            selectedButton++;
            if (selectedButton > buttonMax - 1)
            {
                selectedButton = 0;
            }
            action = true;

        }
        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN) && !action)
        {
            buttons[selectedButton].onClick.Invoke();
            action = true;
        }

        buttons[selectedButton].Select();

	}

    // When a button is pressed, this function is invoked
    public void SwitchLevel(string levelName)
    {

        //SwitchLevel temp = GetComponent<SwitchLevel>();

        //if (temp)
        //{
        //    temp.m_fDelay = transitionTime;
        //    temp.m_szLevel = levelName;
        //    temp.EnableCountdown(true, true);
        //}

        Application.LoadLevel(levelName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
