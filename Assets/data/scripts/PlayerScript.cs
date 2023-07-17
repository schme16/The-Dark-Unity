using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerScript : MonoBehaviour
{
	public GameManager gm;
	public Transform lanternAttachPoint;
	public PlayerInput input;
	public VisualEffect playerVFX;
	public float vfxLerpSpeed;
	private float playerVFXLerp;
	private float lanertnVFXLerp;

	// Start is called before the first frame update
	void Start()
	{
		playerVFXLerp = 0;
	}

	// Update is called once per frame
	void Update()
	{
		playerVFXLerp = Mathf.Min(1, Mathf.Max(0, gm.lantern.isHeld ? playerVFXLerp + (Time.deltaTime * vfxLerpSpeed) : playerVFXLerp - (Time.deltaTime * vfxLerpSpeed)));
		lanertnVFXLerp = Mathf.Min(1, Mathf.Max(0, gm.lantern.isHeld ? playerVFXLerp + (Time.deltaTime * vfxLerpSpeed) : playerVFXLerp - (Time.deltaTime * (vfxLerpSpeed * 2))));
		
		if (gm.lantern != null && gm.lantern.isHeld)
		{
			playerVFX.SetFloat("Intensity", 1);
			playerVFX.SetFloat("Lerp", playerVFXLerp);
			gm.lantern.lanternVFX.SetFloat("Lerp", lanertnVFXLerp);
			if (input.actions["Action"].triggered)
			{
				gm.lantern.ParticleBurst();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Lantern") && !gm.lantern.isHeld)
		{
			Debug.Log(1111);
			gm.lantern.Pickup(lanternAttachPoint);
		}
	}
}