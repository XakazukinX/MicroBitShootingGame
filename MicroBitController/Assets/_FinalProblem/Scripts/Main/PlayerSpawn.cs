using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _PCFinal
{
    public class PlayerSpawn : NetworkBehaviour
    {
        [SyncVar] public int playerNumber = -1;
        [SerializeField] private PlayerSpaceShipUISelecter _uiSelecter;
        
        public override void OnStartServer()
        {
            base.OnStartServer();
            playerNumber = connectionToClient.connectionId;
            Debug.Log($"StartServer! id is{playerNumber}");
        }

        public void OnEnable()
        {
        }
        

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            
            Debug.Log("Init Spawn");
            InitSpawn();
        }

        public void InitSpawn()
        {
            _uiSelecter.Init();
        }


        private void Update()
        {
/*            if (!isLocalPlayer)
            {
                //ローカルのオブジェクトでなければSendできる必要はないのではじく
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Spawn");
                var message = new MicroBattleMessage.SelectedPlayerData
                {
                    selectedIndex = 0,
                };
                NetworkClient.Send(message);
            }
            
            if (Input.GetKeyDown(KeyCode.B))
            {
/*                Debug.Log("Spawn");#1#
                var message = new MicroBattleMessage.SelectedPlayerData
                {
                    selectedIndex = 2,
                };
                NetworkClient.Send(message);
            }*/

        }
        
    }
}