using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _20191128
{
    public class EnemyController : MonoBehaviour
    {
        public int enemyIndex;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Bullet")
            {
                Destroy(this.gameObject);
            }
        }
    }
}