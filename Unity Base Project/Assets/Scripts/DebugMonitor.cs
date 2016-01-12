using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugMonitor : MonoBehaviour
{
    public Text debugText;

    int lineCounter = 1;

	// Use this for initialization
	void Start ()
    {
        UpdateText("Hello Debug!");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void UpdateText(string newText)
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
