using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTeleporter : MonoBehaviour
{

    public GameObject receptor;
    public float offsetForwardTeleportation;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something collided");
        if (other.gameObject.CompareTag(Constants.ObjectsTags.THROWABLE))
        {
            Rigidbody body = other.gameObject.GetComponent<Rigidbody>();
            if (!body.isKinematic)
            {
                Vector3 receptorForward = receptor.transform.forward;
                body.velocity = GetVelocityRotation(body.velocity.normalized, receptorForward) * body.velocity;
                other.gameObject.transform.rotation = Quaternion.LookRotation(other.gameObject.transform.position - receptor.transform.position);
                other.gameObject.transform.position = receptor.transform.position + (receptorForward * offsetForwardTeleportation);
            }
        }
    }

    private Quaternion GetVelocityRotation(Vector3 velocityNormal, Vector3 receptorForward)
    {
        return Quaternion.FromToRotation(velocityNormal, receptorForward);
    }
}
