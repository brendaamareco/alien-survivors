using UnityEngine;

public class Motionless : Motion
{
    public override void Move(Vector3 vector, float speed) 
    {
        //Vector3 targetDirection = vector - transform.position;
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f * Time.deltaTime, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newDirection);

        //Debug.DrawRay(transform.position, newDirection, Color.red);    
    }
}
