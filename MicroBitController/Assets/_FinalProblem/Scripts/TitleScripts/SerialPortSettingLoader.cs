using System;
using System.IO;
using SerialPortUtility;
using UnityEngine;

namespace _FinalProblem.Scripts.TitleScripts
{
    public class SerialPortSettingLoader : MonoBehaviour
    {
        [SerializeField] private GameObject SerialPortManager;
#if UNITY_EDITOR
        [ContextMenu("TestLoad")]
        public void TestLoad()
        {
            Debug.Log(Application.streamingAssetsPath);
            if (!File.Exists(Application.streamingAssetsPath + "/serialSetting.json"))
            {
                var data = new SerialPortSetting()
                {
                    baudRate = BaudRate.B_57600,
                    selectedPort = 0,
                    targetPortName = "SerialName"
                };
                var jsonFile = JsonUtility.ToJson(data);
/*            File.Create(Application.streamingAssetsPath + "/serialSetting.json");*/
                File.WriteAllText(Application.streamingAssetsPath + "/serialSetting.json", jsonFile);
            }
            else
            {
                var jsonText = File.ReadAllText(Application.streamingAssetsPath + "/serialSetting.json");
                var data = JsonUtility.FromJson<SerialPortSetting>(jsonText);
                Debug.Log(data.baudRate);
                Debug.Log(data.targetPortName);
            }
        }
#endif

        private void Start()
        {
            LoadSerialSettingJson();
        }

        private void LoadSerialSettingJson()
        {
            var jsonText = File.ReadAllText(Application.streamingAssetsPath + "/serialSetting.json");
            var data = JsonUtility.FromJson<SerialPortSetting>(jsonText);
            var serialPotManager = Instantiate(SerialPortManager);
            
            var manager = serialPotManager.GetComponent<MicroBitSerialPortManager>();
            manager.SerialPortSetting = data;
            manager.Init(data);
        }
    }
}
