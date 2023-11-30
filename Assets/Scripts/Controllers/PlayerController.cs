using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private Player m_Player;
    private Vector3 m_Input;
    private bool playerIsDead = false;

    private void Start()
    {
        LoadPlayer();
        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleDead);
    }

    private void HandleDead(EventContext context)
    {
        try
        {
            Player player = (Player)context.GetEntity();

            if (player != null)
            {
                playerIsDead = true;
            }
        }
        catch { }

    }
        private void LoadPlayer()
    {
        m_Player = GetComponent<Player>();
    }

    public void Reset() 
    {
        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleDead);
    }

    private void FixedUpdate()
    {
        if (!playerIsDead) { 
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
}
