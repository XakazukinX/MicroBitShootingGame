using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _20191114
{
    public enum PowerState
    {
        Off = 0,
        On
    }
    public enum MicrowaveState
    {
       Inactive = 0,
       Active,
    }
    public enum MicrowaveDoorState
    {
        Close = 0,
        Open,
    }
    
    public class MicrowaveManageTest20191114 : MonoBehaviour
    {
        private Color activeLampColor = Color.red;
        private Color inactiveLampColor= Color.white;
        
        private PowerState _powerState = PowerState.Off;
        [SerializeField] private Image _powerStateImage;
        
        private MicrowaveState _microwaveState = MicrowaveState.Inactive;
        [SerializeField] private Image _microwaveStateImage;

        private MicrowaveDoorState _doorState = MicrowaveDoorState.Close;
        [SerializeField] private Text _doorStateText;
        
        private void Start()
        {
            InitStates();
        }
        private void InitStates()
        {
            _powerStateImage.color = inactiveLampColor;
            _microwaveStateImage.color = inactiveLampColor;

            _powerState = PowerState.Off;
            _microwaveState = MicrowaveState.Inactive;
        }
        private void MicrowaveStop()
        {
            _microwaveState = MicrowaveState.Inactive;
            _microwaveStateImage.color = inactiveLampColor;
        }

        

        public void OnClickPowerButton()
        {
            switch (_powerState)
            {
                case PowerState.Off:
                    _powerState = PowerState.On;
                    _powerStateImage.color = activeLampColor;
                    break;
                case PowerState.On:
                    InitStates();
                    break;
            }
        }
        public void OnClickMicrowaveOperationButton()
        {
            if (_powerState == PowerState.Off) return;
            if (_doorState == MicrowaveDoorState.Open) return;
            switch (_microwaveState)
            {
                case MicrowaveState.Inactive:
                    _microwaveState = MicrowaveState.Active;
                    _microwaveStateImage.color = activeLampColor;
                    break;
                case MicrowaveState.Active:
                    MicrowaveStop();
                    break;
            }
        }
        public void OnClickOpenMicrowaveDoorButton()
        {
            switch (_doorState)
            {
                case MicrowaveDoorState.Open:
                    _doorState = MicrowaveDoorState.Close;
                    _doorStateText.text = "Close!";
                    break;
                
                case MicrowaveDoorState.Close:
                    _doorState = MicrowaveDoorState.Open;
                    _doorStateText.text = "Open!";
                    if (_microwaveState == MicrowaveState.Active)
                    {
                        _microwaveState = MicrowaveState.Inactive;
                        _microwaveStateImage.color = inactiveLampColor;
                    }
                    break;
            }
        }
    }
}

