using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private Player m_Player;
    private Vector3 m_Input;

    private void Start()
    {
        m_Player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        m_Input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        m_Player.Move(m_Input);
        m_Player.Attack(transform.position);
    }
}
