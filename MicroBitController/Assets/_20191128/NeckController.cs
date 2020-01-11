using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _20181128
{
    public class NeckController : MonoBehaviour
    {
        private static readonly Regex ButtonMatchPattern = new Regex("(Rotation)(?=:)", RegexOptions.Compiled);
        private static readonly Regex MessagePattern = new Regex("(?<=:)(.*)", RegexOptions.Compiled);

        [SerializeField] private Animator targetAnimator;
        private HumanPose _currentHumanPose;
        private HumanPoseHandler _humanPoseHandler;

        private void Start()
        {
            _humanPoseHandler = new HumanPoseHandler(targetAnimator.avatar, targetAnimator.transform);
        }

        private void OnEnable()
        {
            SerialPortReadLineReader.OnGetReadLine += OnGetReadLine;
        }

        private void OnGetReadLine(string _getData)
        {
            var commandName = ButtonMatchPattern.Match(_getData);

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
                    
                    var headMuscleValue = (float) tmp / (1024.0f / 2.0f);
//                    Debug.Log(headMuscle);
                    _humanPoseHandler.GetHumanPose(ref _currentHumanPose);
                    _currentHumanPose.muscles[14] = headMuscleValue;
                    _humanPoseHandler.SetHumanPose(ref _currentHumanPose);

                }
            }
        }
    }
}