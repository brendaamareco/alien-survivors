using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichiMovement : MonoBehaviour
{
    [SerializeField] Enemy Et;
    private Player m_Player;
    private Vector3 m_Input;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GetComponent<Player>();
    }

    private void Update()
    {
        Vector3 targetPosition = Et.transform.position;

        m_Player.Move(targetPosition);
        Rotate(targetPosition);
        //RandomMovement();
        //RandomMovement2();
        //RandomMovement2();
    }
    public void Rotate(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void RandomMovement2() 
    {
        float horizontalMove = 0f;
        int rightAxis = Random.Range(1, 3);
        switch (rightAxis)
        {
            case 1:
                horizontalMove = -1f;
                break;
            case 2:
                horizontalMove = 1f;
                break;
        }
        m_Input = new Vector3(horizontalMove, 0, 0);
        m_Player.Move(m_Input);
    }
    public void RandomMovement()
    {
        int forwardAxis = Random.Range(1, 3);
        int rightAxis = Random.Range(1, 3);

        float horizontalMove = 0f;
        float verticalMove = 0f;

        switch (forwardAxis)
        {
            case 1:
                Debug.Log("Flecha arriba 1f");
                verticalMove = 1f;
                break;
            case 2:
                verticalMove = -1f;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                horizontalMove = -1f;
                break;
            case 2:
                horizontalMove = 1f;
                break;
        }
        m_Input = new Vector3(horizontalMove, 0, verticalMove);
        Debug.Log(m_Input);
        m_Player.Move(m_Input);
    }
}
