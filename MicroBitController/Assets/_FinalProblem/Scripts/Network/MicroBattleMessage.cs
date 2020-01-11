using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MicroBattleMessage
{
    
    [System.Serializable]
    public class PlayerMoveMessageData : MessageBase
    {
        public int playerId;
        public Vector2 mPlayerPos;
    }
    
    [System.Serializable]
    public class PlayerRotateMessageData : MessageBase
    {
        public int playerId;
        public float mPlayerRot;
    }

    [System.Serializable]
    public class PlayerAttackMessageData : MessageBase
    {
        public int playerId;
    }

    [System.Serializable]
    public class PlayerUseBombMessageData : MessageBase
    {
        public int playerId;
        public int bombId;
    }

    [System.Serializable]
    public class PlayerDeathMessageData : MessageBase
    {
        public int playerId;
    }

    [System.Serializable]
    public class SelectedPlayerData : MessageBase
    {
        public int selectedIndex;
        public GameObject spawnObject;
    }

}
