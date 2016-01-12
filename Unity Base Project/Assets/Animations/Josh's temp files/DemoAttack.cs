using UnityEngine;
using System.Collections;

public class DemoAttack : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

        if (animation.isPlaying == false)
        {
            animation.Play("Idle");
        }

        // TODO: Substitue these arbritary inputs to the actual actions
        if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN) ||
            Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animation.Play("Left Slash");
        }
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.B, InputChecker.BUTTON_STATE.DOWN) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            animation.Play("Right Slash");
        }
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.Y, InputChecker.BUTTON_STATE.DOWN) ||
        Input.GetKeyDown(KeyCode.DownArrow))
        {
            animation.Play("Down Slash");
        }
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_L, InputChecker.BUTTON_STATE.DOWN) ||
        Input.GetKeyDown(KeyCode.O))
        {
            animation.Play("React Side");
        }
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_R, InputChecker.BUTTON_STATE.DOWN) ||
        Input.GetKeyDown(KeyCode.P))
        {
            animation.Play("React Front");
        }
        else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.X, InputChecker.BUTTON_STATE.DOWN) ||
        Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.up, 180.0f);
        }
    }
}
