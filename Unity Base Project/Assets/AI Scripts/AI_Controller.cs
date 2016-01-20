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

    public float shortTimer = 0.0f, mediumTimer = 0.0f, longTimer = 0.0f, guardTimer = 1.0f;

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
        if (patrolling)
            agent.stoppingDistance = 0.0f;

        currentAction = actions[0];
    }

    // Update is called once per frame
    void Update()
    {

        if (puppet.curState != PuppetScript.State.DEAD)
        {
            if (patrolling)
            {
                if ((player.gameObject.transform.position - gameObject.transform.position).magnitude < noticeDistance)
                {
                    patrolling = false;
					attackID = monitor.AddEnemy(this);
                    return;
                }

                if(!waiting)
                {
                    Debug.Log(gameObject.transform.position + " " + patrolPoints[currentPatrolPoint]);
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
                    if(waitTimer >= waitTimerMax)
                    {
                        waiting = false;
                    }
                }

            }

            else
            {
                // Checking player dir and distance
                Vector3 playerDirection = player.transform.position - gameObject.transform.position;
				playerDirection.y = 0.0f;
                Vector3 AIForward = transform.forward;
                float angleToPlayer = Vector3.Angle(AIForward.normalized, playerDirection.normalized);

				// Checking other enemies to move away
				//Vector3 right = transform.right;
				//foreach (AI_Controller enemy in monitor.enemies)
				//{
				//	Vector3 toEnemy = enemy.gameObject.transform.position - transform.position;
				//	if(toEnemy.magnitude < 2.0f)
				//	{
				//		Transform ownTransform = transform;
				//		if (Vector3.Dot(right, toEnemy) < 0)
				//		{
				//			ownTransform.RotateAround(player.transform.position, Vector3.up, -1.0f);
				//			agent.SetDestination(ownTransform.position);
				//		}
				//
				//		//else
				//		//{
				//		//	ownTransform.RotateAround(player.transform.position, Vector3.up, 1.0f);
				//		//	agent.SetDestination(ownTransform.position);
				//		//}
				//		return;
				//
				//	}
				//}

                if (angleToPlayer > 5.0f || (gameObject.transform.position - player.transform.position).magnitude > agent.stoppingDistance)
                {
                    inRange = false;
                }
                else
                {
                    inRange = true;
					agent.Stop();
                }

				if ((inRange || currentAction.isBehaving()) && (monitor.CanIAttack(attackID)/* || currentAction.type == Action.TYPE.WINDOW*/))
                {
                    if (currentAction.Execute() == COMPLETION_STATE.COMPLETE)
                    {
						if (currentAction.type == Action.TYPE.WINDOW/* && monitor.CanIAttack(attackID)*/)
							monitor.AttackDone();
                        // Increment the action
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

                else
                {
                    SeekPlayer();
                }
            }
        }
		else
		{

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

            Debug.Log(angleToPlayer + " " + (gameObject.transform.position - player.transform.position).magnitude + " " + 
				" Seek Player");

            if (angleToPlayer > 5.0f &&
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
                agent.stoppingDistance = 2.7f;
                agent.SetDestination(player.transform.position);
                if (!animation.IsPlaying("Walk Forward"))
                {
                    puppet.ChangeState(PuppetScript.State.MOVING);
                    animation.Play("Walk Forward");
                }

                if (agent.remainingDistance < agent.stoppingDistance + 0.5f)
                {
                    //inRange = true;
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

	public void ConnectToMonitor(AI_Monitor connectME)
	{
		if(monitor == null)
			monitor = connectME;
		if(!patrolling)
		{
			attackID = monitor.AddEnemy(this);
			agent.avoidancePriority = attackID;
		}
	}

	public void FindMonitor()
	{
		GameObject bob = GameObject.FindGameObjectWithTag("Enemy Monitor");
		monitor = bob.GetComponent<AI_Monitor>();
		if (monitor != null)
			monitor.ReadyToConnect();
		
	}

	public void OnDestroy()
	{
		monitor.RemoveEnemy(attackID);
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
