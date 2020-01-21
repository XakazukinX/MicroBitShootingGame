using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using Mirror;
using UniRx;
using UniRx.Async;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _PCFinal
{ 
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private BulletPoolManager poolManager;
        [SerializeField] private PlayerInfoUIUpdater playerInfoUiUpdater;
        
        private Animator _spaceShipAnimator;
        [SyncVar] public int playerHp;
        [SyncVar] public int playerNumber = -1;
        [SyncVar] public bool isDamage;
        public bool isTestMode = true;


        [Header("このSpaceShipの能力値を指定")]
        //これProfileから引っ張ってこれるようにした方がいいよね
        [SerializeField] private int maxHp = 10;
        [SerializeField] private float moveValue = 0.03f;
        [SerializeField] private float rotValue = 0.03f;
        [SerializeField] private int bulletCount = 10;
        [SerializeField] private int damageWaitTime = 2500;

        private Vector3 _playerDirectionVector;
        private Vector3 _playerRotationEuler;

        private GameObject _virtualCameraGameObject;

        //newしたくないので変数を用意
        private Vector3 _bufferPosVector;
        private Vector3 _bufferRotVector;
        private Vector2 _moveMessagePlayerPosBuffer;
        
        //このMessageObjectを持ち回りする
        private readonly MicroBattleMessage.PlayerMoveMessageData _moveMessage = new MicroBattleMessage.PlayerMoveMessageData();
        private readonly MicroBattleMessage.PlayerRotateMessageData _rotateMessage = new MicroBattleMessage.PlayerRotateMessageData();
        private readonly MicroBattleMessage.PlayerAttackMessageData _attackData = new MicroBattleMessage.PlayerAttackMessageData();
        private readonly MicroBattleMessage.PlayerDamageMessageData _damageData = new MicroBattleMessage.PlayerDamageMessageData();
        
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
            
            _virtualCameraGameObject = GetComponentInChildren<CinemachineVirtualCamera>(true).gameObject;
            //権限持ちでない限りVirtualCameraを殺しておく
            if (!hasAuthority)
            {
                Destroy(_virtualCameraGameObject);
                Debug.Log("DelVCam");
                //UIの初期化
                playerInfoUiUpdater.Dispose();
            }
            else
            {
                _virtualCameraGameObject.SetActive(true);
                _virtualCameraGameObject.transform.parent = null;
                
                //UIの初期化
                playerInfoUiUpdater.Init(maxHp,bulletCount);
                Debug.Log(_virtualCameraGameObject.transform.position);
            }
            
            Init();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
        }

        private void OnEnable()
        {
            _spaceShipAnimator = GetComponent<Animator>();
            this.ObserveEveryValueChanged(x => x.isDamage).Subscribe(damage =>
            {
                if (damage)
                {
                    _spaceShipAnimator.Play("Damage");
                }
                else
                {
                    _spaceShipAnimator.Play("default");                    
                }
            });
            /*

            .ObserveEveryValueChanged(x => x.isGrounded)
            .Where(x => x)
            .Subscribe(_ => Debug.Log("OnGrounded!"));*/
        }

        private void OnDestroy()
        {

        }

        public void InitInTestMode()
        {
            var vCamObj = GetComponentInChildren<CinemachineVirtualCamera>(true).gameObject;
            Destroy(vCamObj);
            isTestMode = true;
            
            Debug.Log("Test Spawn");
            poolManager.InitBulletPool(bulletCount);

            //独立させる
            poolManager.transform.parent = null;
        }

        public void Init()
        {
            
            //サーバから各種メッセージを受け取った時
            ClientMessageReceiver.OnReceivePlayerMoveMessage += PlayerMove;
            ClientMessageReceiver.OnReceivePlayerRotateMessage += PlayerRotate;
            ClientMessageReceiver.OnReceivePlayerAttackMessage += PlayerAttack;
            ClientMessageReceiver.OnReceivePlayerDeathMessage += PlayerDeath;

            //シリアル通信でバイト配列を受け取った時
            SerialMessagePresenter.OnGetPlayerMoveData += GetPlayerMoveData;
            SerialMessagePresenter.OnGetPlayerRotData += GetPlayerRotData;
            SerialMessagePresenter.OnGetPlayerShot += GetPlayerShot;

            isTestMode = false;
            poolManager.InitBulletPool(bulletCount);
            poolManager.SetBulletAuthorId(playerNumber);
            //独立させる
            poolManager.transform.parent = null;
        }

        //破棄用の関数
        public void Dispose()
        {
            if (!isTestMode)
            {
                ClientMessageReceiver.OnReceivePlayerMoveMessage -= PlayerMove;
                ClientMessageReceiver.OnReceivePlayerRotateMessage -= PlayerRotate;
                ClientMessageReceiver.OnReceivePlayerAttackMessage -= PlayerAttack;
                ClientMessageReceiver.OnReceivePlayerDeathMessage -= PlayerDeath;
            }

            if (hasAuthority)
            {
                Destroy(_virtualCameraGameObject);
            }

            Destroy(poolManager.gameObject);
            Destroy(this.gameObject);
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

            _bufferPosVector.x = playerPos.x;
            _bufferPosVector.y = playerPos.y;
            gameObject.transform.position = _bufferPosVector;

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

            _bufferRotVector.z = rot;
            gameObject.transform.eulerAngles = _bufferRotVector;

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
            if (poolManager.GetFreeBullet(out var bullet))
            {
                ShotBullet(bullet);
            }
        }
        
        private void PlayerDeath(int playerId)
        {
            //受け取ったplayerIDで動作させるプレイヤーかどうかを判定
            if (playerId != playerNumber)
            {
                return;
            }
            
            Debug.Log($"player ID :{playerId} is Death");
            
            this.Dispose();
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

        #region テスト用
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
                if (poolManager.GetFreeBullet(out var bullet))
                {
//                    Debug.Log("Get Free Bullet");
                    ShotBullet(bullet);
                    
                    _attackData.playerId = playerNumber;
                    NetworkClient.Send(_attackData);
                    
                    playerInfoUiUpdater.ShotBullet();
                }
            }
            
            /*
            //PlayerUseBombMessage
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                var data = new MicroBattleMessage.PlayerUseBombMessageData()
                {
                    playerId = playerNumber,
                    bombId = 1
                };

                NetworkClient.Send(data);
            }*/
        }
        #endregion

        #region MicroBitからの入力に対するフィードバック処理各種

        private void GetPlayerMoveData(MoveData data)
        {
            Debug.Log(data);
            
            var moveVector = GetPlayerShipFront();

            if (data == MoveData.Front)
            {
                gameObject.transform.position += (moveValue * Time.deltaTime * moveVector);

            }
            else if (data == MoveData.Back)
            {
                gameObject.transform.position += (-moveValue * Time.deltaTime * moveVector);
            }

            SendMoveData();

        }
        private void GetPlayerRotData(RotData data)
        {
            Debug.Log(data);
            if (data == RotData.Left)
            {
                var playerRotation = GetPlayerShipRotation(rotValue);
                gameObject.transform.rotation *= playerRotation;
                SendRotateData();
            }
            else if (data == RotData.Right)
            {
                var playerRotation = GetPlayerShipRotation(-rotValue);
                gameObject.transform.rotation *= playerRotation;
                SendRotateData();
            }
        }
        
        private void GetPlayerShot()
        {
            if (poolManager.GetFreeBullet(out var bullet))
            {
                ShotBullet(bullet);
                    
                _attackData.playerId = playerNumber;
                NetworkClient.Send(_attackData);
                    
                playerInfoUiUpdater.ShotBullet();
            }
        }

        #endregion
        
        
        //正面方向の取得
        private Vector3 GetPlayerShipFront()
        {
            var angleDir = transform.eulerAngles.z * (Mathf.PI / 180.0f);
            
            _playerDirectionVector.x = -Mathf.Sin(angleDir);
            _playerDirectionVector.y = Mathf.Cos(angleDir);
            _playerDirectionVector.z = 0;

            return _playerDirectionVector;
        }
        
        #region 回転計算と送信
        private Quaternion GetPlayerShipRotation(float rotWeight)
        {
            _playerRotationEuler.z = rotWeight * Time.deltaTime;
            return Quaternion.Euler(_playerRotationEuler);
        }
        
        private void SendRotateData()
        {
            var rot = transform.eulerAngles.z;
            _rotateMessage.playerId = playerNumber;
            _rotateMessage.mPlayerRot = rot;
            NetworkClient.Send(_rotateMessage);
        }
        #endregion
        
        #region 移動計算と送信
        private void SendMoveData()
        {
            var position = transform.position;
            _moveMessagePlayerPosBuffer.x = position.x;
            _moveMessagePlayerPosBuffer.y = position.y;
                
            _moveMessage.playerId = playerNumber;
            _moveMessage.mPlayerPos = _moveMessagePlayerPosBuffer;
            
            NetworkClient.Send(_moveMessage);
        }
        
        #endregion

        #region 弾の発射処理
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
        #endregion

        #region 被弾処理
        public void OnCollisionEnter2D(Collision2D other)
        {
            //文字列生成してGC走るのであとでScriptableObjectのロードとかで変数化する
            if (other.gameObject.CompareTag("Bullet"))
            {
                var bullet = other.gameObject.GetComponent<PlayerBulletBase>();
                //自分のBulletでは自分は食らわない
                //ダメージWait中でも食らわない
                if (bullet.bulletAuthorPlayerId == playerNumber || isDamage)
                {
                    return;
                }
                bullet.HitBullet();
                
                //所有権餅でないダメージ通知は送信しない
                if (!hasAuthority)
                {
                    return;
                }

                playerInfoUiUpdater.DamageHp(1);
                
                _damageData.playerId = playerNumber;
                NetworkClient.Send(_damageData);
                
            }
        }
        
        public async UniTask Damage()
        {
            isDamage = true;
            await UniTask.Delay(damageWaitTime);
            isDamage = false;
        }

        private void PlayerDeath()
        {
            //
            //死んだときの処理
            //
        }

        #endregion
        
    }
}