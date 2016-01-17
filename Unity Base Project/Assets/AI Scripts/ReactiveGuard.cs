using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReactiveGuard : Action
{
	public List<string> dances;
	public PuppetScript playerPuppet;
	public float GuardTimerMax;
	float timer = 0.0f;

	// Use this for initialization
	public void Start()
	{
		//dances = new List<string>();
		
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
		}


		switch (playerPuppet.curState)
		{
			case PuppetScript.State.IDLE:
				animation.Play("Idle");
				puppet.ChangeState(PuppetScript.State.IDLE);
				break;
			case PuppetScript.State.ATK_VERT:
				puppet.GuardUpwards();
				break;
			case PuppetScript.State.ATK_LTR:
				puppet.GuardRight();
				break;
			case PuppetScript.State.ATK_RTL:
				puppet.GuardLeft();
				break;
			case PuppetScript.State.ATK_STAB:
				puppet.DodgeLeft();
				break;
			case PuppetScript.State.ATK_KICK:
				break;
			case PuppetScript.State.DANCE:
				if (puppet.curState != PuppetScript.State.DANCE)
				{
					puppet.ChangeState(PuppetScript.State.DANCE);
					animation.Play(dances[Random.Range(0, dances.Count)]);
				}
				break;
			default:
				break;
		}

		if(puppet.curState != PuppetScript.State.DANCE)
			timer += Time.deltaTime;

		if (timer >= GuardTimerMax && playerPuppet.curState == PuppetScript.State.IDLE)
		{
			timer = 0.0f;
			animation.Play("Idle");
			puppet.ChangeState(PuppetScript.State.IDLE);
			behaving = false;
			return COMPLETION_STATE.COMPLETE;
		}

		return COMPLETION_STATE.IN_PROGRESS;
	}
}
