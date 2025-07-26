using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurGroundCheck : MonoBehaviour
{
    MinotaurController minotaurController;
    void Awake()
    {
        minotaurController = GetComponentInParent<MinotaurController>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == minotaurController.gameObject)
            return;
        minotaurController.SetGroundedStates(true);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == minotaurController.gameObject)
            return;
        minotaurController.SetGroundedStates(false);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == minotaurController.gameObject)
            return;
        minotaurController.SetGroundedStates(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == minotaurController.gameObject)
            return;
        minotaurController.SetGroundedStates(true);
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == minotaurController.gameObject)
            return;
        minotaurController.SetGroundedStates(false);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == minotaurController.gameObject)
            return;
        minotaurController.SetGroundedStates(true);
    }
}
