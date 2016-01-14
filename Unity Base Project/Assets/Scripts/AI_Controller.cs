using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AI_Controller : MonoBehaviour
{
	NavMeshAgent agent;
	GameObject player;
	PuppetScript puppet;

	List<Action> actions;

	bool alive = true;
	bool inRange = false;
	bool behaving = false;
	Action currentAction;
	int nextAction;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		agent = GetComponent<NavMeshAgent>();
		puppet = GetComponent<PuppetScript>();
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
		if (player && agent && puppet)
		{
			Vector3 playerDirection = player.transform.position - gameObject.transform.position;
			Vector3 AIForward = transform.forward;
			float angleToPlayer = Vector3.Angle(AIForward.normalized, playerDirection.normalized);

			//Debug.Log(angleToPlayer + " Minion Script");

			if (angleToPlayer > 15.0f &&
				!currentAction.isBehaving() &&
				(gameObject.transform.position - player.transform.position).magnitude < agent.stoppingDistance &&
				(gameObject.transform.position - player.transform.position).magnitude > 0.78f)
			{
				Debug.Log((gameObject.transform.position - player.transform.position).magnitude);

				// Angle the enemy towards the player
				agent.updateRotation = false;
				agent.updatePosition = false;
				Vector3 playerPosition = player.transform.position;// behaviours[currentBehaviour].positionData;
				Vector3 AIPosition = gameObject.transform.position;
				playerPosition.y = AIPosition.y;
				Vector3 toPlayer = playerPosition - AIPosition;
				toPlayer.Normalize();
				transform.forward = Vector3.RotateTowards(transform.forward, toPlayer, 3.0f * Time.deltaTime, 0.0f);
				angleToPlayer = Vector3.Angle(transform.forward, toPlayer);
				agent.updatePosition = true;
				agent.updateRotation = true;
			}

			else if ((gameObject.transform.position - player.transform.position).magnitude > agent.stoppingDistance && !currentAction.isBehaving())
			{
				// Move the enemy towards the player
				agent.stoppingDistance = 3.0f;
				agent.SetDestination(player.transform.position);
				if (!animation.IsPlaying("Walk Forward"))
				{
					puppet.ChangeState(PuppetScript.State.MOVING);
					animation.Play("Walk Forward");
				}

				if (agent.remainingDistance < agent.stoppingDistance + 0.5f)
				{
					inRange = true;
					animation.Play("Idle");
				}				
			}
			//else if ((gameObject.transform.position - player.transform.position).magnitude < agent.stoppingDistance + 0.5f && !currentAction.isBehaving())
			//{
			//}
		}
	}

	void OnCollisionEnter(Collision col)
	{
	
	}

	void OnCollisionExit(Collision col)
	{

	}

	public void SetRange(bool b)
	{
		inRange = b;
	}
}
