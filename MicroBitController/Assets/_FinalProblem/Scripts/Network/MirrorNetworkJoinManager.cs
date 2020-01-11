using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using _PCFinal.TitleScene;
using Mirror;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MirrorNetworkJoinManager : MonoBehaviour
{
    [SerializeField] private InputField IpAddressInputField;
    private IPAddress ipAddress;
    private NetworkManager _networkManager;
    private TitleSceneManager _titleSceneManager;

    void Awake()
    {
        if (IpAddressInputField == null)
        {
            var inputObj= GameObject.Find("IpAddressInputField");
            if (IpAddressInputField == null)
            {
                Debug.Log("There is no ip address Input Field. See you. I'll die");
                Destroy(this);
            }

            IpAddressInputField = inputObj.GetComponent<InputField>();
            Debug.Log("You Forgot assign input field so i find \"IpAddressInputField\" and set it");
        }
        
        _networkManager = GetComponent<NetworkManager>();
    }

    private void Start()
    {
        IpAddressInputField.text
            .ObserveEveryValueChanged(x => IpAddressInputField.text)
            .Select(x => IPAddress.TryParse(x, out ipAddress))
            .Subscribe(x =>
            {
                if (!x) return;
                _networkManager.networkAddress = ipAddress.ToString();
/*                Debug.Log("ip address is changed=>" + ipAddress);*/
            });
    }

    public void LanHost()
    {
        if (NetworkClient.isConnected || NetworkServer.active) return;
        if (NetworkClient.active) return;
        // LAN Host
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            _networkManager.StartHost();
        }
    }

    public void LanClient()
    {
        if (NetworkClient.isConnected || NetworkServer.active) return;
        if (NetworkClient.active) return;
        // LAN Host
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            _networkManager.StartClient();
        }
    }

    public void Stop()
    {
        // stop
        if (!NetworkServer.active && !NetworkClient.isConnected) return;
        _networkManager.StopHost();
    }
    
    
    
    /*

    void OnGUI()
    {
        if (!showGUI)
            return;

        GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (!NetworkClient.active)
            {
                // LAN Host
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("LAN Host"))
                    {
                        manager.StartHost();
                    }
                }

                // LAN Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("LAN Client"))
                {
                    manager.StartClient();
                }

                //ここの処理UniRxとかで書くとよさげ
                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                GUILayout.EndHorizontal();

                // LAN Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                else
                {
                    if (GUILayout.Button("LAN Server Only")) manager.StartServer();
                }
            }
            else
            {
                // Connecting
                GUILayout.Label("Connecting to " + manager.networkAddress + "..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    manager.StopClient();
                }
            }
        }
        else
        {
            // server / client status message
            if (NetworkServer.active)
            {
                GUILayout.Label("Server: active. Transport: " + Transport.activeTransport);
            }

            if (NetworkClient.isConnected)
            {
                GUILayout.Label("Client: address=" + manager.networkAddress);
            }
        }

        // client ready
        if (NetworkClient.isConnected && !ClientScene.ready)
        {
            if (GUILayout.Button("Client Ready"))
            {
                ClientScene.Ready(NetworkClient.connection);

                if (ClientScene.localPlayer == null)
                {
                    ClientScene.AddPlayer();
                }
            }
        }

        // stop
        if (NetworkServer.active || NetworkClient.isConnected)
        {
            if (GUILayout.Button("Stop"))
            {
                manager.StopHost();
            }
        }

        GUILayout.EndArea();
    }*/
}
