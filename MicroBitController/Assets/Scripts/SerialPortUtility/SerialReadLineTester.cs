using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialReadLineTester : MonoBehaviour
{
    private void OnEnable()
    {
        SerialPortReadLineReader.OnGetReadLine += OnGetReadLine;
    }
    
    private void OnGetReadLine(string _readData)
    {
        Debug.Log(_readData);
    }
}
