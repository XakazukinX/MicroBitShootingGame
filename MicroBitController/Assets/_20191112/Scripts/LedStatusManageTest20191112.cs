using UnityEngine;
using UnityEngine.UI;

namespace _20191112
{
    public enum PowerStatus
    {
        Off,
        On,
    }
    public enum LedStatus
    {
        Dim,
        Bright,
    }
    public class LedStatusManageTest20191112 : MonoBehaviour
    {
        //0がオフ1がオン色
        [SerializeField] private Color[] statusColor = new Color[2];
        [SerializeField] private Image _powerStateImage;
        [SerializeField] private Image _ledStateImage;
        
        public PowerStatus powerStatus;
        public LedStatus ledStatus;

        private void Start()
        {
            ledStatus = LedStatus.Dim;
            powerStatus = PowerStatus.Off;
            
            _powerStateImage.color = statusColor[(int) powerStatus];
            _ledStateImage.color = statusColor[(int) ledStatus];
        }
        public void OnClickPowerButton()
        {
            switch (powerStatus)
            {
                case PowerStatus.Off:
                    powerStatus = PowerStatus.On;
                    break;
                case PowerStatus.On:
                    powerStatus = PowerStatus.Off;
                    if (ledStatus == LedStatus.Bright)
                    {
                        ledStatus = LedStatus.Dim;
                        _ledStateImage.color = statusColor[(int) ledStatus];
                    }
                    break;
            }
            _powerStateImage.color = statusColor[(int) powerStatus];
        }
        public void OnClickLedButton()
        {
            if (powerStatus != PowerStatus.On) return;
            switch (ledStatus)
            {
                case LedStatus.Bright:
                    ledStatus = LedStatus.Dim;
                    break;
                case LedStatus.Dim:
                    ledStatus = LedStatus.Bright;
                    break;
            }
            _ledStateImage.color = statusColor[(int) ledStatus];
        }
    }
}