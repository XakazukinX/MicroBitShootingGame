using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SerialPortUtility
{
    public class SerialPortReadProvider : MonoBehaviour
    {
        private SynchronizationContext _mainContext;

        [SerializeField] private SerialPortSetting _serialPortSetting = new SerialPortSetting();

        private SerialPort serialStream = new SerialPort();
        private bool isSerialReadRunning;

        public virtual void Start()
        {
            _mainContext = SynchronizationContext.Current;

            serialStream = new SerialPort(_serialPortSetting.targetPortName, (int) _serialPortSetting.baudRate);
            serialStream.Open();
            serialStream.DiscardInBuffer();
            serialStream.DiscardOutBuffer();

            isSerialReadRunning = true;

            //受信処理はTaskに名月破にしておく
            Task.Run(ReadSerialPort);

        }

        private void OnDestroy()
        {
            CloseSerialStream();
        }

        public virtual void CloseSerialStream()
        {
            isSerialReadRunning = false;
            serialStream.Close();
        }

        private void ReadSerialPort()
        {
            while (serialStream.IsOpen && serialStream != null && isSerialReadRunning)
            {
                if (serialStream.BytesToRead == 0) continue;
                try
                {
                    while (serialStream.BytesToRead != 0)
                    {
                        var readData = (byte) serialStream.ReadByte();
                        _mainContext.Post(_ => StreamReadBehavior(serialStream, readData), null);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
        }

        public virtual void StreamReadBehavior(SerialPort serialPort, byte readData)
        {
        }
    }
}
