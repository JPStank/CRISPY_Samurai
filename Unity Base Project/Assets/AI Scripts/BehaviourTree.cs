using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum COMPLETION_STATE { NOT_STARTED, IN_PROGRESS, COMPLETE, INVALID };

public class BehaviourTree : MonoBehaviour
{
    public NavMeshAgent agent;
    public PuppetScript puppet;
    NavMeshPath path;
    List<AIBehaviour> behaviours;
    int behaviourCount;
    int currentBehaviour;
    bool currentlyBehaving = false;
    bool behaviourStarted = false;
    float behaviourTimer;
    PuppetScript playerPuppet;
    GameObject player;

    public float StoppingDistance()
    {
        return agent.stoppingDistance;
    }

    public bool isBehaving()
    {
        return currentlyBehaving;
    }

    void Start()
    {
        currentBehaviour = 0;
        puppet = gameObject.GetComponent<PuppetScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = gameObject.GetComponent<NavMeshAgent>();
        FuckYouUnity();
        behaviourTimer = 0.0f;
        //agent.updatePosition = false;
        //agent.updateRotation = false;
    }

    public void FuckYouUnity()
    {
        if (behaviours == null)
        {
            behaviours = new List<AIBehaviour>();
            behaviourCount = 0;
        }
    }
    //public BehaviourTree(PuppetScript _puppet)
    //{
    //
    //}

    public COMPLETION_STATE IterateTree()
    {
        //Debug.Log(behaviours[currentBehaviour].state + " " + behaviours.Count);
        if (behaviours[currentBehaviour].complete == COMPLETION_STATE.COMPLETE)
        {
            if(behaviours[currentBehaviour].iterationCount > 0)
            {
                behaviours[currentBehaviour].iterationCount--;
                if(behaviours[currentBehaviour].iterationCount == 0)
                {
                    behaviours.RemoveAt(currentBehaviour);
                    behaviourCount--;
                    currentBehaviour--;
                }
            }
            currentBehaviour++;
        }

        if(currentBehaviour >= behaviourCount)
        {
            ResetTree();
        }
        
        if (behaviours[currentBehaviour].complete != COMPLETION_STATE.COMPLETE)
        {
            Vector3 direction = Vector3.zero;
            switch (behaviours[currentBehaviour].state)
            {
                case AI_STATE.IDLE:
                    {

                    }
                    break;
                case AI_STATE.PATROL:
                    {
                        Debug.Log(gameObject.transform.position + " " + behaviours[currentBehaviour].positionData);
                        agent.stoppingDistance = behaviours[currentBehaviour].floatData;
                        agent.SetDestination(behaviours[currentBehaviour].positionData);
                        if(!animation.IsPlaying("Walk Forward"))
                        {
                            puppet.ChangeState(PuppetScript.State.MOVING);
                            animation.Play("Walk Forward");
                            currentlyBehaving = true;
                        }
                        if ((gameObject.transform.position - behaviours[currentBehaviour].positionData).magnitude < agent.stoppingDistance + 0.5f)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            animation.Play("Idle");
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.MOVE_TO_PLAYER:
                    {
                        agent.stoppingDistance = behaviours[currentBehaviour].floatData;
                        agent.SetDestination(player.transform.position);
                        if (!animation.IsPlaying("Walk Forward"))
                        {
                            puppet.ChangeState(PuppetScript.State.MOVING);
                            animation.Play("Walk Forward");
                            currentlyBehaving = true;
                        }

                        if (agent.remainingDistance < agent.stoppingDistance + 0.5f)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            animation.Play("Idle");
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;

                    }
                    break;
                case AI_STATE.TURN_TO_PLAYER:
                    {
                        currentlyBehaving = true;
                        agent.updateRotation = false;
                        agent.updatePosition = false;
                        Vector3 playerPosition = player.transform.position;// behaviours[currentBehaviour].positionData;
                        Vector3 AIPosition = gameObject.transform.position;
                        playerPosition.y = AIPosition.y;
                        Vector3 toPlayer = playerPosition - AIPosition;
                        toPlayer.Normalize();
                        transform.forward = Vector3.RotateTowards(transform.forward, toPlayer, behaviours[currentBehaviour].floatData * Time.deltaTime, 0.0f);
                        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);
                        behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                        agent.updatePosition = true;
                        agent.updateRotation = true;
                        currentlyBehaving = false;
                        if (angleToPlayer < 15.0f/*transform.forward.normalized == toPlayer.normalized*/)
                        {
                            PurgeTempBehaviours();
                        }
                    }
                    break;
                case AI_STATE.GUARD_LEFT:
                    {
                        if(!currentlyBehaving)
                        {
                            puppet.GuardLeft();
                            currentlyBehaving = true;
                        }

                        behaviourTimer += Time.deltaTime;
                        if(behaviourTimer >= behaviours[currentBehaviour].floatData)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                            behaviourTimer = 0.0f;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.GUARD_RIGHT:
                    {
                        if(!currentlyBehaving)
                        {
                            puppet.GuardRight();
                            currentlyBehaving = true;
                        }

                        behaviourTimer += Time.deltaTime;
                        if (behaviourTimer >= behaviours[currentBehaviour].floatData)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                            behaviourTimer = 0.0f;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.GUARD_TOP:
                    {
                        if(!currentlyBehaving)
                        {
                            puppet.GuardUpwards();
                            currentlyBehaving = true;
                        }

                        behaviourTimer += Time.deltaTime;
                        if (behaviourTimer >= behaviours[currentBehaviour].floatData)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                            behaviourTimer = 0.0f;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.SLASH_TOP:
                    {
                        if (!currentlyBehaving)
                        {
                            puppet.SlashVert();
                            currentlyBehaving = true;
                        }

                        if(!animation.IsPlaying("Down Slash"))
                        {
                            Debug.Log("Completed Vertical Slash");
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.SLASH_LEFT:
                    {
                        if(!currentlyBehaving)
                        {
                            puppet.SlashRTL();
                            currentlyBehaving = true;
                        }

                        if (!animation.IsPlaying("Left Slash"))
                        {
                            Debug.Log("Completed Left Slash");
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.SLASH_RIGHT:
                    {
                        if(!currentlyBehaving)
                        {
                            puppet.SlashLTR();
                            currentlyBehaving = true;
                        }

                        if (!animation.IsPlaying("Right Slash"))
                        {
                            Debug.Log("Completed Right Slash");
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.THRUST:
                    {
                        if(!currentlyBehaving)
                        {
                            puppet.Thrust();
                            currentlyBehaving = true;
                        }

                        if (!animation.isPlaying)
                        {
                            Debug.Log("Completed Thrust");
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.WINDOW_OF_OPPORTUNITY:
                    {
                        if (!currentlyBehaving)
                        {
                            puppet.ChangeState(PuppetScript.State.IDLE);
                            currentlyBehaving = true;
                        }

                        behaviourTimer += Time.deltaTime;
                        if (behaviourTimer >= behaviours[currentBehaviour].floatData)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                            behaviourTimer = 0.0f;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
            }

        }
        return behaviours[currentBehaviour].complete;
    }

    public bool AddBehaviour(AIBehaviour _behaviour)
    {
        FuckYouUnity();
        behaviours.Add(_behaviour);
        behaviours[behaviourCount].complete = COMPLETION_STATE.NOT_STARTED;
        behaviourCount++;

        return true;
    }

    public bool AddBehaviourNow(AIBehaviour _behaviour)
    {
        FuckYouUnity();
        _behaviour.SetIterationCount(1);
        behaviours.Insert(currentBehaviour, _behaviour);
        behaviourCount++;
        return true;

    }

    public void ClearTree()
    {
        behaviours.Clear();
        currentBehaviour = 0;
        behaviourCount = 0;

    }

    void ResetTree()
    {
        for (int index = 0; index < behaviours.Count; index++)
        {
            behaviours[index].complete = COMPLETION_STATE.NOT_STARTED;
        }
        currentBehaviour = 0;
    }

    void PurgeTempBehaviours()
    {
        while (behaviours[currentBehaviour].iterationCount >= 0)
        {
            behaviours.RemoveAt(currentBehaviour);
            behaviourCount--;
        }
    }
}