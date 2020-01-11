using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerDataSendTester : NetworkBehaviour
{
    [SyncVar]
    public int playerNumber = -1;

    public override void OnStartServer()
    {
        base.OnStartServer();
        playerNumber = connectionToClient.connectionId;
//        Debug.Log(connectionToClient.connectionId);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }


    private void Update()
    {
        if (!isLocalPlayer)
        {
            //ローカルのオブジェクトでなければSendできる必要はないので殺す
            return;
            Destroy(this);
        }

/*        //PlayerMoveMessage
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            var data = new MicroBattleMessage.PlayerMoveMessageData()
            {
                playerId = playerNumber,
                mMoveValue = Random.Range(-3.0f,3.0f)
            };

            NetworkClient.Send(data);
        }*/
        
        //PlayerAttackMessage
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            var data = new MicroBattleMessage.PlayerAttackMessageData()
            {
                playerId = playerNumber,
            };

            NetworkClient.Send(data);
        }
        
        
        //PlayerUseBombMessage
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            var data = new MicroBattleMessage.PlayerUseBombMessageData()
            {
                playerId = playerNumber,
                bombId = 1
            };

            NetworkClient.Send(data);
        }
        
        //PlayerDeathMessage
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            var data = new MicroBattleMessage.PlayerDeathMessageData()
            {
                playerId = playerNumber,
            };
            
            NetworkClient.Send(data);
        }
        
        
    }
}
