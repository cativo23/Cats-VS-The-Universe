using UnityEngine;
using System.Collections;

public class Visualize : MonoBehaviour {
	
	public Light[] lighs;

	private bool m_IsOk = false;
	public int m_NumSamples = 110;
	private float[] m_SamplesL, m_SamplesR;
	private int i;
	public float sample, sumL, sumR, rms, dB;
	private Vector3 scaleL, scaleR;
	// Because rms values are usually very low
	public float volume =50;
	private Color color;
	AudioSource saudio;
	// Use this for initialization
	void Start () {
		// Just proper validation
		lighs = FindObjectsOfType(typeof(Light)) as Light[];
		if (lighs != null) {
			m_SamplesL = new float[m_NumSamples];
			m_SamplesR = new float[m_NumSamples];
			m_IsOk = true;
			saudio = GetComponent<AudioSource>();
		}
		else
			Debug.Log("Missing objects linkage");
	}
	
	// Update is called once per frame
	void Update () {
		// Continuing proper validation
		if (m_IsOk) {
			saudio.GetOutputData(m_SamplesL, 0);
			saudio.GetOutputData(m_SamplesR, 1);
			//maxL = maxR = 0.0f;
			sumL = 0.0f;
			sumR = 0.0f;
			for (i = 0; i < m_NumSamples; i++) {
				sumL = m_SamplesL[i] * m_SamplesL[i];
				sumR = m_SamplesR[i] * m_SamplesR[i];
			}
			rms = Mathf.Sqrt(sumL/m_NumSamples);
			scaleL.y = Mathf.Clamp(rms*volume*volume, 0, 100f);
			color = GetVolumeColor(scaleL.y);
			for (int i = 0; i < lighs.Length; i++)
			{
				lighs[i].color = color;
			}
			rms = Mathf.Sqrt(sumR/m_NumSamples);
			scaleR.y = Mathf.Clamp(rms*volume*volume, 0, 100f);
			color = GetVolumeColor(scaleL.y);
			for (int i = 0; i < lighs.Length; i++)
			{
				lighs[i].color = color;
			}
		}
	}
	
	Color GetVolumeColor (float volume) {
		if (volume > 5f)
			return Color.red;
		if (volume >3.5f)
			return Color.yellow;
		return Color.green;
	}
	
	
}
