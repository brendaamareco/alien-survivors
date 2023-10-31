using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidBodyMotion : Motion
{
    [SerializeField] private float turnSpeed = 360;
    private Rigidbody m_Rb;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    public override void Move(Vector3 vector, float speed)
    {
        if (vector != Vector3.zero)
        {
            var rot = Quaternion.LookRotation(vector.ToIso(), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
        if (m_Rb != null)
            m_Rb.MovePosition(transform.position + transform.forward * vector.normalized.magnitude * speed * Time.deltaTime);
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
