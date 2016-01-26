using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss_AI_Controller : MonoBehaviour
{
	public enum ATTACK_TYPE { LEFT, RIGHT, TOP, THRUST, GUARD_LEFT, GUARD_RIGHT, GUARD_TOP, REACTIVE_GUARD, WINDOW_SHORT, WINDOW_MEDIUM, WINDOW_LONG };
	public enum PHASE { ONE, TWO };
	NavMeshAgent agent;
	GameObject player;
	PuppetScript puppet;

	public List<ATTACK_TYPE> phaseOneAttacks, phaseTwoAttacks;
	public List<Action> phaseOne, phaseTwo;
	public PHASE phase = PHASE.ONE;
	ReactiveGuard protectiveAction;
	public bool repeatActions;
	public bool inRange = false;
	public bool minionSummoned = false;
	public bool minionDead = false;
	public float StoppingDistance = 2.7f;
	public GameObject minionSpawnLocation;
	public GameObject retreatPoint;
	public GameObject minion = null;
	PuppetScript minionPuppet;
	public Action currentAction;
	public Armor armor;
	public float shortTimer = 0.0f, mediumTimer = 0.0f, longTimer = 0.0f, guardTimer = 1.0f;
	int nextAction = 0;

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		agent = GetComponent<NavMeshAgent>();
		puppet = GetComponent<PuppetScript>();
		protectiveAction = ScriptableObject.CreateInstance<ReactiveGuard>();
		protectiveAction.animation = animation;
		protectiveAction.puppet = puppet;
		protectiveAction.GuardTimerMax = guardTimer;
		protectiveAction.playerPuppet = player.GetComponent<PuppetScript>();
		protectiveAction.dances = new List<string>();
		protectiveAction.dances.Add("Twerk");
		protectiveAction.dances.Add("Gangnam Style");
		protectiveAction.dances.Add("Robot");
		PopulateActions(phaseOneAttacks, ref phaseOne);
		if (!repeatActions)
			PopulateActions(phaseTwoAttacks, ref phaseTwo);
		else
			PopulateActions(phaseOneAttacks, ref phaseTwo);

		currentAction = phaseOne[0];
	}

	// Update is called once per frame
	void Update()
	{
		if(puppet.curState != PuppetScript.State.DEAD)
		{
			Vector3 playerDirection = player.transform.position - gameObject.transform.position;
			playerDirection.y = 0.0f;
			Vector3 AIForward = transform.forward;
			float angleToPlayer = Vector3.Angle(AIForward.normalized, playerDirection.normalized);

			if (phase == PHASE.ONE)
			{
				PhaseOne(angleToPlayer);
			}

			if (phase == PHASE.TWO)
			{
				PhaseTwo(angleToPlayer);
			}
		}
	}

	void PopulateActions(List<ATTACK_TYPE> attacks, ref List<Action> actions)
	{
		actions = new List<Action>();
		foreach (ATTACK_TYPE attack in attacks)
		{
			switch (attack)
			{
				case ATTACK_TYPE.LEFT:
					{
						SlashLeft move = ScriptableObject.CreateInstance<SlashLeft>();
						move.animation = animation;
						move.puppet = puppet;
						actions.Add(move);
						break;
					}
				case ATTACK_TYPE.RIGHT:
					{
						SlashRight move = ScriptableObject.CreateInstance<SlashRight>();
						move.animation = animation;
						move.puppet = puppet;
						actions.Add(move);
						break;
					}
				case ATTACK_TYPE.TOP:
					{
						SlashTop move = ScriptableObject.CreateInstance<SlashTop>();
						move.animation = animation;
						move.puppet = puppet;
						actions.Add(move);
						break;
					}
				case ATTACK_TYPE.THRUST:
					{
						ThrustForward move = ScriptableObject.CreateInstance<ThrustForward>();
						move.animation = animation;
						move.puppet = puppet;
						actions.Add(move);
						break;
					}
				case ATTACK_TYPE.GUARD_LEFT:
					{
						GuardLeft move = ScriptableObject.CreateInstance<GuardLeft>();
						move.animation = animation;
						move.puppet = puppet;
						move.GuardTimerMax = guardTimer;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.GUARD_RIGHT:
					{
						GuardRight move = ScriptableObject.CreateInstance<GuardRight>();
						move.animation = animation;
						move.puppet = puppet;
						move.GuardTimerMax = guardTimer;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.GUARD_TOP:
					{
						GuardTop move = ScriptableObject.CreateInstance<GuardTop>();
						move.animation = animation;
						move.puppet = puppet;
						move.GuardTimerMax = guardTimer;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.REACTIVE_GUARD:
					{
						ReactiveGuard move = ScriptableObject.CreateInstance<ReactiveGuard>();
						move.animation = animation;
						move.puppet = puppet;
						move.GuardTimerMax = guardTimer;
						move.playerPuppet = player.GetComponent<PuppetScript>();
						move.dances = new List<string>();
						move.dances.Add("Twerk");
						move.dances.Add("Gangnam Style");
						move.dances.Add("Robot");
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.WINDOW_SHORT:
					{
						WindowOfOpportunity move = ScriptableObject.CreateInstance<WindowOfOpportunity>();
						move.animation = animation;
						move.puppet = puppet;
						move.TimerMax = shortTimer;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.WINDOW_MEDIUM:
					{
						WindowOfOpportunity move = ScriptableObject.CreateInstance<WindowOfOpportunity>();
						move.animation = animation;
						move.puppet = puppet;
						move.TimerMax = mediumTimer;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.WINDOW_LONG:
					{
						WindowOfOpportunity move = ScriptableObject.CreateInstance<WindowOfOpportunity>();
						move.animation = animation;
						move.puppet = puppet;
						move.TimerMax = longTimer;
						actions.Add(move);
					}
					break;
			}
		}

	}

	void SeekPlayer()
	{
		if (player && agent && puppet)
		{
			Vector3 playerDirection = player.transform.position - gameObject.transform.position;
			playerDirection.y = 0.0f;
			Vector3 AIForward = transform.forward;
			float angleToPlayer = Vector3.Angle(AIForward.normalized, playerDirection.normalized);

			Debug.Log(angleToPlayer + " Seek Player");

			if (angleToPlayer > 5.0f &&
				!currentAction.isBehaving() &&
				(gameObject.transform.position - player.transform.position).magnitude < StoppingDistance &&
				(gameObject.transform.position - player.transform.position).magnitude > 0.78f)
			{

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

			else if ((gameObject.transform.position - player.transform.position).magnitude > StoppingDistance
				&& !currentAction.isBehaving())
			{
				// Move the enemy towards the player
				agent.SetDestination(player.transform.position);
				if (!animation.IsPlaying("Walk Forward"))
				{
					puppet.ChangeState(PuppetScript.State.MOVING);
					animation.Play("Walk Forward");
				}

				//if (agent.remainingDistance < agent.stoppingDistance + 0.5f)
				//{
				//	//inRange = true;
				//	animation.Play("Idle");
				//}
			}
		}
	}

	void TurnToPlayer()
	{
		Vector3 playerDirection = player.transform.position - gameObject.transform.position;
		playerDirection.y = 0.0f;
		Vector3 AIForward = transform.forward;
		float angleToPlayer = Vector3.Angle(AIForward.normalized, playerDirection.normalized);

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

	void PhaseOne(float angleToPlayer)
	{
		if (angleToPlayer > 5.0f || (gameObject.transform.position - player.transform.position).magnitude > StoppingDistance)
		{
			inRange = false;
		}
		else
		{
			inRange = true;
			agent.Stop();
		}

		if (inRange || currentAction.isBehaving())
		{
			if (currentAction.Execute() == COMPLETION_STATE.COMPLETE)
			{
				// Increment the action
				nextAction++;

				if (nextAction >= phaseOne.Count)
					nextAction = 0;

				currentAction = phaseOne[nextAction];
			}
		}

		else
		{
			SeekPlayer();
		}

		if (puppet.curTallys <= 2)
		{
			if (minion != null)
			{
				GameObject thisGuy = (GameObject)(GameObject.Instantiate(minion, minionSpawnLocation.transform.position, Quaternion.identity));
				minionPuppet = thisGuy.GetComponent<PuppetScript>();
				minionSummoned = true;
				agent.SetDestination(retreatPoint.transform.position);
			}
			phase = PHASE.TWO;
		}
	}

	void PhaseTwo(float angleToPlayer)
	{
		if (angleToPlayer > 5.0f || (gameObject.transform.position - player.transform.position).magnitude > StoppingDistance)
		{
			inRange = false;
		}
		else
		{
			inRange = true;
			agent.Stop();
		}

		if (minionSummoned && !minionDead)
		{
			
			Debug.Log(minionPuppet.curState);
			if (minionPuppet.curState == PuppetScript.State.DEAD)
			{
				minionDead = true;
				nextAction = 0;
				currentAction = phaseTwo[nextAction];
			}

			if(agent.remainingDistance > 0.0f)
			{
				agent.SetDestination(retreatPoint.transform.position);
			}

			if (agent.remainingDistance == 0.0f)
			{
				if (inRange)
					protectiveAction.Execute();
				else
					TurnToPlayer();
			}

		}

		else
		{
			if (inRange || currentAction.isBehaving())
			{
				if (currentAction.Execute() == COMPLETION_STATE.COMPLETE)
				{
					// Increment the action
					nextAction++;

					if (nextAction >= phaseTwo.Count)
						nextAction = 0;

					currentAction = phaseTwo[nextAction];
				}
			}

			else
			{
				SeekPlayer();
			}
		}
	}

}
