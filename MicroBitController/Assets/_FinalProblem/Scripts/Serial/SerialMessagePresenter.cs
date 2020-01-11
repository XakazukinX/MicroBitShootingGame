using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class SerialMessagePresenter : MonoBehaviour
{
    private enum SerialDataStructure : byte
    {
        Acc = 254,
        Button = 255,
        Bomb = 253
    }

    private enum AccDataStructure : byte
    {
        AccX = 0,
        AccY = 1,
        AccZ = 2,
    }
    
    private enum ButtonDataStructure : byte
    {
        ButtonA = 0,
        ButtonB = 1
    }
    
    private enum BombDataStructure : byte
    {
        BombA = 0,
        BombB = 1,
        //追加ごとに増やしていく
    }
    

    //加速度のシリアル受信したときのマッチパターン
    private static readonly Regex AccXPattern = new Regex("(AccX)(?=:)", RegexOptions.Compiled);
    private static readonly Regex AccYPattern = new Regex("(AccY)(?=:)", RegexOptions.Compiled);
    private static readonly Regex AccZPattern = new Regex("(AccZ)(?=:)", RegexOptions.Compiled);

    //ボタンのシリアル受信したときのマッチパターン
    private static readonly Regex ButtonAPattern = new Regex("(ButtonA)(?=:)", RegexOptions.Compiled);
    private static readonly Regex ButtonBPattern = new Regex("(ButtonB)(?=:)", RegexOptions.Compiled);

    //タッチセンサのシリアル受信したときのマッチパターン
    private static readonly Regex BombPattern = new Regex("(Bomb)(?=:)", RegexOptions.Compiled);

    
    private static readonly Regex MessagePattern = new Regex("(?<=:)(.*)", RegexOptions.Compiled);
    private StringBuilder _builder = new StringBuilder();
    private void OnEnable()
    {
        MicroBitSerialPortManager._serialPortReader.OnStreamRead += OnStreamRead;
    }

    private void OnStreamRead(SerialPort serialPort, byte readData)
    {
        var readChar = (char) readData;
        
        if (readChar == '\n')
        {
            //Debug.Log(_builder.ToString());
            _builder.Clear();
        }
        else
        {
            _builder.Append(readChar);
        }
    }
}
