using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUIUpdater : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private BulletGageManager _bulletGageManager;
    [SerializeField] private Text _scoreText;

    private bool isInitialized = false;

    /// <param name="playerMaxHp"></param>
    /// <param name="bulletCount"></param>
    public void Init(int playerMaxHp,int bulletCount)
    {
        gameObject.SetActive(true);
        
        InitHpSlider(playerMaxHp);
        _bulletGageManager.InitBulletGages(bulletCount);

        isInitialized = true;
    }
    
    public void Dispose()
    {
        Destroy(this.gameObject);
    }

    private void InitHpSlider(int maxHp)
    {
        _hpSlider.maxValue = maxHp;
        _hpSlider.value = _hpSlider.maxValue;
    }

    public void DamageHp(int damageAmount)
    {
        if(!isInitialized) return;
        _hpSlider.value -= damageAmount;
    }

    public void ShotBullet()
    {
        if(!isInitialized) return;
        _bulletGageManager.BulletGageDown();
    }

    public void ScoreUpdate(int value)
    {
        //なんか数字によってエフェクト加えるならここで。そのためにわざわざ分離してある
        int.TryParse(_scoreText.text,out var currentScore);
        _scoreText.text = (currentScore + value).ToString();
    }

}
