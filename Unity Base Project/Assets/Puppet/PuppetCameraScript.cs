using UnityEngine;
using System.Collections;

public class PuppetCameraScript : MonoBehaviour
{
	// New things, added by Dakota 1/13 8:46pm
	public bool invertY = false;
	public bool invertX = false;

	public GameObject followCam;
	public GameObject camTarg;
	public bool camLockedOn = false;
	public Vector3 camOffsetPos;
	public Vector3 def_OffsetPos;
	public Vector3 targOffsetPos;
	public Vector3 def_rockedOffsetPos;
	public Vector3 rockedOffsetPos;
	public float rockedMinMax;

	private PuppetScript Owner;
	private Quaternion camRot;
	private float def_camSpeed;
	private float camSpeed;
	private float last_camSpeed;
	private float peekTmr = 0.0f;

	// Use this for initialization
	void Start()
	{

	}

	// Because we dont know what orders the Start()s are called in.
	public void Initialize(PuppetScript _sender)
	{
		Owner = _sender;

		camSpeed = _sender.camSpeed;
		def_camSpeed = camSpeed;
		followCam = GameObject.FindGameObjectWithTag("MainCamera");
		camTarg = GameObject.FindGameObjectWithTag("CamTarg");
		camRot = followCam.transform.rotation;


		if (camOffsetPos == Vector3.zero)
		{
			camOffsetPos.x = 0.0f;
			camOffsetPos.y = 1.0f;
			camOffsetPos.z = -4.0f;
		}
		def_OffsetPos = camOffsetPos;
		if (targOffsetPos == Vector3.zero)
		{
			targOffsetPos.x = 0.0f;
			targOffsetPos.y = 1.5f;
			targOffsetPos.z = 0.0f;
		}
		if (rockedOffsetPos == Vector3.zero)
		{
			rockedOffsetPos.x = 1.5f;
			rockedOffsetPos.y = 0.0f;
			rockedOffsetPos.z = 0.0f;
		}
		def_rockedOffsetPos = rockedOffsetPos;
		if (rockedMinMax == 0.0f)
		{
			rockedMinMax = 2.0f;
		}
	}

	// Update is called once per frame
	void Update()
	{

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
			// Lerp camTarg to where it should be in reference to the puppet
			Vector3 pos = transform.position;
			pos += targOffsetPos.y * camTarg.transform.up;
			camTarg.transform.position = Vector3.Lerp(camTarg.transform.position, pos, Time.deltaTime * def_camSpeed);

			// additional funcitonality for rocking on (and out)
			if (Owner.rockedOn)
			{
				// adjust the offset
				pos += rockedOffsetPos.x * camTarg.transform.right;
				pos += rockedOffsetPos.y * camTarg.transform.up;
				pos += rockedOffsetPos.z * camTarg.transform.forward;
				camTarg.transform.position = Vector3.Lerp(camTarg.transform.position, pos, Time.deltaTime * def_camSpeed);

				Vector3 targPos = Owner.curTarg.transform.position + Owner.targOffset;
				targPos.y = camTarg.transform.position.y;
				camTarg.transform.LookAt(Vector3.Lerp(camTarg.transform.position, targPos, Time.deltaTime * def_camSpeed));
				camRot = camTarg.transform.rotation;
			}

			// puts the camera itself on a pole in the offset's direction from the target
			pos += camOffsetPos.y * camTarg.transform.up;
			pos += camOffsetPos.z * camTarg.transform.forward;
			followCam.transform.position = Vector3.Lerp(followCam.transform.position, pos, Time.deltaTime * def_camSpeed);

			// does the rotations for us
			followCam.transform.LookAt(camTarg.transform);
		}
	}

	public int MoveCamera(Vector3 _dir)
	{
		if (_dir == Vector3.zero)
			camSpeed = 0.0f;
		camSpeed = Mathf.Lerp(camSpeed, def_camSpeed, Time.deltaTime * 5.0f);

		_dir.x *= Time.deltaTime;
		_dir.y *= Time.deltaTime;
		_dir.x *= camSpeed * 20.0f;
		_dir.y *= camSpeed;

		if (invertY)
			_dir.y *= -1;
		if (invertX)
			_dir.x *= -1;

		// free camera movement
		if (!Owner.rockedOn)
		{
			camTarg.transform.rotation = camRot;
			camTarg.transform.Rotate(0.0f, _dir.x, 0.0f);
			camRot = camTarg.transform.rotation;
		}
		else // movement while locked on
		{
			// adjust the amount and error check it
			rockedOffsetPos.x += _dir.x * 0.05f;

			if (rockedOffsetPos.x < def_rockedOffsetPos.x - rockedMinMax)
				rockedOffsetPos.x = def_rockedOffsetPos.x - rockedMinMax;
			else if (rockedOffsetPos.x > def_rockedOffsetPos.x + rockedMinMax)
				rockedOffsetPos.x = def_rockedOffsetPos.x + rockedMinMax;


			if (_dir.x == 0.0f)
			{

				if (peekTmr > 0.0f)
				{
					peekTmr -= Time.deltaTime;
					if (peekTmr <= 0.0f)
						peekTmr = 0.0f;
				}
				if (peekTmr == 0.0f)
					rockedOffsetPos = Vector3.Lerp( rockedOffsetPos, def_rockedOffsetPos, Time.deltaTime * 1.5f);
			}
			else
				peekTmr = 0.3f;
		}

		// adjust the ammount and error check it
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
		if (Owner.curState == PuppetScript.State.DGE_BACK
			|| Owner.curState == PuppetScript.State.DGE_FORWARD
			|| Owner.curState == PuppetScript.State.DGE_LEFT
			|| Owner.curState == PuppetScript.State.DGE_RIGHT)
			return -1;

		if (!camLockedOn)
		{
			Vector3 orgPos = camTarg.transform.position;
			Vector3 tempPos = transform.position;
			tempPos -= 1.0f * transform.forward;
			camTarg.transform.position = tempPos;
			camTarg.transform.LookAt(transform);
			camTarg.transform.position = orgPos;

			Owner.moveSpeed = Owner.def_moveSpeed * Owner.lockMoveSpeedMod;
		}
		else
		{
			camRot = camTarg.transform.rotation;

			Owner.moveSpeed = Owner.def_moveSpeed;
		}

		camLockedOn = !camLockedOn;
		Owner.rockedOn = camLockedOn;
		return 1;
	}
}
