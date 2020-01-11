using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _PCFinal
{
    public class NetworkPresentTest : MonoBehaviour
    {
        public bool moveTest;
        public bool attackTest;
        public bool bombTest;
        public bool deathTest;
        
        private void OnEnable()
        {
            ClientMessageReceiver.OnReceivePlayerMoveMessage += OnReceivedMoveMessage;
            ClientMessageReceiver.OnReceivePlayerAttackMessage += OnReceivedAttackMessage;
            ClientMessageReceiver.OnReceivePlayerUseBombMessage += OnReceivedUseBombMessage;
            ClientMessageReceiver.OnReceivePlayerDeathMessage += OnReceivedDeathMessage;
        }
        
        private void OnReceivedMoveMessage(int playerId,Vector2 playerPos)
        {
            if (!moveTest)
            {
                return;
            }
            Debug.Log($"player {playerId} is move to X_{playerPos.x},Y_{playerPos.y}");
        }
        
        private void OnReceivedAttackMessage(int playerId)
        {
            if (!attackTest)
            {
                return;
            }
            Debug.Log($"player {playerId} is Attack");
        }
        
        private void OnReceivedUseBombMessage(int playerId,int bombId)
        {
            if (!bombTest)
            {
                return;
            }
            Debug.Log($"player {playerId} is use bomb{bombId}");
        }
        
        private void OnReceivedDeathMessage(int playerId)
        {
            if (!deathTest)
            {
                return;
            }
            Debug.Log($"player {playerId} is death");
        }
    }
}