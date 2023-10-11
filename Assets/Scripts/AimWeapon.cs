using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    private Quaternion iniRot;
    // Start is called before the first frame update
    void Start()
    {
        iniRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Obten la rotación actual del GameObject
        Vector3 currentRotation = transform.eulerAngles;

        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            iniRot = Quaternion.Euler(currentRotation.x, 6f, currentRotation.z);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            iniRot = Quaternion.Euler(currentRotation.x, 96f, currentRotation.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow))
        {
            iniRot = Quaternion.Euler(currentRotation.x, -174f, currentRotation.z);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            iniRot = Quaternion.Euler(currentRotation.x, -84f, currentRotation.z);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            iniRot = Quaternion.Euler(currentRotation.x, 51f, currentRotation.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            iniRot = Quaternion.Euler(currentRotation.x, 141f, currentRotation.z);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            iniRot = Quaternion.Euler(currentRotation.x, -129f, currentRotation.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            iniRot = Quaternion.Euler(currentRotation.x, -39f, currentRotation.z);
        }
    }
    void LateUpdate()
    {
        transform.rotation = iniRot;
    }
}
