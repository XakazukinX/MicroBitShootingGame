using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ClientMessageReceiver : MonoBehaviour
{
    
    //移動受信時のアクション
    public static Action<int, Vector2> OnReceivePlayerMoveMessage;
    //移動受信時のアクション
    public static Action<int, float> OnReceivePlayerRotateMessage;
    //攻撃受信時のアクション
    public static Action<int> OnReceivePlayerAttackMessage;
    //ボム受信時のアクション
    public static Action<int, int> OnReceivePlayerUseBombMessage;
    //プレイヤー死亡受信時のアクション
    public static Action<int> OnReceivePlayerDeathMessage;
    
/*    //自機決定受信時のアクション
    public static Action<int> OnReceiveSelectedSpaceShip;*/
    
    private void OnEnable()
    {
        //移動受信ハンドラ
        NetworkClient.RegisterHandler<MicroBattleMessage.PlayerMoveMessageData>(ReceivedMoveInfo);
        //回転受信ハンドラ
        NetworkClient.RegisterHandler<MicroBattleMessage.PlayerRotateMessageData>(ReceivedRotateInfo);
        //攻撃受信ハンドラ
        NetworkClient.RegisterHandler<MicroBattleMessage.PlayerAttackMessageData>(ReceivedAttackInfo);
        //ボム使用受信ハンドラ
        NetworkClient.RegisterHandler<MicroBattleMessage.PlayerUseBombMessageData>(ReceivedUseBombInfo);
        //死亡受信ハンドラ
        NetworkClient.RegisterHandler<MicroBattleMessage.PlayerDeathMessageData>(ReceivedDeathInfo);
        
/*        //自機決定受信ハンドラ
        NetworkClient.RegisterHandler<MicroBattleMessage.SelectedPlayerData>(ReceivedSelectedPlayerInfo);*/
    }

    private void OnDestroy()
    {
        
    }

    //Clientで登録するコールバックメソッド
    private void ReceivedMoveInfo(NetworkConnection conn, MicroBattleMessage.PlayerMoveMessageData receivedData)
    {
        OnReceivePlayerMoveMessage?.Invoke(receivedData.playerId, receivedData.mPlayerPos);
    }
    
    private void ReceivedRotateInfo(NetworkConnection conn, MicroBattleMessage.PlayerRotateMessageData receivedData)
    {
        OnReceivePlayerRotateMessage?.Invoke(receivedData.playerId, receivedData.mPlayerRot);
    }
        
    private void ReceivedAttackInfo(NetworkConnection conn, MicroBattleMessage.PlayerAttackMessageData receivedData)
    {
        OnReceivePlayerAttackMessage?.Invoke(receivedData.playerId);
    }
        
    private void ReceivedUseBombInfo(NetworkConnection conn, MicroBattleMessage.PlayerUseBombMessageData receivedData)
    {
        OnReceivePlayerUseBombMessage?.Invoke(receivedData.playerId, receivedData.bombId);
    }
        
    private void ReceivedDeathInfo(NetworkConnection conn, MicroBattleMessage.PlayerDeathMessageData receivedData)
    {
        OnReceivePlayerDeathMessage?.Invoke(receivedData.playerId);
    }
    
/*    private void ReceivedSelectedPlayerInfo(NetworkConnection conn, MicroBattleMessage.SelectedPlayerData receivedData)
    {
        OnReceiveSelectedSpaceShip?.Invoke(receivedData.selectedIndex);
    }*/
}
