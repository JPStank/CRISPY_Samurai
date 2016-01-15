﻿using UnityEngine;
using System.Collections;

public class WindowOfOpportunity : Action 
{
    public float TimerMax;
    float timer = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override COMPLETION_STATE Execute()
    {
        timer += Time.deltaTime;
        if(timer >= TimerMax)
        {
            timer = 0;
            return COMPLETION_STATE.COMPLETE;
        }
        return COMPLETION_STATE.IN_PROGRESS;
    }
}
