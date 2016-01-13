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
            currentlyBehaving = true;
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
                            animation.Play("Walk Forward");
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
                            animation.Play("Walk Forward");
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
                        agent.updateRotation = false;
                        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;// behaviours[currentBehaviour].positionData;
                        Vector3 AIPosition = gameObject.transform.position;
                        playerPosition.y = 0;
                        AIPosition.y = 0;
                        Quaternion lookDir = Quaternion.LookRotation(playerPosition - AIPosition);
                        lookDir.y = transform.rotation.y;
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDir, behaviours[currentBehaviour].floatData * Time.deltaTime);
                        if (transform.rotation == lookDir)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.GUARD_LEFT:
                    {
                        puppet.GuardLeft();
                        behaviourTimer += Time.deltaTime;
                        if(behaviourTimer >= behaviours[currentBehaviour].floatData)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.GUARD_RIGHT:
                    {
                        puppet.GuardRight();
                        behaviourTimer += Time.deltaTime;
                        if (behaviourTimer >= behaviours[currentBehaviour].floatData)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.GUARD_TOP:
                    {
                        puppet.GuardUpwards();
                        behaviourTimer += Time.deltaTime;
                        if (behaviourTimer >= behaviours[currentBehaviour].floatData)
                        {
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.COMPLETE;
                            currentlyBehaving = false;
                        }
                        else
                            behaviours[currentBehaviour].complete = COMPLETION_STATE.IN_PROGRESS;
                    }
                    break;
                case AI_STATE.SLASH_TOP:
                    {
                        if (puppet.SlashVert() == 1)
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
                        if (puppet.SlashLTR() == 1)
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
                        if (puppet.SlashRTL() == 1)
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
                        if (puppet.Thrust() == 1)
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
}