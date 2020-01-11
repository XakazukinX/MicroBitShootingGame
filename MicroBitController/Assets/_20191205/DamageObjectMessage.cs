using System;
using Mirror;
using UnityEngine;

namespace _20191205
{
    [Serializable]
    public class DamageObjectMessage : MessageBase
    {
        public Vector3 damageObjectFallingPos = new Vector3();
    }

    [Serializable]
    public class PlayerPosMessage : MessageBase
    {
        public Vector3 playerPos = new Vector3();
    }
}