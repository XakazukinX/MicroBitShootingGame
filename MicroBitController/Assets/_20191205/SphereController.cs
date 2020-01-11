using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _20191205
{
    public class SphereController : MicroBitAcceleratorProviderBase
    {
        public static Action OnGetDamage;
        [SerializeField] private int Threshold = 600;
        [SerializeField] private float forceValue = 1;
        private Rigidbody targetRigidBody;
        private bool isGameOver = false;
        
        private void Start()
        {
            targetRigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            var msg = new PlayerPosMessage()
            {
                playerPos = this.transform.position
            };
            
            NetworkServer.SendToAll(msg);
        }

        public override void OnAccXGet(int accValue)
        {
            if(isGameOver) return;
            base.OnAccXGet(accValue);
            if (Mathf.Abs(accValue) > Threshold)
            {
                if (accValue > 0)
                {
                    targetRigidBody.AddForce(forceValue,0,0);
                    Debug.Log("Right");

                }
                
                if (accValue < 0)
                {
                    targetRigidBody.AddForce(-forceValue,0,0);
                    Debug.Log("Left");
                }
            }
        }
        
        public override void OnAccYGet(int accValue)
        {
            if(isGameOver) return;
            base.OnAccXGet(accValue);
            if (Mathf.Abs(accValue) > Threshold)
            {
                if (accValue > 0)
                {
                    targetRigidBody.AddForce(0, 0, forceValue);
                    Debug.Log("Front");
                }
                
                if (accValue < 0)
                {
                    targetRigidBody.AddForce(0, 0, -forceValue);
                    Debug.Log("Back");
                }
            }
        }


        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "DamageObject")
            {
                OnGetDamage?.Invoke();
                isGameOver = true;
            }
        }
    }
}