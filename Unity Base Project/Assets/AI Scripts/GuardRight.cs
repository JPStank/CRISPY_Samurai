using UnityEngine;
using System.Collections;

public class GuardRight : Action
{
	public float GuardTimerMax = 1.0f;
	float timer = 0.0f;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public override COMPLETION_STATE Execute()
	{
		if (!behaving)
		{
			behaving = true;
			puppet.GuardRight();
		}

		timer += Time.deltaTime;
		if (timer >= GuardTimerMax)
		{
			animation.Play("Idle");
			puppet.ChangeState(PuppetScript.State.IDLE);
			behaving = false;
			return COMPLETION_STATE.COMPLETE;
		}

		return COMPLETION_STATE.IN_PROGRESS;
	}
}
