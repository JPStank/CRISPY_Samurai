using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DebugMonitor : MonoBehaviour
{
	public Text debugText;

	int lineCounter = 1;

	// Use this for initialization
	void Start()
	{
		//debugText = GameObject.Find("Debug_Canvas/Text Panel/Debug Text").GetComponent<Text>();
		//if (debugText)
		//{
		//	UpdateText("Hello Debug!");
		//}
	}

	// Update is called once per frame
	void Update()
	{
		//debugText = GameObject.Find("Debug_Canvas/Text Panel/Debug Text").GetComponent<Text>();
	}

	public void UpdateText(string newText)
	{
		if (debugText)
		{
			lineCounter++;
			debugText.text += ('\n' + newText);

			if (lineCounter == 9)
			{
				lineCounter--;
				int loc = debugText.text.IndexOf('\n');
				debugText.text = debugText.text.Remove(0, loc + 1);
			}
		}
	}
}
