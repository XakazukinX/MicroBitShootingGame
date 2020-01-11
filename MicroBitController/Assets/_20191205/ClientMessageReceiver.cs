using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _20191205
{
    public class ClientMessageReceiver : MonoBehaviour
    {
        [SerializeField] private GameObject playerObject;

        private void Start()
        {
            NetworkClient.RegisterHandler<PlayerPosMessage>(GetPlayerPosMessage);
        }

        private void GetPlayerPosMessage(NetworkConnection conn, PlayerPosMessage message)
        {
            playerObject.transform.position = message.playerPos;
        }
    }

}