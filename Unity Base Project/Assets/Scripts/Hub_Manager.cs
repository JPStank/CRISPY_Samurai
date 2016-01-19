using UnityEngine;
using System.Collections;

public class Hub_Manager : MonoBehaviour
{
	DebugMonitor degubber;
	// Use this for initialization
	void Start ()
	{
		degubber = GameObject.Find("Debug_Monitor").GetComponent<DebugMonitor>();
		if (degubber)
		{
			degubber.UpdateText("1: Dakota Scene");
			degubber.UpdateText("2: Josh Scene");
			degubber.UpdateText("3: Logan Scene");
			degubber.UpdateText("4: Sam Scene");
			degubber.UpdateText("5: Steve Scene");
			degubber.UpdateText("6: Menu Scene");
			//degubber.UpdateText("7: Dakota Scene");

		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
