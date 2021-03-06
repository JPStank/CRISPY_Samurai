﻿using UnityEngine;
using System.Collections;

public class PuppetGuardScript : MonoBehaviour {

    public float GrdTmrCur;

    private PuppetScript Owner;
    private PuppetScript.State lastGrdDir;
    private float GrdTmrMax;
	// Use this for initialization
	void Start () {
	
	}
    // Because we dont know what orders the Start()s are called in.
    public void Initialize(PuppetScript _sender)
    {
        Owner = _sender;

        GrdTmrMax = _sender.GrdTmrMax;
        GrdTmrCur = 0.0f;
    }
	
	
	// Update is called once per frame
	void Update () {


        UpdateTmrs(ref GrdTmrCur);
    }

    void UpdateTmrs(ref float _cur)
    {
        if (_cur > 0.0f)
        {
            _cur -= Time.deltaTime;
            if (_cur <= 0.0f)
            {
                if (Owner.curState == PuppetScript.State.PARRY)
                {
                    _cur = GrdTmrMax;
                    //Owner.ChangeState(lastGrdDir);
                    Owner.lastState = Owner.curState;
                    Owner.curState = lastGrdDir;
                }
                else 
                {
                    _cur = 0.0f;
                    Owner.ChangeState(PuppetScript.State.IDLE);
                }
            }
        }
    }
    // Guard Upwards function
    // returns -1 on failure
    // returns 1 on success
    public int GuardUpwards(PuppetScript _sender)
    {
        GrdTmrCur = GrdTmrMax;
        lastGrdDir = PuppetScript.State.GRD_TOP;
        animation.CrossFade("Block Up", 0.1f);
        return 1;
    }

    // Guard Left function
    // returns -1 on failure
    // returns 1 on success
    public int GuardLeft(PuppetScript _sender)
    {
        GrdTmrCur = GrdTmrMax;
        lastGrdDir = PuppetScript.State.GRD_LEFT;
        animation.CrossFade("Block Left", 0.1f);
        return 1;
    }

    // Guard Right function
    // returns -1 on failure
    // returns 1 on success
    public int GuardRight(PuppetScript _sender)
    {
        GrdTmrCur = GrdTmrMax;
        lastGrdDir = PuppetScript.State.GRD_RIGHT;
        animation.CrossFade("Block Right", 0.1f);
        return 1;
    }


}
