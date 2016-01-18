using UnityEngine;
using System.Collections;

public class ThrustForward : Action {

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
			puppet.Thrust();
			behaving = true;
		}

		if (!animation.IsPlaying("Stab"))
		{
			Debug.Log("Completed Thrust");
			behaving = false;
			return COMPLETION_STATE.COMPLETE;
		}
		else
			return COMPLETION_STATE.IN_PROGRESS;
	}
}
