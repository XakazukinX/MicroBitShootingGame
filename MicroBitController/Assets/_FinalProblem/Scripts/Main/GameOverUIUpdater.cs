using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace _PCFinal
{
    public class GameOverUIUpdater : MonoBehaviour
    {
        [SerializeField] private RectTransform gameOverUIParent;
        [SerializeField] private Image gameOverPanelImage;
        [SerializeField] private Color fadeEndColor;
        [SerializeField] private Text GameOverText;
        [SerializeField] private Text ScoreText;
        [SerializeField] private Text InfoText;

        private bool isInitialized;

        private void Start()
        {
            SerialMessagePresenter.OnGetSystemButtonB += GetSystemButtonB;
        }
        
        private void OnDestroy()
        {
            SerialMessagePresenter.OnGetSystemButtonB -= GetSystemButtonB;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                ReLoadPlayerSpawn();
            }
        }

        public void Init(int score)
        {
            gameOverUIParent.gameObject.SetActive(true);

            gameOverPanelImage.DOFade(fadeEndColor.a, 3.0f);

            ScoreText.text = score.ToString();
            GameOverText.DOText("Game Over", 2f);
            
            isInitialized = true;
        }

        private void GetSystemButtonB()
        {
            ReLoadPlayerSpawn();
        }

        private void ReLoadPlayerSpawn()
        {
            if (isInitialized)
            {
                ClientScene.localPlayer.gameObject.GetComponent<PlayerSpawn>().InitSpawn();
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
