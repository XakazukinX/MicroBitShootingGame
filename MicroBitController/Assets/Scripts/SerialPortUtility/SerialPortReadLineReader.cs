using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using SerialPortUtility;
using UnityEngine;

public class SerialPortReadLineReader : SerialPortReadProvider
{
    private StringBuilder builder = new StringBuilder();
    public static Action<string> OnGetReadLine;

    public override void StreamReadBehavior(SerialPort serialPort, byte readData)
    {
        char readChar = (char) readData;
        if (readChar == '\n')
        {
            OnGetReadLine?.Invoke(builder.ToString());
/*            Debug.Log(builder.ToString());*/
            builder.Clear();
        }
        else
        {
            builder.Append(readChar);
        }
    }
}
