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
        LoadPlayer();
    }

    private void LoadPlayer()
    {
        m_Player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        float movimientoHorizontal = 0f;
        float movimientoVertical = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            movimientoVertical = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movimientoHorizontal = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movimientoVertical = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movimientoHorizontal = 1f;
        }

        m_Input = new Vector3(movimientoHorizontal, 0, movimientoVertical);

        m_Player.Move(m_Input);
        m_Player.Attack(transform.position);    
    }
}
