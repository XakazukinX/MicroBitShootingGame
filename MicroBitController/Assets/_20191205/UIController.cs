using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _20191205
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject GameOverUI;
        
        private void OnEnable()
        {
            GameOverUI.SetActive(false);
            SphereController.OnGetDamage += OnSphereController;
        }

        private void OnSphereController()
        {
            GameOverUI.SetActive(true);
        }
    }
}