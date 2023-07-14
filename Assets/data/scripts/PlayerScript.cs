using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
	public LanternScript lantern;
	public Transform lanternAttachPoint;
	public PlayerInput input;

	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	void Update()
	{
		if (lantern != null && lantern.isHeld)
		{
			if (input.actions["Action"].triggered)
			{
				lantern.ParticleBurst();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.name);
		if (other.CompareTag("Lantern"))
		{
			lantern.Pickup(lanternAttachPoint);
		}
	}
}