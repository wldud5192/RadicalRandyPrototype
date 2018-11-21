using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMaterials : MonoBehaviour
{
	public float fadeScale;
	public Color emissiveColor;

	void Update()
	{
		Renderer renderer = GetComponent<Renderer>();
		Material renderMat = renderer.materials[1];
		float emissiveIntensity = Mathf.PingPong(Time.time, fadeScale);

		Color emissionColor = emissiveColor * Mathf.LinearToGammaSpace(emissiveIntensity);
		Color baseColor = emissiveColor;

		renderMat.SetColor("_EmissionColor", emissionColor);
		renderMat.SetColor("_Color", baseColor);
	}
}
