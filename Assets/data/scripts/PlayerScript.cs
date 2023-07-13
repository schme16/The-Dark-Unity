using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Transform lanternAttachPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Lantern"))
        {
            other.transform.SetParent(lanternAttachPoint);
            other.transform.localPosition = Vector3.zero;
            other.GetComponent<LanternScript>().isHeld = true;
        }
    }
}
