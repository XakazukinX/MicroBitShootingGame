using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _20191205
{
    public class FallingMessagePresenter : MonoBehaviour
    {
        [SerializeField] private GameObject damageObject;
        private void OnEnable()
        {
            HostMessageReceiver.OnGetDamageObjectFallingPos += OnGetDamageObjectFallingPos;
        }
        
        private void OnGetDamageObjectFallingPos(Vector3 fallingPos)
        {
            Debug.Log(fallingPos);
            var tmpObj = GameObject.Instantiate(damageObject, fallingPos, Quaternion.identity);

        }
    }
}