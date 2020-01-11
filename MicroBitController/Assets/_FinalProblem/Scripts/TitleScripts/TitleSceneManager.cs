using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace _PCFinal.TitleScene
{ 
    public class TitleSceneManager : MonoBehaviour
    {
        private TitleSceneState _sceneState;

        private void Start()
        {
            this.ObserveEveryValueChanged(x => _sceneState)
                .Subscribe(_ =>
                {
                    if (_sceneState == TitleSceneState.WaitJoinNetwork)
                    {
                        
                    }
                    else if (_sceneState == TitleSceneState.WaitStartButton)
                    {
                        
                    }
                });
        }

        private void OnEnable()
        {
            //最初はネットワークのJoin待ち？
            _sceneState = TitleSceneState.WaitJoinNetwork;
        }

    }
}