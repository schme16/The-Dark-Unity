using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PointScript : MonoBehaviour
{
	public float lifetime;

	private float time;

	// Start is called before the first frame update
	void Start()
	{
		Destroy(gameObject, lifetime);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}