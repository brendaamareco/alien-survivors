using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectInvincible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            Player player = playerObject.GetComponent<Player>();
            player.MakeInvincible();
        }
    }
}
