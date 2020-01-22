using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _PCFinal
{
    public class MicroBattleServerManager : MonoBehaviour
    {
        [SerializeField] private MicroBattleNetworkManager _networkManager;
        private Dictionary<int, PlayerController> spaceShipDictionary = new Dictionary<int, PlayerController>();

        public void Update()
        {
           // Debug.Log($"SpaceShipDictCount {spaceShipDictionary.Count}");
        }

        private void OnEnable()
        {
            //辞書の初期化
            spaceShipDictionary = new Dictionary<int, PlayerController>();
            #region ハンドラの登録

            //移動受信ハンドラ
            NetworkServer.RegisterHandler<MicroBattleMessage.PlayerMoveMessageData>(ReceiveClientMoveMessage);
            //回転受信ハンドラ
            NetworkServer.RegisterHandler<MicroBattleMessage.PlayerRotateMessageData>(ReceiveClientRotateMessage);
            //攻撃受信ハンドラ
            NetworkServer.RegisterHandler<MicroBattleMessage.PlayerAttackMessageData>(ReceiveClientAttackMessage);
            //ボム使用受信ハンドラ
            NetworkServer.RegisterHandler<MicroBattleMessage.PlayerUseBombMessageData>(ReceiveClientUseBombMessage);
            //死亡受信ハンドラ
            NetworkServer.RegisterHandler<MicroBattleMessage.PlayerDeathMessageData>(ReceiveClientDeathMessage);

            //SpaceShipセレクト受信ハンドラ
            NetworkServer.RegisterHandler<MicroBattleMessage.SelectedPlayerData>(ReceiveSelectedPlayer);
            
            //SpaceShipセレクト受信ハンドラ
            NetworkServer.RegisterHandler<MicroBattleMessage.PlayerDamageMessageData>(ReceiveClientDamageMessage);

            #endregion
        }

        # region Server時に登録するコールバックメソッド

        private void ReceiveClientMoveMessage(NetworkConnection conn, MicroBattleMessage.PlayerMoveMessageData message)
        {
            NetworkServer.SendToAll(message);
        }

        private void ReceiveClientRotateMessage(NetworkConnection conn,
            MicroBattleMessage.PlayerRotateMessageData message)
        {
            NetworkServer.SendToAll(message);
        }

        private void ReceiveClientAttackMessage(NetworkConnection conn,
            MicroBattleMessage.PlayerAttackMessageData message)
        {
            NetworkServer.SendToAll(message);
        }

        private void ReceiveClientUseBombMessage(NetworkConnection conn,
            MicroBattleMessage.PlayerUseBombMessageData message)
        {
            NetworkServer.SendToAll(message);
        }

        private void ReceiveClientDeathMessage(NetworkConnection conn,
            MicroBattleMessage.PlayerDeathMessageData message)
        {
            NetworkServer.SendToAll(message);
        }
        
        private void ReceiveClientDamageMessage(NetworkConnection conn,
            MicroBattleMessage.PlayerDamageMessageData message)
        {
            spaceShipDictionary[conn.connectionId].playerHp -= 1;
            if (spaceShipDictionary[conn.connectionId].playerHp > 0)
            {
                //クソ設計
                var task = spaceShipDictionary[conn.connectionId].Damage();
            }
            else
            {
                //Hp0以下だったらメッセ発行して殺す
                var deathMessage = new MicroBattleMessage.PlayerDeathMessageData
                {
                    playerId = conn.connectionId
                };
                
                spaceShipDictionary.Remove(conn.connectionId);
                Debug.Log($"{spaceShipDictionary.TryGetValue(conn.connectionId, out var val)}");
                NetworkServer.SendToAll(deathMessage);
            }
        }

        private void ReceiveSelectedPlayer(NetworkConnection conn, MicroBattleMessage.SelectedPlayerData message)
        {
            Debug.Log("Select Player!");
            var spaceShip = _networkManager.playerSpaceShipProfiles[message.selectedIndex].spaceShipObject;
            var obj = Instantiate(spaceShip);
            var pc = obj.GetComponent<PlayerController>();
            pc.playerNumber = conn.connectionId;
            //辞書に登録
            spaceShipDictionary.Add(conn.connectionId, pc);
            NetworkServer.Spawn(obj, conn);
            
//            pc.Init();

/*            NetworkServer.AddPlayerForConnection(conn, obj);*/
/*            NetworkServer.ReplacePlayerForConnection(conn, obj);*/
            //NetworkServer.SendToAll(message);
        }
        #endregion
    }
}
