﻿using UnityEngine;
using System.Collections;

public class PuppetCameraScript : MonoBehaviour
{

	public GameObject followCam;
	public GameObject camTarg;
	public bool camLockedOn = false;
	public Vector3 camOffsetPos;
	public Vector3 defOffsetPos;
	public Vector3 targOffsetPos;
	public Vector3 rockedOffsetPos;

	private PuppetScript Owner;
	private Quaternion camRot;
	private float camSpeed;
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
		//if (rockedOffsetPos == Vector3.zero)
		//{
		//	targOffsetPos.x = 0.0f;
		//	targOffsetPos.y = 0.0f;
		//	targOffsetPos.z = 0.0f;
		//}
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
			camTarg.transform.position = Vector3.Lerp(camTarg.transform.position, pos, Time.deltaTime * camSpeed);

			// additional funcitonality for rocking on (and out)
			if (Owner.rockedOn)
			{
				pos += rockedOffsetPos;
				camTarg.transform.position = Vector3.Lerp(camTarg.transform.position, pos, Time.deltaTime * camSpeed);
			}

			// puts the camera itself on a pole in the offsets direction from the target
			pos += camOffsetPos.y * camTarg.transform.up;
			pos += camOffsetPos.z * camTarg.transform.forward;
			followCam.transform.position = pos;

			// does the rotations for us
			followCam.transform.LookAt(camTarg.transform);
		}
	}

	public int MoveCamera(Vector3 _dir)
	{
		_dir.x *= Time.deltaTime;
		_dir.y *= Time.deltaTime;
		_dir.x *= camSpeed * 20.0f;
		_dir.y *= camSpeed;

		// free camera movement
		if (!Owner.rockedOn)
		{
			camTarg.transform.rotation = camRot;
			camTarg.transform.Rotate(0.0f, _dir.x, 0.0f);
			camRot = camTarg.transform.rotation;
		}
		else // movement while locked on
		{
			rockedOffsetPos.x += _dir.x * 0.2f;

			if (rockedOffsetPos.x < -2.0f)
				rockedOffsetPos.x = -2.0f;
			else if (rockedOffsetPos.x > 2.0f)
				rockedOffsetPos.x = 2.0f;


			if (_dir.x == 0.0f)
			{

				if (peekTmr > 0.0f)
				{
					peekTmr -= Time.deltaTime;
					if (peekTmr <= 0.0f)
					{
						peekTmr = 0.0f;
					}
				}
				if (peekTmr == 0.0f)
				{
					rockedOffsetPos = Vector3.Lerp( rockedOffsetPos, Vector3.zero, Time.deltaTime * 2.0f);
				}
			}
			else
			{
				peekTmr = 0.3f;
			}
		}


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
