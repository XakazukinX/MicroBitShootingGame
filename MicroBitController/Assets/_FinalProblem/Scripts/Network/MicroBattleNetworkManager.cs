using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _PCFinal
{
    public class MicroBattleNetworkManager : NetworkManager
    {
        public List<PlayerSpaceShipProfile> playerSpaceShipProfiles = new List<PlayerSpaceShipProfile>();
/*    [SerializeField] private GameObject ServerMessageReceiver;*/
        [SerializeField] private GameObject ClientMessageReceiver;
/*    private GameObject serverReceiverObject;*/

        private GameObject clientReceiverObject;

        public override void OnStartHost()
        {
            base.OnStartHost();
/*        serverReceiverObject = Instantiate(ServerMessageReceiver);*/
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            clientReceiverObject = Instantiate(ClientMessageReceiver);
        }


        public override void OnStopHost()
        {
            base.OnStopHost();
/*        Destroy(serverReceiverObject);*/
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            Destroy(clientReceiverObject);
        }
    }
}