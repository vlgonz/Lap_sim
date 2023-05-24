using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganCollision : MonoBehaviour
{
    void Start()
    {
        // Disable the MeshRenderer component at the start
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Enter");
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Check if the collision was with the capsule game object
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Stay");
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the collision was with the capsule game object
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Exit");
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
