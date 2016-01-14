using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AI_Controller : MonoBehaviour
{
	List<Action> actions;

	bool alive = true;
	bool inRange = false;
	bool behaving = false;
	Action currentAction;
	int nextAction;

	// Use this for initialization
	void Start ()
	{
		currentAction = actions[0];
		nextAction = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (alive)
		{
			if (inRange || currentAction.isBehaving())
			{
				if (currentAction.Execute() == COMPLETION_STATE.COMPLETE)
				{
					// Increment the action
					currentAction = actions[nextAction];
					nextAction++;

					if (nextAction > actions.Count)
					{
						nextAction = 0;
					}
				}
			}
			else
			{
				SeekPlayer();
			}
		}
	}

	void SeekPlayer()
	{

	}
}
