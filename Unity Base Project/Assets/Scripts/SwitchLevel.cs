using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchLevel : MonoBehaviour {

    public string m_szLevel;
    public float m_fDelay;
    public float m_fDelayRemaining;
    public bool m_bCountdown;
	public Image m_tSplash;
	// Use this for initialization
	void Start ()
    {
        m_fDelayRemaining = m_fDelay;
        m_bCountdown = true;                // On by default
	}
	
	// Update is called once per frame
	void Update ()
   {

        if(m_bCountdown)                           
            m_fDelayRemaining -= Time.deltaTime;

		if(m_tSplash != null)
			m_tSplash.color = new Color(m_tSplash.color.r, m_tSplash.color.g, m_tSplash.color.b, ((m_fDelay - m_fDelayRemaining) / m_fDelay));

        if(m_fDelayRemaining < 0.0f)
            Application.LoadLevel(m_szLevel);
	
	}

    public void EnableCountdown(bool bEnable, bool bReset)
    {
        if (bEnable == true)
            m_tSplash.gameObject.SetActive(true);

        m_bCountdown = bEnable;
		if(bReset)
			m_fDelayRemaining = m_fDelay;
    }

	public void SetDelay(float fDelay)
	{
		m_fDelay = fDelay;
	}

	public void SetDelayRemaining(float fDelay)
	{
		m_fDelayRemaining = fDelay;
	}
}
