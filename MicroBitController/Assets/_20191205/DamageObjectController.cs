using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObjectController : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRigidBody;
    [SerializeField] private float forceValue;

    private void FixedUpdate()
    {
        targetRigidBody.AddForce(0, forceValue, 0);
    }
}
