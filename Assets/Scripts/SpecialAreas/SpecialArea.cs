using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpecialArea : MonoBehaviour
{
    [SerializeField] SpecialAreaBehaviour specialAreaBehaviour;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInChildren<Player>();

        if (player != null)
            specialAreaBehaviour.HandlePlayerInArea(player);
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponentInChildren<Player>();

        if (player != null)
            specialAreaBehaviour.HandlePlayerExitArea(player);
    }
}
