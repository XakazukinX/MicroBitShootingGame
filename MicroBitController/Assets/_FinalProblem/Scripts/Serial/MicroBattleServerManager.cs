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
        
        private void OnEnable()
        {
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

        private void ReceiveSelectedPlayer(NetworkConnection conn, MicroBattleMessage.SelectedPlayerData message)
        {
            var spaceShip = _networkManager.playerSpaceShipProfiles[message.selectedIndex].spaceShipObject;
            var obj = Instantiate(spaceShip);
            var pc = obj.GetComponent<PlayerController>();
            pc.playerNumber = conn.connectionId;
            NetworkServer.Spawn(obj, conn);
            
//            pc.Init();

/*            NetworkServer.AddPlayerForConnection(conn, obj);*/
/*            NetworkServer.ReplacePlayerForConnection(conn, obj);*/
            //NetworkServer.SendToAll(message);
        }
        #endregion
    }
}
