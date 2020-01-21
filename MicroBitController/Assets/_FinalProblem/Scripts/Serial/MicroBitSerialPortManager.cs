using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using _PCFinal;
using SerialPortUtility;
using UnityEngine;

public class MicroBitSerialPortManager : MonoBehaviour
{
    [SerializeField] private SerialMessagePresenter _presenter;
    private SerialPortSetting _serialPortSetting;
    public SerialPortSetting SerialPortSetting
    {
        get => _serialPortSetting;
        set => _serialPortSetting = value;
    }
    
    public SerialPort serialStream = new SerialPort();
    
    public static SerialPortReader _serialPortReader;
    public static SerialPortWriter _serialPortWriter;

    public bool isTestMode = false;
    private SerialPortSetting testModeSetting;

    public void OnEnable()
    {
        if (isTestMode)
        {
            Debug.Log("テストモードでSerialManagerを起動しました");
            Init(testModeSetting);
        }
    }

    public void Init(SerialPortSetting setting)
    {
        try
        {
            _serialPortSetting = setting;
            serialStream = new SerialPort(_serialPortSetting.targetPortName, (int) _serialPortSetting.baudRate);
            
            serialStream.Open();
            serialStream.DiscardInBuffer();
            serialStream.DiscardOutBuffer();

        }
        catch (Exception e)
        {
            Debug.LogError("シリアルポートのOpenに失敗しました");
            serialStream.Close();
            enabled = !enabled;
            return;
        }
        
        _serialPortReader = new SerialPortReader(serialStream);
        StartSerialRead();
        _serialPortWriter = new SerialPortWriter(serialStream);
        DontDestroyOnLoad(this.gameObject);
        
        _presenter.InitPresenter();
        Debug.Log("シリアルポートのOpenに成功しました");
    }
    
    //別スレッドでSerialReadし続けるモードを起動する
    private void StartSerialRead()
    {
        _serialPortReader.StartReadStream();
    }

    //終了時に破棄
    private void OnDestroy()
    {
        if (!serialStream.IsOpen) return;
        _serialPortReader.StopReadStream();
        serialStream.Close();
    }
}
