using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpAttractor : MonoBehaviour
{
    public float attractionSpeed = 5f; // Adjust this value to control the attraction speed.
    private Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            AttractExpDrops();
        }
    }

    private void AttractExpDrops()
    {
        foreach (GameObject expDrop in GameObject.FindGameObjectsWithTag("Exp"))
        {
            Vector3 direction = playerTransform.position - expDrop.transform.position;
            float distance = direction.magnitude;

            if (distance > 0.1f)
            {
                float step = attractionSpeed * Time.deltaTime;
                expDrop.transform.position = Vector3.MoveTowards(expDrop.transform.position, playerTransform.position, step);
            }
        }
    }
}

