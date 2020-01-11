using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _PCFinal
{ 
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private BulletPoolManager _poolManager;
        
        [SyncVar]
        public int playerNumber = -1;
        public bool isTestMode = true;
        
        [SerializeField] private float moveValue = 0.03f;
        [SerializeField] private float rotValue = 0.03f;
        
        private Vector3 playerDirectionVect;
        private Vector3 playerRotationEuler;

        private GameObject virtualCameraGameObject;

        //newしたくないので変数を用意
        private Vector3 bufferPosVect;
        private Vector3 bufferRotVect;
        private Vector2 moveMessagePlayerPosBuffer;
        
        //このMessageObjectを持ち回りする
        private readonly MicroBattleMessage.PlayerMoveMessageData moveMessage = new MicroBattleMessage.PlayerMoveMessageData();
        private readonly MicroBattleMessage.PlayerRotateMessageData rotateMessage = new MicroBattleMessage.PlayerRotateMessageData();
        private readonly MicroBattleMessage.PlayerAttackMessageData attackData = new MicroBattleMessage.PlayerAttackMessageData();
        
        //弾の出現位置のオフセット。後方から射出したいときや自機からずれて射出したいときに有効に使う
        [SerializeField] private Vector3 bulletSpawnOffset;
        
        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log("StartServer!");
/*            playerNumber = netIdentity.observers[0].connectionId;*/
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            virtualCameraGameObject = GetComponentInChildren<CinemachineVirtualCamera>(true).gameObject;
            //権限持ちでない限りVirtualCameraを殺しておく
            if (!hasAuthority)
            {
                Destroy(virtualCameraGameObject);
                Debug.Log("DelVCam");
            }
            else
            {
                virtualCameraGameObject.SetActive(true);
                virtualCameraGameObject.transform.parent = null;
                Debug.Log(virtualCameraGameObject.transform.position);
            }
            
            Init();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
        }
        

        private void OnDestroy()
        {
            if (!isTestMode)
            {
                ClientMessageReceiver.OnReceivePlayerMoveMessage -= PlayerMove;
                ClientMessageReceiver.OnReceivePlayerRotateMessage -= PlayerRotate;
                ClientMessageReceiver.OnReceivePlayerAttackMessage -= PlayerAttack;
            }

            if (hasAuthority)
            {
                Destroy(virtualCameraGameObject);
            }

            Destroy(_poolManager.gameObject);
        }

        public void InitInTestMode()
        {
            var vCamObj = GetComponentInChildren<CinemachineVirtualCamera>(true).gameObject;
            Destroy(vCamObj);
            isTestMode = true;
            
            Debug.Log("Test Spawn");
            
            //独立させる
            _poolManager.transform.parent = null;
        }

        public void Init()
        {
            ClientMessageReceiver.OnReceivePlayerMoveMessage += PlayerMove;
            ClientMessageReceiver.OnReceivePlayerRotateMessage += PlayerRotate;
            ClientMessageReceiver.OnReceivePlayerAttackMessage += PlayerAttack;

            isTestMode = false;
            
            //独立させる
            _poolManager.transform.parent = null;
        }
        

        private void PlayerMove(int playerId,Vector2 playerPos)
        {
            //受け取ったplayerIDで動作させるプレイヤーかどうかを判定
            if (playerId != playerNumber)
            {
                return;
            }
            
            //権限持ちの場合は座標を更新しなくてよい?
            if (hasAuthority)
            {
                return;
            }

            bufferPosVect.x = playerPos.x;
            bufferPosVect.y = playerPos.y;
            gameObject.transform.position = bufferPosVect;

//            Debug.Log($"player {playerId} is move to X_{playerPos.x},Y_{playerPos.y}");
        }

        private void PlayerRotate(int playerId, float rot)
        {
            //受け取ったplayerIDで動作させるプレイヤーかどうかを判定
            if (playerId != playerNumber)
            {
                return;
            }
            
            //権限持ちの場合は回転を更新しなくてよい
            if (hasAuthority)
            {
                return;
            }

            bufferRotVect.z = rot;
            gameObject.transform.eulerAngles = bufferRotVect;

//            Debug.Log($"playerID:{playerId}/rot:{rot}");
//            Debug.Log($"player {playerId} is move to X_{playerPos.x},Y_{playerPos.y}");
        }

        private void PlayerAttack(int playerId)
        {
            //受け取ったplayerIDで動作させるプレイヤーかどうかを判定
            if (playerId != playerNumber)
            {
                return;
            }
            
            //権限持ちの場合はショットを発生させなくてもよい
            if (hasAuthority)
            {
                return;
            }  
            
            //プールからバレットを持ってくる
            if (_poolManager.GetFreeBullet(out var bullet))
            {
                ShotBullet(bullet);
            }
        }
        

        private void Update()
        {
            //テストモード時はこの条件はスルー
            //そもそもSpawnされないので他のPlayerはテストモード時にローカル空間内に現れない。
            //ので、この条件式でも大丈夫だが、直感的でない
            if (!isTestMode && !hasAuthority)
            {
                //権限を持っていなければ操作できない
                return;
            }

            //完全にデバッグ用
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isTestMode = !isTestMode;
            }
            
            TestControl(isTestMode);
        }

        private void TestControl(bool isTest)
        {
            if (!isTest)
            {
                return;
            }

            //移動系
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                var playerRotation = GetPlayerShipRotation(rotValue);
                gameObject.transform.rotation *= playerRotation;
                SendRotateData();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                var playerRotation = GetPlayerShipRotation(-rotValue);
                gameObject.transform.rotation *= playerRotation;
                SendRotateData();
            }
            
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                var moveVector = GetPlayerShipFront();
                gameObject.transform.position += (moveValue * Time.deltaTime * moveVector);
                SendMoveData();

            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                var moveVector = GetPlayerShipFront();
                gameObject.transform.position += (-moveValue * Time.deltaTime * moveVector);
                SendMoveData();
            }


            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (_poolManager.GetFreeBullet(out var bullet))
                {
//                    Debug.Log("Get Free Bullet");
                    ShotBullet(bullet);
                    
                    attackData.playerId = playerNumber;
                    NetworkClient.Send(attackData);
                }
            }
            
            //
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


        //正面方向の取得
        private Vector3 GetPlayerShipFront()
        {
            var angleDir = transform.eulerAngles.z * (Mathf.PI / 180.0f);
            
            playerDirectionVect.x = -Mathf.Sin(angleDir);
            playerDirectionVect.y = Mathf.Cos(angleDir);
            playerDirectionVect.z = 0;

            return playerDirectionVect;
        }
        
        #region 回転計算と送信
        private Quaternion GetPlayerShipRotation(float rotWeight)
        {
            playerRotationEuler.z = rotWeight * Time.deltaTime;
            return Quaternion.Euler(playerRotationEuler);
        }
        
        private void SendRotateData()
        {
            var rot = transform.eulerAngles.z;
            rotateMessage.playerId = playerNumber;
            rotateMessage.mPlayerRot = rot;
            NetworkClient.Send(rotateMessage);
        }
        #endregion
        
        #region 移動計算と送信
        private void SendMoveData()
        {
            var position = transform.position;
            moveMessagePlayerPosBuffer.x = position.x;
            moveMessagePlayerPosBuffer.y = position.y;
                
            moveMessage.playerId = playerNumber;
            moveMessage.mPlayerPos = moveMessagePlayerPosBuffer;
            
            NetworkClient.Send(moveMessage);
        }
        
        #endregion

        private void ShotBullet(PlayerBulletBase bullet)
        {
            var front = GetPlayerShipFront();

            //自機の回転からQuaternionを取得
            var playerTransform = transform;
            var playerQuaternion = Quaternion.Euler(playerTransform.eulerAngles);
            var offsetResult = playerQuaternion * bulletSpawnOffset;

            var p = playerTransform.position + (front * Time.deltaTime) + offsetResult;
            var r = playerTransform.rotation;
                    
            bullet.SpawnBullet(p, r);
        }
    }
}