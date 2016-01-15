using UnityEngine;
using System.Collections;

public class SlashRight : Action {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override COMPLETION_STATE Execute()
    {
        if (!behaving)
        {
            puppet.SlashLTR();
            behaving = true;
        }

        if (!animation.IsPlaying("Right Slash"))
        {
            Debug.Log("Completed Right Slash");
            behaving = false;
            return COMPLETION_STATE.COMPLETE;
        }
        else
            return COMPLETION_STATE.IN_PROGRESS;
    }
}
