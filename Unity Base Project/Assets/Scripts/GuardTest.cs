using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuardTest : MonoBehaviour
{
    public Image Left, Right, Top;
    public GameObject debugger;

    bool action = true;
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.RIGHT) == 0 ||
            InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.X) == 0 ||
            InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.Y) == 0)
        {
            Right.color = Color.white;
            Left.color = Color.white;
            Top.color = Color.white;

            if (action)
            {
                //debugger.GetComponent<DebugMonitor>().UpdateText("Guard OFF");   
			}
            action = false;
        }
		
        if (InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.RIGHT) > 0 &&
            InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.X) > 0 && !action)
        {
            Right.color = Color.red;
            Left.color = Color.white;
            Top.color = Color.white;

            action = true;
            debugger.GetComponent<DebugMonitor>().UpdateText("Guard Right!");
        }
        else if (InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.RIGHT) > 0 &&
           InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.X) < 0 && !action)
        {
            Right.color = Color.white;
            Left.color = Color.red;
            Top.color = Color.white;

            action = true;
            debugger.GetComponent<DebugMonitor>().UpdateText("Guard Left!");

        }
        else if (InputChecker.GetTrigger(InputChecker.PLAYER_NUMBER.ONE, InputChecker.TRIGGER.RIGHT) > 0 &&
           InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.RIGHT, InputChecker.AXIS.Y) < 0 && !action)
        {
            Right.color = Color.white;
            Left.color = Color.white;
            Top.color = Color.red;

            action = true;
            debugger.GetComponent<DebugMonitor>().UpdateText("Guard Top!");
        }
	}
}
