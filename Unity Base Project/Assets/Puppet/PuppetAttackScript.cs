using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuppetAttackScript : MonoBehaviour {

    public float AtkTmrCur;

    private PuppetScript Owner;
    //private Animator Animetor;
    //private float AtkTmrMax;

	public float attackSpeed = 1.0f;

	// Use this for initialization
	void Start () 
	{

	}

    // Because we dont know what orders the Start()s are called in.
    public void Initialize(PuppetScript _sender)
    {
        Owner = _sender;
        //Animetor = _sender.Animetor;

        //AtkTmrMax = _sender.AtkTmrMax;
        AtkTmrCur = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

		UpdateTmrs(ref AtkTmrCur);

	}

    void UpdateTmrs(ref float _cur)
    {
        if (_cur > 0.0f)
        {
            _cur -= Time.deltaTime;
            if (_cur <= 0.0f)
            {
                _cur = 0.0f;
                Owner.ChangeState(PuppetScript.State.IDLE);
            }
        }
    }

    // Vertical Slash function
    // returns -1 on failure
    // returns 1 on success
    public int SlashVert(PuppetScript _sender)
    {
        //Trigger animation
		animation["Down Slash"].speed = attackSpeed;
        animation.Play("Down Slash");
		AtkTmrCur = _sender.animTimers["SlashVert"] / animation["Down Slash"].speed;

		// move us closer to the enemy please!
		Owner.rigidbody.AddForce(Owner.transform.forward * 50000.0f);

        return 1;
    }

    // Horizontal Left-To-Right Slash function
    // returns -1 on failure
    // returns 1 on success
    public int SlashLTR(PuppetScript _sender)
    {
		animation["Right Slash"].speed = attackSpeed;

        animation.Play("Right Slash");
		AtkTmrCur = _sender.animTimers["SlashLTR"] / animation["Right Slash"].speed;

		// move us closer to the enemy please!
		Owner.rigidbody.AddForce(Owner.transform.forward * 50000.0f);

        return 1;
    }

    // Horizontal Right-To-Left Slash function
    // returns -1 on failure
    // returns 1 on success
    public int SlashRTL(PuppetScript _sender)
    {
		animation["Left Slash"].speed = attackSpeed;

        animation.Play("Left Slash");
		AtkTmrCur = _sender.animTimers["SlashRTL"] / animation["Left Slash"].speed;

		// move us closer to the enemy please!
		Owner.rigidbody.AddForce(Owner.transform.forward * 50000.0f);

        return 1;
    }

    // Thrust function
    // returns -1 on failure
    // returns 1 on success
    public int Thrust(PuppetScript _sender)
    {
        animation.Play("Down Slash");
        //AtkTmrCur = animation.animation.clip.length;
        return 1;
    }

    // Kick function
    // returns -1 on failure
    // returns 1 on success
    public int Kick(PuppetScript _sender)
    {
        animation.Play("Down Slash");
        //AtkTmrCur = Animetor.animation.clip.length;
        return 1;
    }

}
