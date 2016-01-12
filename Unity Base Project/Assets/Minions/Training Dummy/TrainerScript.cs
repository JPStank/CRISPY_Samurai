using UnityEngine;
using System.Collections;

public enum TRAINER_TYPE { ATTACK_TOP, ATTACK_LEFT, ATTACK_RIGHT, GUARD_TOP, GUARD_LEFT, GUARD_RIGHT };
public class TrainerScript : MonoBehaviour 
{
    public BehaviourTree behaviourTree;
    PuppetScript puppet;
    bool changeFlag;
    public TRAINER_TYPE trainerType;
	// Use this for initialization
	void Start () 
    {
        puppet = gameObject.GetComponent<PuppetScript>();
        behaviourTree = gameObject.GetComponent<BehaviourTree>();
        switch (trainerType)
        {
            case TRAINER_TYPE.ATTACK_TOP:
                behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.SLASH_TOP));
                break;
            case TRAINER_TYPE.ATTACK_LEFT:
                behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.SLASH_LEFT));
                break;
            case TRAINER_TYPE.ATTACK_RIGHT:
                behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.SLASH_RIGHT));
                break;
            case TRAINER_TYPE.GUARD_LEFT:
                behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.GUARD_LEFT, 1.5f));
                break;
        }

        behaviourTree.AddBehaviour(new AIBehaviour(AI_STATE.WINDOW_OF_OPPORTUNITY, 1.5f));
	}
	
	// Update is called once per frame
	void Update () 
    {
        behaviourTree.IterateTree();
	}
}
