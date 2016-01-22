using UnityEngine;
using System.Collections;

public class DemoAttack : MonoBehaviour 
{
    public GameObject bDirty;

	// Use this for initialization
	void Start () 
    {
	//stuff
        bDirty.SetActive(false);
    }
	

    void ActivateDirty()
    {
        bDirty.SetActive(true);
    }

    void DisableDirty()
    {
        bDirty.SetActive(false);
    }

	// Update is called once per frame
	void Update () 
    {
        if (animation.isPlaying == false)
        {
            animation.Play("Idle");
        }


        if (Input.GetKeyDown(KeyCode.O))
        {
            animation.Play("Block Left");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            animation.Play("Block Right");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            animation.Play("Block");
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            animation["New Down Slash"].time = 0.36667f;

            animation.Play("New Down Slash");
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            animation.Play("Down Slash");
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            animation["New Left Slash"].time = 0.6f;

            animation.Play("New Left Slash");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            animation.Play("Left Slash");
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            animation["New Right Slash"].time = 0.63333f;

            animation.Play("New Right Slash");
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            animation.Play("Right Slash");
        }
        

        //// TODO: Substitue these arbritary inputs to the actual actions
        //if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN) ||
        //    Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    animation.Play("Left Slash");
        //}
        //else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.B, InputChecker.BUTTON_STATE.DOWN) ||
        //    Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    animation.Play("Right Slash");
        //}
        //else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.Y, InputChecker.BUTTON_STATE.DOWN) ||
        //Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    animation.Play("Down Slash");
        //}
        //else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_L, InputChecker.BUTTON_STATE.DOWN) ||
        //Input.GetKeyDown(KeyCode.O))
        //{
        //    animation.Play("React Side");
        //}
        //else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_R, InputChecker.BUTTON_STATE.DOWN) ||
        //Input.GetKeyDown(KeyCode.P))
        //{
        //    animation.Play("React Front");
        //}
        //else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.X, InputChecker.BUTTON_STATE.DOWN) ||
        //Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    transform.Rotate(Vector3.up, 180.0f);
        //}
    }
}
