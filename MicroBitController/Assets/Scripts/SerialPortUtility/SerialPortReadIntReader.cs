using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using SerialPortUtility;
using UnityEngine;

public class SerialPortReadIntReader : SerialPortReadProvider
{
    public static Action<int> OnGetReadInt;
    
    public override void StreamReadBehavior(SerialPort serialPort, byte readData)
    {
        OnGetReadInt?.Invoke(readData);
    }
}
