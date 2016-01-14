using UnityEngine;
using System.Collections;

enum ENEMY_STATE { PATROL, ATTACK};

public class BasicMinionScript : MonoBehaviour 
{

    public BehaviourTree behaviourTree;
    ENEMY_STATE enemyState;
    public LayerMask playerLayer;
    [SerializeField]
    PuppetScript puppet;
    GameObject player;
    SphereCollider noticeArea;
    public bool patrolling = true;
    bool readyToIterate = false;
    bool windowOfOpportunity = false;

    
    [SerializeField]
    GameObject[] patrolPoints;
    int currentPatrolPoint = 0;

	// Use this for initialization
	void Start () 
    {
        enemyState = ENEMY_STATE.PATROL;
        puppet = GetComponent<PuppetScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        noticeArea = gameObject.GetComponent<SphereCollider>();
        behaviourTree = gameObject.GetComponent<BehaviourTree>();
        behaviourTree.FuckYouUnity(); 
        foreach (GameObject patrolPoint in patrolPoints)
        {
            behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.PATROL, patrolPoint.transform.position, 0.0f));    
        }
        if(patrolPoints.Length == 0)
        {
            behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.PATROL, gameObject.transform.position, 0.0f));
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if(patrolling)
        {
            behaviourTree.IterateTree();
        }
        else
        {
            //Debug.Log(behaviourTree)
            if(readyToIterate)
            {
                if (behaviourTree.IterateTree() == COMPLETION_STATE.COMPLETE)
                    readyToIterate = false;
                return;
            }

            Vector3 playerDirection = player.transform.position - gameObject.transform.position;
            Vector3 AIForward = transform.forward;
            float angleToPlayer = Vector3.Angle(AIForward.normalized, playerDirection.normalized);

            //Debug.Log(angleToPlayer + " Minion Script");

            if (angleToPlayer > 15.0f &&
                !behaviourTree.isBehaving() &&
                (gameObject.transform.position - player.transform.position).magnitude < behaviourTree.StoppingDistance() &&
                (gameObject.transform.position - player.transform.position).magnitude > 0.78f)
            {
                Debug.Log((gameObject.transform.position - player.transform.position).magnitude);
                behaviourTree.AddBehaviourNow(new AIBehaviour(AI_STATE.TURN_TO_PLAYER, 3.0f));
                readyToIterate = true;
            }

            else if ((gameObject.transform.position - player.transform.position).magnitude > behaviourTree.StoppingDistance() && !behaviourTree.isBehaving())
            {
                behaviourTree.AddBehaviourNow(new AIBehaviour(AI_STATE.MOVE_TO_PLAYER, 2.5f));
                readyToIterate = true;
            }

            else if ((gameObject.transform.position - player.transform.position).magnitude < behaviourTree.StoppingDistance() + 0.5f && !behaviourTree.isBehaving())
            {
                readyToIterate = true;
            }
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Weapon")
        {
            patrolling = false;
            noticeArea.enabled = false;
            readyToIterate = true;
            behaviourTree.ClearTree();
            behaviourTree.AddBehaviourNow(new AIBehaviour(AI_STATE.MOVE_TO_PLAYER, 3.0f));
            behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.SLASH_LEFT));
            behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.SLASH_RIGHT));
            behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.SLASH_TOP));
        }
    }
}
