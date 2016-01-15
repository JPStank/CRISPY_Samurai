using UnityEngine;
using System.Collections;

public class SlashLeft : Action 
{

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override COMPLETION_STATE Execute()
    {
        if (!behaving)
        {
            puppet.SlashRTL();
            behaving = true;
        }

        if (!animation.IsPlaying("Left Slash"))
        {
            Debug.Log("Completed Left Slash");
            behaving = false;
            return COMPLETION_STATE.COMPLETE;
        }
        else
            return COMPLETION_STATE.IN_PROGRESS;
    }
}
