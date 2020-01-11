using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _20181128
{
    public class PlayerMoveController : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerObject;
        private static readonly Regex RotationMatchPattern = new Regex("(Rotation)(?=:)", RegexOptions.Compiled);
        private static readonly Regex MessagePattern = new Regex("(?<=:)(.*)", RegexOptions.Compiled);

        private Vector3 moveVec = Vector3.zero;
        
        private void OnEnable()
        {
            SerialPortReadLineReader.OnGetReadLine += OnGetReadLine;
        }

        private void OnGetReadLine(string _getData)
        {
            var commandName = RotationMatchPattern.Match(_getData);
            if (!commandName.Success)
            {
                return;
            }

            var messageName = MessagePattern.Match(_getData);
            if (messageName.Success)
            {
                if (int.TryParse(messageName.Value, out var volumeValue))
                {  
                    var tmp = (float) (512.0f - volumeValue);
                    var moveValue = (float) tmp / (1024.0f / 2.0f) / 100.0f;
                    moveVec.x = moveValue;
                }
            }
        }

        private void Update()
        {
            var playerX = PlayerObject.transform.position.x;
            
            if (playerX > 2 && moveVec.x > 0) return;
            if (playerX < -2 && moveVec.x < 0) return;
            
            PlayerObject.transform.position += moveVec;
        }
    }
}