using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _PCFinal
{
    public enum MicroBattleSerialMessageStructure : byte
    {
        Rot = 190,
        Move = 191,
        Shot = 192,
        Bomb = 193,
        SystemA = 90,
        SystemB = 92,
        SystemC = 199
    }

    public enum MicroBattleRotStructure : byte
    {
        Left = 20,
        Right = 21
    }

    public enum MicroBattleMoveStructure : byte
    {
        Front = 21,
        Back = 20
    }
}