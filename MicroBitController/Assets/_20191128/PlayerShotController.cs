using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _20181128
{
    public class PlayerShotController : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerObject;
        [SerializeField] private GameObject bulletObject;
        private static readonly Regex RotationMatchPattern = new Regex("(Shot)(?=:)", RegexOptions.Compiled);
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
            
            Shot();
        }

        private void Shot()
        {
            var tmpBulletObj = Instantiate(bulletObject, PlayerObject.transform.position, Quaternion.identity);
        }
    }
}