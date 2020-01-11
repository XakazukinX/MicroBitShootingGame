using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _20191205
{
    public class HostMessageReceiver : MonoBehaviour
    {
        public static Action<Vector3> OnGetDamageObjectFallingPos;

        private void Start()
        {
            NetworkServer.RegisterHandler<DamageObjectMessage>(GetDamageObjectMessage);
            NetworkClient.RegisterHandler<PlayerPosMessage>(GetPlayerPosMessage);
        }

        private void GetDamageObjectMessage(NetworkConnection conn ,DamageObjectMessage message)
        {
            OnGetDamageObjectFallingPos?.Invoke(message.damageObjectFallingPos);
        }
        private void GetPlayerPosMessage(NetworkConnection conn ,PlayerPosMessage message)
        {
        }
    }
}