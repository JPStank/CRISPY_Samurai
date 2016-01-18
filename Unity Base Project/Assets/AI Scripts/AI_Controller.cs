﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AI_Controller : MonoBehaviour
{
    public enum ATTACK_TYPE { LEFT, RIGHT, TOP, THRUST, GUARD_LEFT, GUARD_RIGHT, GUARD_TOP, REACTIVE_GUARD, WINDOW_SHORT, WINDOW_MEDIUM, WINDOW_LONG };

    NavMeshAgent agent;
    GameObject player;
    PuppetScript puppet;

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

    public float shortTimer = 0.0f, mediumTimer = 0.0f, longTimer = 0.0f, guardTimer = 1.0f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        puppet = GetComponent<PuppetScript>();
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
        if (patrolling)
            agent.stoppingDistance = 0.0f;

        currentAction = actions[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (patrolling)
            {
                if ((player.gameObject.transform.position - gameObject.transform.position).magnitude < noticeDistance)
                {
                    patrolling = false;
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
                Vector3 AIForward = transform.forward;
                float angleToPlayer = Vector3.Angle(AIForward.normalized, playerDirection.normalized);

                if (angleToPlayer > 15.0f || (gameObject.transform.position - player.transform.position).magnitude > agent.stoppingDistance)
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
}
