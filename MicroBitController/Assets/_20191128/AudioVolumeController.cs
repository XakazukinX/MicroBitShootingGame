using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _20191128
{
    public class AudioVolumeController : MonoBehaviour
    {
        private static readonly Regex ButtonMatchPattern = new Regex("(Rotation)(?=:)", RegexOptions.Compiled);
        private static readonly Regex MessagePattern = new Regex("(?<=:)(.*)", RegexOptions.Compiled);

        [SerializeField] private AudioSource targetAudioSource;

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
                    var audioVolume = (float) volumeValue / (1024.0f);
                    targetAudioSource.volume = audioVolume;
/*                    Debug.Log(audioVolume);*/

                }
            }
        }
    }
}