using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _PCFinal
{
    public enum MoveData
    {
        Front,
        Back
    }
    
    public enum RotData
    {
        Left,
        Right
    }
    
    public class SerialMessagePresenter : MonoBehaviour
    {
        private int dataIndex = 0;
        private readonly byte[] _dataBuffer = new byte[4];
        public static Action<MoveData> OnGetPlayerMoveData;
        public static Action<RotData> OnGetPlayerRotData;
        public static Action OnGetPlayerShot;

        public static Action OnGetSystemButtonC;
        public static Action OnGetSystemButtonB;



        public void InitPresenter()
        {
            MicroBitSerialPortManager._serialPortReader.OnStreamRead += OnStreamRead;
        }

        private void OnStreamRead(SerialPort serialPort, byte[] readData)
        {
            //Debug.Log($"DataCount :{dataIndex}/data :{readData}");

            if (readData[0] != 200 && readData[3] !=255)
            {
                return;
            }
            
            if (readData[1] == (byte) MicroBattleSerialMessageStructure.Rot)
            {
                InvokeGetRot((MicroBattleRotStructure) readData[2]);
            }
            else if (readData[1] == (byte) MicroBattleSerialMessageStructure.Move)
            {
                InvokeGetMove((MicroBattleMoveStructure) readData[2]);
            }
            else if (readData[1] == (byte) MicroBattleSerialMessageStructure.Shot)
            {
                OnGetPlayerShot?.Invoke();
                Debug.Log("Yellow");
            }
            else if (readData[1] == (byte) MicroBattleSerialMessageStructure.Bomb)
            {
                Debug.Log("Green");
            }
            else if (readData[1] == (byte) MicroBattleSerialMessageStructure.SystemC)
            {
                OnGetSystemButtonC?.Invoke();
                Debug.Log("SystemC");
            }
/*            else if (readData[1] == (byte) MicroBattleSerialMessageStructure.SystemA)
            {
                Debug.Log("SystemA");
            }*/
            else if (readData[1] == (byte) MicroBattleSerialMessageStructure.SystemB)
            {
                OnGetSystemButtonB?.Invoke();
                Debug.Log("SystemB");
            }
            
            
/*            _dataBuffer[dataIndex] = readData;
            dataIndex += 1;
            //index ==4 のとき
            if (dataIndex == 4)
            {
                dataIndex = 0;
                if (_dataBuffer[0] != 200 && _dataBuffer[3] !=255)
                {
                    return;
                }

                if (_dataBuffer[1] == (byte) MicroBattleSerialMessageStructure.Rot)
                {
                    InvokeGetRot((MicroBattleRotStructure) _dataBuffer[2]);
                }
                else if (_dataBuffer[1] == (byte) MicroBattleSerialMessageStructure.Move)
                {
                    InvokeGetMove((MicroBattleMoveStructure) _dataBuffer[2]);
                }
                else if (_dataBuffer[1] == (byte) MicroBattleSerialMessageStructure.Shot)
                {
                }
                else if (_dataBuffer[1] == (byte) MicroBattleSerialMessageStructure.Bomb)
                {
                }
            }*/
        }

        private void InvokeGetRot(MicroBattleRotStructure rotStructure)
        {
            switch (rotStructure)
            {
                case MicroBattleRotStructure.Left:
                    OnGetPlayerRotData?.Invoke(RotData.Left);
                    Debug.Log("Left");
                    break;
                case MicroBattleRotStructure.Right:
                    OnGetPlayerRotData?.Invoke(RotData.Right);
                    Debug.Log("Right");
                    break;
            }
        }

        private void InvokeGetMove(MicroBattleMoveStructure moveStructure)
        {
            switch (moveStructure)
            {
                case MicroBattleMoveStructure.Front:
                    OnGetPlayerMoveData?.Invoke(MoveData.Front);
                    Debug.Log("Front");
                    break;
                case MicroBattleMoveStructure.Back:
                    OnGetPlayerMoveData?.Invoke(MoveData.Back);
                    Debug.Log("Back");
                    break;
            }
        }
        
    }
}