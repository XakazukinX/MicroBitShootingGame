using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _20191205
{
    public class AcceleratorDebugger : MonoBehaviour
    {
        private void OnEnable()
        {
            SerialPortReadLineReader.OnGetReadLine += OnGetReadLine;
        }

        private void OnGetReadLine(string readData)
        {
            Debug.Log(readData);
        }
    }
}