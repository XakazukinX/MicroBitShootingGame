using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class SerialPortNameTester : MonoBehaviour
{
    [ContextMenu("TestName")]
    public void TestName()
    {
        var ports = SerialPort.GetPortNames();
        foreach (var t in ports)
        {
            Debug.Log(t);
        }
    }
}
