using UnityEngine;
using System.Collections;

public class WindowOpenEffectBehavior : MonoBehaviour 
{
    public ParticleSystem ps = null;
    public float endLifetime = 0.3f;
	public float CurAlpha;
	public float EndAlpha;

	void Start () 
    {
	    if (ps == null)
        {
            ps = GetComponent<ParticleSystem>();
        }
        transform.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
		CurAlpha = 1.0f;
		EndAlpha = 0.0f;
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
			// alpha stuff
			CurAlpha = Mathf.Lerp(CurAlpha, EndAlpha, elapsedTime * 0.1f / length);
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.maxParticles];
			int numParticles = ps.GetParticles(particles);
			for (int i = 0; i < numParticles; i++)
			{
				particles[i].color = new Color(particles[i].color.r, particles[i].color.g, particles[i].color.b, CurAlpha);
			}
			ps.SetParticles(particles, numParticles);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        ps.enableEmission = false;
        Destroy(gameObject);
    }
}
