using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _PCFinal
{
    public class PlayerDataReceiver : NetworkBehaviour
    {
        //移動受信時のアクション
        public static Action<int, Vector2> OnReceivePlayerMoveMessage;
        //攻撃受信時のアクション
        public static Action<int> OnReceivePlayerAttackMessage;
        //ボム受信時のアクション
        public static Action<int, int> OnReceivePlayerUseBombMessage;
        //プレイヤー死亡受信時のアクション
        public static Action<int> OnReceivePlayerDeathMessage;
        
        
        private void Start()
        {
/*            //移動受信ハンドラ
            NetworkClient.RegisterHandler<MicroButtleMessage.PlayerMoveMessageData>(ReceivedMoveInfo);
            //攻撃受信ハンドラ
            NetworkClient.RegisterHandler<MicroButtleMessage.PlayerAttackMessageData>(ReceivedAttackInfo);
            //ボム使用受信ハンドラ
            NetworkClient.RegisterHandler<MicroButtleMessage.PlayerUseBombMessageData>(ReceivedUseBombInfo);
            //死亡受信ハンドラ
            NetworkClient.RegisterHandler<MicroButtleMessage.PlayerDeathMessageData>(ReceivedDeathInfo);*/
        }

        public override void OnStartServer()
        {
/*            Debug.Log("OnStartServer");
            //移動受信ハンドラ
            NetworkServer.RegisterHandler<MicroButtleMessage.PlayerMoveMessageData>(ReceiveClientMoveMessage);
            //攻撃受信ハンドラ
            NetworkServer.RegisterHandler<MicroButtleMessage.PlayerAttackMessageData>(ReceiveClientAttackMessage);
            //ボム使用受信ハンドラ
            NetworkServer.RegisterHandler<MicroButtleMessage.PlayerUseBombMessageData>(ReceiveClientUseBombMessage);
            //死亡受信ハンドラ
            NetworkServer.RegisterHandler<MicroButtleMessage.PlayerDeathMessageData>(ReceiveClientDeathMessage);*/
        }

        public override void OnStartClient()
        {
/*            Debug.Log("OnStartClient");

            //移動受信ハンドラ
            NetworkClient.RegisterHandler<MicroButtleMessage.PlayerMoveMessageData>(ReceivedMoveInfo);
            //攻撃受信ハンドラ
            NetworkClient.RegisterHandler<MicroButtleMessage.PlayerAttackMessageData>(ReceivedAttackInfo);
            //ボム使用受信ハンドラ
            NetworkClient.RegisterHandler<MicroButtleMessage.PlayerUseBombMessageData>(ReceivedUseBombInfo);
            //死亡受信ハンドラ
            NetworkClient.RegisterHandler<MicroButtleMessage.PlayerDeathMessageData>(ReceivedDeathInfo);*/
        }

/*        //Server時に登録するコールバックメソッド
        private void ReceiveClientMoveMessage(NetworkConnection conn, MicroButtleMessage.PlayerMoveMessageData message)
        {
            NetworkServer.SendToAll(message);
        }
        
        private void ReceiveClientAttackMessage(NetworkConnection conn, MicroButtleMessage.PlayerAttackMessageData message)
        {
            NetworkServer.SendToAll(message);
        }
        
        private void ReceiveClientUseBombMessage(NetworkConnection conn, MicroButtleMessage.PlayerUseBombMessageData message)
        {
            NetworkServer.SendToAll(message);
        }
        
        private void ReceiveClientDeathMessage(NetworkConnection conn, MicroButtleMessage.PlayerDeathMessageData message)
        {
            NetworkServer.SendToAll(message);
        }*/
        


        
/*        //Clientで登録するコールバックメソッド
        private void ReceivedMoveInfo(NetworkConnection conn, MicroButtleMessage.PlayerMoveMessageData receivedData)
        {
            OnReceivePlayerMoveMessage?.Invoke(conn.connectionId, receivedData.playerPos);
        }
        
        private void ReceivedAttackInfo(NetworkConnection conn, MicroButtleMessage.PlayerAttackMessageData receivedData)
        {
            OnReceivePlayerAttackMessage?.Invoke(conn.connectionId);
        }
        
        private void ReceivedUseBombInfo(NetworkConnection conn, MicroButtleMessage.PlayerUseBombMessageData receivedData)
        {
            OnReceivePlayerUseBombMessage?.Invoke(conn.connectionId, receivedData.bombId);
        }
        
        private void ReceivedDeathInfo(NetworkConnection conn, MicroButtleMessage.PlayerDeathMessageData receivedData)
        {
            OnReceivePlayerDeathMessage?.Invoke(conn.connectionId);
        }*/
        
    }

}