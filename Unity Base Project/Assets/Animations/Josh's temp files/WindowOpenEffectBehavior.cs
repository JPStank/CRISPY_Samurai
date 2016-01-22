using UnityEngine;
using System.Collections;

public class WindowOpenEffectBehavior : MonoBehaviour 
{
    public ParticleSystem ps = null;
    public float endLifetime = 0.3f;

	void Start () 
    {
	    if (ps == null)
        {
            ps = GetComponent<ParticleSystem>();
        }
        transform.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
	}
	
    public void GoTime(float t)
    {
        StartCoroutine(ShrinkOverTime(t));
    }
    public IEnumerator ShrinkOverTime(float length)
    {

        float elapsedTime = 0.0f;

        float startLife, endLife, currLife;

        startLife = currLife = ps.startLifetime;

        endLife = endLifetime;

        while (elapsedTime < length)
        {
            ps.startLifetime = currLife;
            currLife = Mathf.Lerp(startLife, endLifetime, elapsedTime / length);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        ps.enableEmission = false;
        Destroy(gameObject);
    }
}
