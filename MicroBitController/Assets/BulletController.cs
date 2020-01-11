using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(this.gameObject, 2);
    }

    private void Update()
    {
        transform.position += new Vector3(0, 0.02f, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            this.gameObject.SetActive(false);
        }
    }

    
}
