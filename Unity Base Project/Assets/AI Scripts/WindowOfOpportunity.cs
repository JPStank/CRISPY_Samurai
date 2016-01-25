using UnityEngine;
using System.Collections;

public class WindowOfOpportunity : Action 
{
    public float TimerMax;
    private GameObject windowTell = null;

    public GameObject WindowTell
    {
        get { return windowTell; }
        set { windowTell = value; }
    }
    float timer = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override COMPLETION_STATE Execute()
    {
		if (!behaving)
		{
			behaving = true;
			puppet.ChangeState(PuppetScript.State.IDLE);
            if (windowTell)
            {
                GameObject tell = (GameObject)Instantiate(windowTell, puppet.gameObject.transform.position, Quaternion.identity);
                tell.transform.SetParent(puppet.gameObject.transform);
                tell.GetComponent<WindowOpenEffectBehavior>().GoTime(TimerMax);
            }
		}

        //if(puppet.curState != PuppetScript.State.FLINCH)
        timer += Time.deltaTime;
        if(timer >= TimerMax && puppet.curState != PuppetScript.State.FLINCH)
        {
            timer = 0;
			behaving = false;
            return COMPLETION_STATE.COMPLETE;
        }
        return COMPLETION_STATE.IN_PROGRESS;
    }
}
