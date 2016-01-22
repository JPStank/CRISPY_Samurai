using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AI_Controller : MonoBehaviour
{
	public enum ATTACK_TYPE { LEFT, RIGHT, TOP, THRUST, GUARD_LEFT, GUARD_RIGHT, GUARD_TOP, REACTIVE_GUARD, WINDOW_SHORT, WINDOW_MEDIUM, WINDOW_LONG };

	NavMeshAgent agent;
	GameObject player;
	PuppetScript puppet;
	AI_Monitor monitor;

	//public List<Action> actions;
	[Header("Patrol Stuff")]
	public bool patrolling;
	bool waiting;
	public float waitTimerMax = 2.0f;
	float waitTimer = 0.0f;
	Vector3[] patrolPoints;
	int currentPatrolPoint = 0;
	bool alive = true;
	[Header("Action Stuff")]
	public float noticeDistance = 20.0f;
	public List<ATTACK_TYPE> attacks;
	public List<Action> actions;
	public bool inRange = false;
	public Action currentAction;
	int nextAction;
	public int attackID;
	public float stoppingDistance = 2.7f;
	bool movingToDistance = false;
	bool myTurn = false;
	float tooCloseTimer = 0.0f;
	Vector3 destination;
	public float shortTimer = 0.0f, mediumTimer = 0.0f, longTimer = 0.0f, guardTimer = 1.0f;
	bool IAmDead = false;
	public int DeadLayer = 10;

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		agent = GetComponent<NavMeshAgent>();
		puppet = GetComponent<PuppetScript>();
		FindMonitor();
		nextAction = 1;
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
						move.type = Action.TYPE.SLASH;
						actions.Add(move);
						break;
					}
				case ATTACK_TYPE.RIGHT:
					{
						SlashRight move = ScriptableObject.CreateInstance<SlashRight>();
						move.animation = animation;
						move.puppet = puppet;
						move.type = Action.TYPE.SLASH;
						actions.Add(move);
						break;
					}
				case ATTACK_TYPE.TOP:
					{
						SlashTop move = ScriptableObject.CreateInstance<SlashTop>();
						move.animation = animation;
						move.puppet = puppet;
						move.type = Action.TYPE.SLASH;
						actions.Add(move);
						break;
					}
				case ATTACK_TYPE.THRUST:
					{
						ThrustForward move = ScriptableObject.CreateInstance<ThrustForward>();
						move.animation = animation;
						move.puppet = puppet;
						move.type = Action.TYPE.SLASH;
						actions.Add(move);
						break;
					}
				case ATTACK_TYPE.GUARD_LEFT:
					{
						GuardLeft move = ScriptableObject.CreateInstance<GuardLeft>();
						move.animation = animation;
						move.puppet = puppet;
						move.GuardTimerMax = guardTimer;
						move.type = Action.TYPE.GUARD;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.GUARD_RIGHT:
					{
						GuardRight move = ScriptableObject.CreateInstance<GuardRight>();
						move.animation = animation;
						move.puppet = puppet;
						move.GuardTimerMax = guardTimer;
						move.type = Action.TYPE.GUARD;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.GUARD_TOP:
					{
						GuardTop move = ScriptableObject.CreateInstance<GuardTop>();
						move.animation = animation;
						move.puppet = puppet;
						move.GuardTimerMax = guardTimer;
						move.type = Action.TYPE.GUARD;
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
						move.type = Action.TYPE.GUARD;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.WINDOW_SHORT:
					{
						WindowOfOpportunity move = ScriptableObject.CreateInstance<WindowOfOpportunity>();
						move.animation = animation;
						move.puppet = puppet;
						move.TimerMax = shortTimer;
						move.type = Action.TYPE.WINDOW;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.WINDOW_MEDIUM:
					{
						WindowOfOpportunity move = ScriptableObject.CreateInstance<WindowOfOpportunity>();
						move.animation = animation;
						move.puppet = puppet;
						move.TimerMax = mediumTimer;
						move.type = Action.TYPE.WINDOW;
						actions.Add(move);
					}
					break;
				case ATTACK_TYPE.WINDOW_LONG:
					{
						WindowOfOpportunity move = ScriptableObject.CreateInstance<WindowOfOpportunity>();
						move.animation = animation;
						move.puppet = puppet;
						move.TimerMax = longTimer;
						move.type = Action.TYPE.WINDOW;
						actions.Add(move);
					}
					break;
			}
		}
		agent.stoppingDistance = 0.0f;

		currentAction = actions[0];
	}

	// Update is called once per frame
	void Update()
	{
		if(!IAmDead && gameObject.layer == DeadLayer)
		{
			IAmDead = true;
			monitor.RemoveEnemy(attackID);
		}

		if (puppet.curState != PuppetScript.State.DEAD)
		{
			#region Patrolling
			if (patrolling)
			{
				if ((player.gameObject.transform.position - gameObject.transform.position).magnitude < noticeDistance)
				{
					patrolling = false;
					attackID = monitor.AddEnemy(this);
					return;
				}

				if (!waiting)
				{
					//Debug.Log(gameObject.transform.position + " " + patrolPoints[currentPatrolPoint]);
					agent.SetDestination(patrolPoints[currentPatrolPoint]);
					if (!animation.IsPlaying("Walk Forward"))
					{
						puppet.ChangeState(PuppetScript.State.MOVING);
						animation.Play("Walk Forward");
					}
					if (agent.remainingDistance == agent.stoppingDistance)
					{
						animation.Play("Idle");
						currentPatrolPoint++;
						if (currentPatrolPoint >= patrolPoints.Length)
							currentPatrolPoint = 0;

						waiting = true;
					}
				}
				else
				{
					waitTimer += Time.deltaTime;
					if (waitTimer >= waitTimerMax)
					{
						waiting = false;
					}
				}

			}
			#endregion

			#region AttackingThePlayer
			else
			{
				//Debug.Log("Attacking");
				// Checking player dir and distance
				// and initializing variables for later use
				Vector3 playerDirection = player.transform.position - gameObject.transform.position;
				playerDirection.y = 0.0f;
				Vector3 AIForward = transform.forward;
				float angleToPlayer = Vector3.Angle(AIForward.normalized, playerDirection.normalized);
				myTurn = monitor.CanIAttack(attackID);
				if (myTurn)
					movingToDistance = false;

				

				#region myTurn
				// If it is my turn
				if (myTurn)
				{
					//Debug.Log("My Turn");
					// This check will determine if I am in range of the player
					if (angleToPlayer > 5.0f || (gameObject.transform.position - player.transform.position).magnitude > stoppingDistance)
					{
						inRange = false;
					}
					else
					{
						inRange = true;
						agent.Stop();
					}

					// If I am in range of the player or I am currently in the middle of an attack
					if (inRange || currentAction.isBehaving())
					{
						// I will execute an attack until it is complete
						if (currentAction.Execute() == COMPLETION_STATE.COMPLETE)
						{
							// If the finished action is a window of opportunity, 
							// I will let someone else have a turn at the player
							if (currentAction.type == Action.TYPE.WINDOW)
								monitor.AttackDone();

							// I will then try to perform the next action in my sequence
							if (actions.Count > 1)
							{
								currentAction = actions[nextAction];
								nextAction++;

								if (nextAction >= actions.Count)
								{
									nextAction = 0;
								}
							}
						}
					}

					// If I am not in range
					else
					{
						// I will position myself so that I am able to attack the player
						SeekPlayer();
					}
				}
				#endregion

				#region notMyTurn
				// If it is not my turn
				if(!myTurn)
				{
					//Debug.Log("Not My Turn");
					// Maintaining proper distance
					float distanceToMaintain = stoppingDistance * 2;

					// If I am too close or too far and not currently moving to a destination
					if ((playerDirection.magnitude < distanceToMaintain || playerDirection.magnitude > distanceToMaintain * 1.5f) && !movingToDistance)
					{
						// I will pick a destination and move there
						destination = MaintainDistance(distanceToMaintain);
						agent.SetDestination(destination);
						movingToDistance = true;
						animation.Play("Walk Forward");
					}

					// If I am moving to a distance
					if (movingToDistance)
					{
						// Update my timers
						tooCloseTimer += Time.deltaTime;

						if(!animation.IsPlaying("Walk Forward"))
						{
							animation.Play("Walk Forward");
						}

						// If I am too close to the player for too long (try to walk through the player)
						if(playerDirection.magnitude < 0.87f && tooCloseTimer >= 0.5f)
						{
							destination = MaintainDistance(distanceToMaintain);
							tooCloseTimer = 0.0f;
						}

						// I will keep moving there
						agent.SetDestination(destination);

						// If I have reached my destination or I have gone too far
						if (agent.remainingDistance == 0.0f || 
							(playerDirection.magnitude > distanceToMaintain && playerDirection.magnitude < distanceToMaintain + 1.0f))
						{
							movingToDistance = false;
							animation.Play("Idle");
						}
					}

					SeekPlayer();
				}
				#endregion
			}
			#endregion
		}
		else
		{

		}
	}

	void SeekPlayer()
	{
		//Debug.Log("Seeking");
		// Initializing variables
		Vector3 playerDirection = player.transform.position - gameObject.transform.position;
		playerDirection.y = 0.0f;
		Vector3 AIForward = transform.forward;
		float angleToPlayer = Vector3.Angle(AIForward.normalized, playerDirection.normalized);

		// If it is my turn
		if (myTurn)
		{

			//Debug.Log(angleToPlayer + " " + (gameObject.transform.position - player.transform.position).magnitude);

			if (angleToPlayer > 5.0f &&
				!currentAction.isBehaving() &&
				(gameObject.transform.position - player.transform.position).magnitude < agent.stoppingDistance &&
				(gameObject.transform.position - player.transform.position).magnitude > 0.78f)
			{
				//Debug.Log((gameObject.transform.position - player.transform.position).magnitude);

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
				agent.SetDestination(player.transform.position);
				if (!animation.IsPlaying("Walk Forward"))
				{
					puppet.ChangeState(PuppetScript.State.MOVING);
					animation.Play("Walk Forward");
				}
			}
		}

		// If it is not my turn
		if(!myTurn)
		{
			// If I am not looking at the player
			if (angleToPlayer > 5.0f && !currentAction.isBehaving())
			{
				// I will turn towards the player
				agent.updateRotation = false;
				Vector3 playerPosition = player.transform.position;
				Vector3 AIPosition = gameObject.transform.position;
				playerPosition.y = AIPosition.y;
				Vector3 toPlayer = playerPosition - AIPosition;
				toPlayer.Normalize();
				transform.forward = Vector3.RotateTowards(transform.forward, toPlayer, 3.0f * Time.deltaTime, 0.0f);
				angleToPlayer = Vector3.Angle(transform.forward, toPlayer);
				agent.updateRotation = true;
			}
		}
	}

	public void SetRange(bool b)
	{
		inRange = b;
	}

	public void ConnectToMonitor(AI_Monitor connectME)
	{
		if (monitor == null)
			monitor = connectME;
		if (!patrolling
			&& !IAmDead)
		{
			attackID = monitor.AddEnemy(this);
		}
	}

	public void FindMonitor()
	{
		GameObject bob = GameObject.FindGameObjectWithTag("Enemy Monitor");
		monitor = bob.GetComponent<AI_Monitor>();
		if (monitor != null)
			monitor.ReadyToConnect();

	}

	//public void OnDestroy()
	//{
	//	monitor.RemoveEnemy(attackID);
	//}

	Vector3 MaintainDistance(float _distanceToMaintain)
	{
		Vector3 directionToRaycast = Random.insideUnitSphere;
		directionToRaycast.y = 0.0f;
		Ray direction = new Ray(player.transform.position, directionToRaycast);
		Vector3 location = direction.GetPoint(_distanceToMaintain);
		location.y = gameObject.transform.position.y;
		return location;
	}

	#region ductTapeNbubbleGum
	// Sam: Duct tape and bubblegum
	void SetLastLTR()
	{
		//lastAttack = ATTACKS.LTR;
	}

	void SetLastRTL()
	{
		//lastAttack = ATTACKS.RTL;
	}

	void SetLastVert()
	{
		//lastAttack = ATTACKS.VERT;
	}

	void SetLastNone()
	{
		//lastAttack = ATTACKS.NONE;
	}
	#endregion
}
