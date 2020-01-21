using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace SerialPortUtility
{
    public class SerialPortReader
    {
        private SynchronizationContext _mainContext;
        public  Action<SerialPort, byte[]> OnStreamRead;
        private SerialPort _serialStream;
        private bool _isSerialReadRunning;

        private byte[] microBitDataBuffer = new byte[256];
        private byte[] serialDataBuffer = new byte[4];
        private int dataCount = 0;
         
        public SerialPortReader(SerialPort serialStream)
        {
            _serialStream = serialStream;
            if (!_serialStream.IsOpen)
            {
                Debug.LogError("SerialPortが開いていません");
                return;
            }
            
            _mainContext = SynchronizationContext.Current;
            Debug.Log("Open SerialPort:Read");
        }

        //1byteだけ読む
        private byte ReadByte()
        {
            if (!_serialStream.IsOpen)
            {
                Debug.LogError("SerialPortが開いていません");
            }

            if (_isSerialReadRunning)
            {
                Debug.Log("ReadStreamModeになっています。1byteずつ読みたい場合はStopReadByteTask()を実行してください");
            }
            
            return (byte) _serialStream.ReadByte();
        }

        
        //ReadStreamモードをオンにしてbyteを受信し続けるようにする。
        public void StartReadStream()
        {
            _isSerialReadRunning = true;
            Task.Run(ReadStreamTask);
        }
        
        private void ReadStreamTask()
        {
            while (_serialStream.IsOpen && _serialStream != null && _isSerialReadRunning)
            {
                if (_serialStream.BytesToRead == 0) continue;
                try
                {
                    var readData = (byte) _serialStream.ReadByte();
/*
                    Debug.Log((char)readData);
*/
                    microBitDataBuffer[dataCount] = readData;
                    dataCount += 1;
//                    Debug.Log(readData);
                    
/*                    var tes = "";
                    for (int i = 0; i < dataCount; i++)
                    {
                        tes += $"[{microBitDataBuffer[i]}]";
                    }
                        
                    Debug.Log(tes);*/
                    if (readData == 255 && dataCount >= 4)
                    {
                        if (microBitDataBuffer[dataCount - 4] == 200)
                        {
                            Buffer.BlockCopy(microBitDataBuffer, dataCount - 4, serialDataBuffer, 0, 4);
/*                            var tes = "";
                            for (int i = 0; i <= dataCount; i++)
                            {
                                tes += $"[{microBitDataBuffer[i]}]";
                            }
                            Debug.Log(tes);*/

/*                            Debug.Log(tes);*/
                            _mainContext.Post(_ =>
                                {
                                    //1byte受け取ったらアクションを発火する
                                    OnStreamRead?.Invoke(_serialStream, serialDataBuffer);
                                }
                                , null);
                                
/*                                microBitDataBuffer= new byte[256];*/
                        }
                        
                        dataCount = 0;
                    }
                    else
                    {
                        microBitDataBuffer.Initialize();

/*                        var tes = "";
                        for (int i = 0; i < dataCount; i++)
                        {
                            tes += $"[{microBitDataBuffer[i]}]";
                        }
                        Debug.Log(tes);*/
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
        }
        
        public void StopReadStream()
        {
            _isSerialReadRunning = false;
        }
    }
}
