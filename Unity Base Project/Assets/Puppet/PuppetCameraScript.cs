using UnityEngine;
using System.Collections;

public class PuppetCameraScript : MonoBehaviour {

    public GameObject followCam;
    public bool camLockedOn = false;

    private PuppetScript Owner;
    private Animator Animetor;
    private Quaternion camRot;
    public Vector3 camOffsetPos;
    public Vector3 camOffsetRot;
    public float camSpeed;

	// Use this for initialization
	void Start () {
	
	}

    // Because we dont know what orders the Start()s are called in.
    public void Initialize(PuppetScript _sender)
    {
        Owner = _sender;
        Animetor = _sender.Animetor;

        camSpeed = _sender.camSpeed;
        followCam = GameObject.FindGameObjectWithTag("MainCamera");
        camRot = followCam.transform.rotation;
        camOffsetRot.x = 20.0f;
        followCam.transform.Rotate(camOffsetRot);

        camOffsetPos.x = 0.0f;
        camOffsetPos.y = 1.0f;
        camOffsetPos.z = -4.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // Update Camera Function
    // updates camera position and rotation
    public void UpdateCam()
    {
        Vector3 pos = transform.position;
        //Quaternion rot = transform.rotation;
        followCam.transform.position = pos;
        //followCam.transform.rotation = rot;

        followCam.transform.rotation = camRot;
        followCam.transform.Rotate(camOffsetRot);

        followCam.transform.Translate(camOffsetPos);
    }

    public int MoveCamera(Vector3 _dir)
    {
        _dir.x *= Time.deltaTime;
        _dir.y *= Time.deltaTime;
        _dir.x *= camSpeed;
        _dir.y *= camSpeed;

        followCam.transform.rotation = camRot;
        followCam.transform.Rotate(_dir.y, _dir.x, 0.0f);
        camRot = followCam.transform.rotation;
		camRot.z = 0.0f;
		followCam.transform.rotation = camRot;

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
