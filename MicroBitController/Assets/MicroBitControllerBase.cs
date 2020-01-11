using System;
using System.Collections;
using System.Collections.Generic;
using AppKit;
using UnityEngine;

public class MicroBitControllerBase : MonoBehaviour
{
    private ArduinoSerial _serial;


    private void Start()
    {
        _serial = ArduinoSerial.Instance;
        bool success = _serial.Open(null, ArduinoSerial.Baudrate.B_115200);
        if (!success)
        {
            return;
        }
        _serial.OnDataReceived += OnDataReceived;
    }

    private void OnDisable()
    {
        _serial.Close();
        _serial.OnDataReceived -= OnDataReceived;
    }

    public virtual void OnDataReceived (string msg)
    { 
        var button = MicroBitCommands.GetButton(msg);
      //  Debug.Log(button);
        var gesture = MicroBitCommands.GetGesture(msg);
      //  Debug.Log(gesture);
    }
    
}
