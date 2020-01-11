using System;
using System.Collections;
using System.Collections.Generic;
using SerialPortUtility;
using UnityEngine;

namespace _20191128
{
    public class CubeController : MonoBehaviour
    {
        private enum CubeDirectionStatus
        {
            Center =0,
            Left,
            Right,
        }
        private CubeDirectionStatus _cubeDirectionStatus;
        private Vector3 rotVec = Vector3.zero;
        private Vector3 moveVec = Vector3.forward;
        [SerializeField] private float moveSpeed = 0.7f;
        [SerializeField] private KeyCode resetKey = KeyCode.R;
        private void Start()
        {
            moveVec = new Vector3(0, 0, moveSpeed);
        }
        private void OnEnable()
        {
            SerialPortReadIntReader.OnGetReadInt += OnGetReadInt;
        }
        private void OnGetReadInt(int _getData)
        {
            _cubeDirectionStatus = (CubeDirectionStatus) _getData;
        }
        private void Update()
        {
            if (Input.GetKeyDown(resetKey))
            {
                transform.position = Vector3.zero;
                _cubeDirectionStatus = CubeDirectionStatus.Center;
            }
            switch (_cubeDirectionStatus)
            {
                //めんどくさいしオイラー角で
                case CubeDirectionStatus.Center:
                    rotVec.Set(0, 0, 0);
                    break;
                case CubeDirectionStatus.Left:
                    rotVec.Set(0, -30, 0);
                    break;
                case CubeDirectionStatus.Right:
                    rotVec.Set(0, 30, 0);
                    break;
            }
            transform.eulerAngles = rotVec;
            transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
    }
}