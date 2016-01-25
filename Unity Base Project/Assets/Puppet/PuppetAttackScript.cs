using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuppetAttackScript : MonoBehaviour {

    //public float AtkTmrCur;

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
        //AtkTmrCur = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

		if (Owner.IsAttackState() && animation.isPlaying == false)
		{
			Owner.ChangeState(PuppetScript.State.IDLE);
		}

		if (animation.IsPlaying("Right Slash"))
		{
			animation["Right Slash"].speed = attackSpeed;
		}
		else if (animation.IsPlaying("Down Slash"))
		{
			animation["Down Slash"].speed = attackSpeed;
		}
		else if (animation.IsPlaying("Left Slash"))
		{
			animation["Left Slash"].speed = attackSpeed;
		}
		else if (animation.IsPlaying("Stab"))
		{
			animation["Stab"].speed = attackSpeed;
		}
		else if (animation.IsPlaying("Kick"))
		{
			animation["Kick"].speed = attackSpeed;
		}

		//UpdateTmrs(ref AtkTmrCur);

	}

	//void UpdateTmrs(ref float _cur)
	//{
	//	if (_cur > 0.0f)
	//	{
	//		_cur -= Time.deltaTime;
	//		if (_cur <= 0.0f)
	//		{
	//			_cur = 0.0f;
	//			Owner.ChangeState(PuppetScript.State.IDLE);
	//		}
	//	}
	//}

    // Vertical Slash function
    // returns -1 on failure
    // returns 1 on success
    public int SlashVert(PuppetScript _sender)
    {
        //Trigger animation
		animation["Down Slash"].speed = attackSpeed;
        animation.Play("Down Slash");
        //AtkTmrCur = animation["Down Slash"].length / animation["Down Slash"].speed;

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
        //AtkTmrCur = animation["Right Slash"].length / animation["Right Slash"].speed;

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
        //AtkTmrCur = animation["Left Slash"].length / animation["Left Slash"].speed;

		// move us closer to the enemy please!
		Owner.rigidbody.AddForce(Owner.transform.forward * 50000.0f);

        return 1;
    }

    // Thrust function
    // returns -1 on failure
    // returns 1 on success
    public int Thrust(PuppetScript _sender)
    {
		animation["Stab"].speed = attackSpeed;

		animation.Play("Stab");
		//AtkTmrCur = animation["Stab"].length / animation["Left Slash"].speed;

		// move us closer to the enemy please!
		Owner.rigidbody.AddForce(Owner.transform.forward * 100000.0f);

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
