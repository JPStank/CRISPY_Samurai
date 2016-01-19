using UnityEngine;
using System.Collections;

public class PuppetDodgeScript : MonoBehaviour {

    public float DgeTmrCur;

    private PuppetScript Owner;
    //private Animator Animetor;
    private Quaternion orgRot;
    private float DgeTmrMax;
    private float rotationSpeed;
    private float rotationDist;

	// Use this for initialization
	void Start () {
        rotationSpeed = 750.0f;
        rotationDist = 10.0f;
	}

    // Because we dont know what orders the Start()s are called in.
    public void Initialize(PuppetScript _sender)
    {
        Owner = _sender;
        //Animetor = _sender.Animetor;

        DgeTmrMax = _sender.DgeTmrMax;
        DgeTmrCur = 0.0f;
    }
	
	// Update is called once per frame
    void Update()
    {
        if (DgeTmrCur > 0.0f)
        {
            Quaternion curRot = transform.rotation;
            switch (Owner.curState)
            {
                case PuppetScript.State.DGE_FORWARD:
                    {
                        transform.rotation = orgRot;
                        transform.Translate(0.0f, 0.0f, Time.deltaTime * rotationDist);
                        transform.rotation = curRot;
                       //transform.Rotate(Time.deltaTime * rotationSpeed, 0.0f, 0.0f);
                        break;
                    }
                case PuppetScript.State.DGE_LEFT:
                    {
                        //transform.rotation = orgRot;
                        transform.Translate(-Time.deltaTime * rotationDist, 0.0f, 0.0f);
                        //transform.rotation = curRot;
                        //transform.Rotate(0.0f, 0.0f, Time.deltaTime * rotationSpeed);
                        break;
                    }
                case PuppetScript.State.DGE_RIGHT:
                    {
                        //transform.rotation = orgRot;
                        transform.Translate(Time.deltaTime * rotationDist, 0.0f, 0.0f);
                        //transform.rotation = curRot;
                        //transform.Rotate(0.0f, 0.0f, -Time.deltaTime * rotationSpeed);
                        break;
                    }
                case PuppetScript.State.DGE_BACK:
                    {
                        transform.rotation = orgRot;
                        transform.Translate(0.0f, 0.0f, -Time.deltaTime * rotationDist);
                        transform.rotation = curRot;
                        //transform.Rotate(-Time.deltaTime * rotationSpeed, 0.0f, 0.0f);
                        break;
                    }
            }
			if (Owner.rockedOn && Owner.curTarg)
			{
				transform.LookAt(Owner.curTarg.transform);
			}
        }


        UpdateTmrs(ref DgeTmrCur);
	}

    void UpdateTmrs(ref float _cur)
    {
        if (_cur > 0.0f)
        {
            _cur -= Time.deltaTime;
            if (_cur <= 0.0f)
            {
                transform.rotation = orgRot;
                _cur = 0.0f;
                Owner.ChangeState(PuppetScript.State.IDLE);
            }
        }
    }

    // Dodge Forward function
    // returns -1 on failure
    // returns 1 on success
    public int DodgeForward(PuppetScript _sender)
    {
		animation.Play("Dodge Forward");
        DgeTmrCur = DgeTmrMax;
        orgRot = transform.rotation;
        return 1;
    }

    // Dodge Left function
    // returns -1 on failure
    // returns 1 on success
    public int DodgeLeft(PuppetScript _sender)
	{
		animation.Play("Dodge Left");
        DgeTmrCur = DgeTmrMax;
        orgRot = transform.rotation;
        return 1;
    }

    // Dodge Right function
    // returns -1 on failure
    // returns 1 on success
    public int DodgeRight(PuppetScript _sender)
	{
		animation.Play("Dodge Right");
        DgeTmrCur = DgeTmrMax;
        orgRot = transform.rotation;
        return 1;
    }

    // Dodge Backwards function
    // returns -1 on failure
    // returns 1 on success
    public int DodgeBackwards(PuppetScript _sender)
	{
		animation.Play("Dodge Backward");
        DgeTmrCur = DgeTmrMax;
        orgRot = transform.rotation;
        return 1;
    }


}
