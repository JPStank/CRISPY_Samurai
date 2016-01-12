using UnityEngine;
using System.Collections;

public class PuppetCameraScript : MonoBehaviour {

	public GameObject followCam;
	public GameObject camTarg;
	public bool camLockedOn = false;
	public Vector3 camOffsetPos;
	public Vector3 defOffsetPos;
    public Vector3 targOffsetPos;

    private PuppetScript Owner;
    //private Animator Animetor;
    private Quaternion camRot;
    private float camSpeed;

	// Use this for initialization
	void Start () {
	
	}

    // Because we dont know what orders the Start()s are called in.
    public void Initialize(PuppetScript _sender)
    {
        Owner = _sender;
        //Animetor = _sender.Animetor;

        camSpeed = _sender.camSpeed;
		followCam = GameObject.FindGameObjectWithTag("MainCamera");
		camTarg = GameObject.FindGameObjectWithTag("CamTarg");
        camRot = followCam.transform.rotation;


		if (camOffsetPos == Vector3.zero)
		{
			camOffsetPos.x = 0.0f;
			camOffsetPos.y = 1.0f;
			camOffsetPos.z = -4.0f;
		}
		defOffsetPos = camOffsetPos;
		if (targOffsetPos == Vector3.zero)
		{
			targOffsetPos.x = 0.0f;
			targOffsetPos.y = 1.0f;
			targOffsetPos.z = 0.0f;
		}
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // Update Camera Function
    // updates camera position and rotation
    public void UpdateCam()
	{
		//Vector3 pos = transform.position;
		//pos += camOffsetPos.y * transform.up;
		//pos += camOffsetPos.z * transform.forward;
		//followCam.transform.position = Vector3.Lerp(followCam.transform.position, pos, Time.deltaTime * camSpeed);

		//followCam.transform.LookAt(transform);
    }

	public void LateUpdate()
	{
		if (Owner.tag == "Player")
		{
			Vector3 pos = transform.position;
			pos += targOffsetPos.y * camTarg.transform.up;

			camTarg.transform.position = Vector3.Lerp(camTarg.transform.position, pos, Time.deltaTime * camSpeed);

			pos += camOffsetPos.y * camTarg.transform.up;
			pos += camOffsetPos.z * camTarg.transform.forward;
			//followCam.transform.position = Vector3.Lerp(followCam.transform.position, pos, Time.deltaTime * camSpeed);
			followCam.transform.position = pos;

			followCam.transform.LookAt(camTarg.transform);
		}
	}

    public int MoveCamera(Vector3 _dir)
    {
        _dir.x *= Time.deltaTime;
        _dir.y *= Time.deltaTime;
        _dir.x *= camSpeed * 20.0f;
		_dir.y *= camSpeed;

        camTarg.transform.rotation = camRot;
		camTarg.transform.Rotate(0.0f, _dir.x, 0.0f);
		camRot = camTarg.transform.rotation;

		camOffsetPos.y += _dir.y;
		camOffsetPos.z += _dir.y;

		if (camOffsetPos.y < 0.0f)
			camOffsetPos.y = 0.0f;
		else if (camOffsetPos.y > 4.0f)
			camOffsetPos.y = 4.0f;

		if (camOffsetPos.z > -3.0f)
			camOffsetPos.z = -3.0f;
		else if (camOffsetPos.z < -5.0f)
			camOffsetPos.z = -5.0f;


        return 1;
    }


    // ToggleLockon Function
    // Toggles the lockon feature of the camera
    // returns -1 on failure
    // returns 1 on success
    public int ToggleLockon()
    {
        camLockedOn = !camLockedOn;
        return 1;
    }
}
