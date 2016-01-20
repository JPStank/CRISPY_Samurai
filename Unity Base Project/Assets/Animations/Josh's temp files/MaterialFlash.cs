using UnityEngine;
using System.Collections;

public class MaterialFlash : MonoBehaviour 
{
    public Material flashMaterial = null;

    private Material origMaterial;

    private bool flashing = false;
    public float flashTime = 1.0f;
    private float timer = 0.0f;
    public int flashTick = 0;
    private bool isOrig = true;

	void Start () 
    {
        origMaterial = renderer.material;

        if (flashMaterial == null)
        {
            Debug.LogError("YOU MUST SET A FLASH MATERIAL IDIOT!!");
        }
	}
	
	void Update () 
    {
	    if (flashing)
        {
            timer += Time.deltaTime;
            if (timer >= flashTime) // reset everything and gtfo
            {
                flashing = false;
                timer = 0.0f;
                flashTick = 0;
                renderer.material = origMaterial;
                isOrig = true;
                return;
            }

            flashTick++;

            if (flashTick >= 10) // magic numbers ftw
            {
                flashTick = 0;
                renderer.material = (isOrig ? flashMaterial : origMaterial);
                isOrig = !isOrig;
            }
        }
	}

    public void StartFlash()
    {
        flashing = true;
        renderer.material = flashMaterial;
        isOrig = false;
    }
}
