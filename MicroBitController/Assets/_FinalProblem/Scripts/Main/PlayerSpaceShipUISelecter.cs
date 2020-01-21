﻿using System;
using System.Collections;
using System.Collections.Generic;
using _PCFinal;
using DG.Tweening;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace _PCFinal
{
    //ごみ～～～～～～～～～～～～～～～！！！！！！！！！！！！！ゴミ設計！カス！
    public class PlayerSpaceShipUISelecter : MonoBehaviour
    {
        public Transform pivotTransform;
        public Transform testPlayerParent;
        
        [SerializeField] private RectTransform UIParentTransform;
        
        [SerializeField] private Text spaceShipNameText;
        [SerializeField] private Text spaceShipExplanationText;
        [SerializeField] private Text bulletNameText;
        [SerializeField] private Text bulletExplanationText;

        //自機選択後のUI
        [SerializeField] private RectTransform readyPanel;
        [SerializeField] private Button selectSpaceShipButton;
        [SerializeField] private Button backToSelectSpaceShipButton;
        [SerializeField] private Button startGameAndSpawnPlayerButton;

        
        private MicroBattleNetworkManager _networkManager;
        
        private List<PlayerController> playerControllers = new List<PlayerController>(); 
        
        private bool isSpaceShipSelectMode = true;
        private int _selectedIndex = 0;

        private void OnEnable()
        {
            SerialMessagePresenter.OnGetSystemButtonB += GetSystemButtonB;
            SerialMessagePresenter.OnGetSystemButtonC += GetSystemButtonC;
        }

        private void OnDestroy()
        {
            selectSpaceShipButton.onClick.RemoveListener(OnClickSelectSpaceShipButton);
            backToSelectSpaceShipButton.onClick.RemoveListener(OnClickBackToSpaceShipSelectButton);
            startGameAndSpawnPlayerButton.onClick.RemoveListener(OnClickSpawnPlayerAndStartButton);

            SerialMessagePresenter.OnGetSystemButtonB -= GetSystemButtonB;
            SerialMessagePresenter.OnGetSystemButtonC -= GetSystemButtonC;
/*            ClearPlayerControllerLists();*/
        }
        
        private void GetSystemButtonB()
        {
            RotateSpaceShipTable(1);
        }

        private void GetSystemButtonC()
        {
            OnClickSelectSpaceShipButton();
        }

        private void Update()
        {
/*            if (!isSpaceShipSelectMode)
            {
                return;
            }*/

/*            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RotateSpaceShipTable(-1);

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RotateSpaceShipTable(1);
            }*/
            
        }

        public void Init()
        {
            UIParentTransform.gameObject.SetActive(true);
            playerControllers = new List<PlayerController>();
            
            //う～～～～～～～～～～～～～～～～～～～～～～～～～
            //ネットワークマネージャを探す
            _networkManager = FindObjectOfType<MicroBattleNetworkManager>();
            //Debug.Log(_networkManager.playerSpaceShipProfiles.Count);
            
            //ローカル内にテスト用のPlayerControllerを出現させる
            for (int i = 0; i < _networkManager.playerSpaceShipProfiles.Count; i++)
            {
                var obj = Instantiate(_networkManager.playerSpaceShipProfiles[i].spaceShipObject,
                    pivotTransform.position,
                    pivotTransform.rotation);

                //obj.transform.parent = testPlayerParent;
                obj.SetActive(false);

                var player = obj.GetComponent<PlayerController>();
                //テストモードで起動
                player.InitInTestMode();
                playerControllers.Add(player);
            }
            
            playerControllers[_selectedIndex].gameObject.SetActive(true);
            StartCoroutine(LoadSpaceShipData(_networkManager.playerSpaceShipProfiles[_selectedIndex]));
            
            //各種ボタンにコールバックイベントを追加
            selectSpaceShipButton.onClick.AddListener(OnClickSelectSpaceShipButton);
            backToSelectSpaceShipButton.onClick.AddListener(OnClickBackToSpaceShipSelectButton);
            startGameAndSpawnPlayerButton.onClick.AddListener(OnClickSpawnPlayerAndStartButton);
        }
        
        
        //PlayerControllerのリスト初期化関数
        private void ClearPlayerControllerLists()
        {
/*            foreach (var controller in playerControllers)
            {
                Destroy(controller.gameObject);
            }*/
            playerControllers = new List<PlayerController>();
        }

        #region 自機選択
        public void RotateSpaceShipTable(int addCount)
        {
            if (!isSpaceShipSelectMode)
            {
                return;
            }
            
            //加算前のPlayerObjectを非表示にする
            playerControllers[_selectedIndex].gameObject.SetActive(false);
            
            var maxSize = _networkManager.playerSpaceShipProfiles.Count;
            _selectedIndex = (maxSize + _selectedIndex + addCount) % maxSize;
            
            //テキストを読み込む
            StartCoroutine(LoadSpaceShipData(_networkManager.playerSpaceShipProfiles[_selectedIndex]));
            
            //加算後のPlayerObjectを表示する
            var displayPlayer = playerControllers[_selectedIndex].gameObject;
            ResetTestPlayerPosition(ref displayPlayer);
            displayPlayer.SetActive(true);
            
//            Debug.Log($"Select is :{selectedIndex}");
        }
        
        private IEnumerator LoadSpaceShipData(PlayerSpaceShipProfile profile)
        {
            spaceShipNameText.text = "";
            spaceShipExplanationText.text = " ";
            bulletNameText.text = " ";
            bulletExplanationText.text = " ";
            yield return new WaitForEndOfFrame();
            
            spaceShipNameText.DOText(profile.spaceShipName, 0.2f).SetEase(Ease.Linear);
            spaceShipExplanationText.DOText(profile.GetSpaceShipExplanation(),1).SetEase(Ease.Linear);
            bulletNameText.DOText(profile.bulletName,0.2f).SetEase(Ease.Linear);
            bulletExplanationText.DOText(profile.bulletExplanation, 1).SetEase(Ease.Linear);
        }

        private void ResetTestPlayerPosition(ref GameObject target)
        {
            target.transform.position = pivotTransform.position;
            target.transform.rotation = pivotTransform.rotation;
        }

        public void OnClickSelectSpaceShipButton()
        {
            isSpaceShipSelectMode = false;
            readyPanel.gameObject.SetActive(true);
            readyPanel.DOPunchScale(new Vector3(1.05f, 1.05f), 0.2f);
        }
        
        #endregion


        #region ゲーム開始
        
        public void OnClickBackToSpaceShipSelectButton()
        {
            isSpaceShipSelectMode = true;
            readyPanel.DOPunchScale(new Vector3(-1.55f, 1.25f), 0.2f)
                .OnComplete(() =>
                {
                    readyPanel.gameObject.SetActive(false);
                });
        }
        
        public void OnClickSpawnPlayerAndStartButton()
        {
            foreach (var controller in playerControllers)
            {
                controller.Dispose();
/*                Destroy(controller.gameObject);*/
            }
            
            UIParentTransform.gameObject.SetActive(false);
            
            var message = new MicroBattleMessage.SelectedPlayerData
            {
                selectedIndex = _selectedIndex
            };
/*            Debug.Log("Send");*/
            NetworkClient.Send(message);
        }
        #endregion
    }
}