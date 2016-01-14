using UnityEngine;
using System.Collections;

public class SlashTop : Action {

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
            puppet.SlashVert();
            behaving = true;
        }

        if (!animation.IsPlaying("Down Slash"))
        {
            Debug.Log("Completed Top Slash");
            behaving = false;
            return COMPLETION_STATE.COMPLETE;
        }
        else
            return COMPLETION_STATE.IN_PROGRESS;
    }
}
