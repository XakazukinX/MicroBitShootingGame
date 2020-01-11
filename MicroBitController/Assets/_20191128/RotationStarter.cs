using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _20191128
{
    public class RotationStarter : MonoBehaviour
    {
        [SerializeField] private GameObject targetObject;
        [SerializeField] private float rotateSpeed;
        private bool isRotation;
        private void OnEnable()
        {
            SerialPortReadIntReader.OnGetReadInt += OnGetReadInt;
        }

        private void OnGetReadInt(int _getData)
        {
            Debug.Log(_getData);
            isRotation = _getData == 1;
        }

        private void Update()
        {
            if (isRotation)
            {
                transform.eulerAngles += new Vector3(0, rotateSpeed, 0);
            }
        }
    }
}