using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private Player m_Player;

    private void Start()
    {
        m_Player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        
    }
}
