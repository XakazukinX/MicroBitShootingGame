using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGageManager : MonoBehaviour
{
    [SerializeField] private BulletPoolManager _poolManager;
    [SerializeField] private RectTransform parentTransform;
    [SerializeField] private RectTransform pivotPosFirst;
    [SerializeField] private GameObject bulletGagePrefab;
    private readonly WaitForSeconds _recoveryTime = new WaitForSeconds(2.8f);
    private BulletGageController[] bulletGages;
    
    [ContextMenu("Test")]
    //test
    public void Test()
    {
        InitBulletGages(5);
    }

    public void InitBulletGages(int maxBullet)
    {
        bulletGages = new BulletGageController[maxBullet];
        
        for (int i = 0; i < maxBullet; i++)
        {
            var tmpObj = Instantiate(bulletGagePrefab, parentTransform);
            var rect = tmpObj.GetComponent<RectTransform>();
            rect.position = pivotPosFirst.position;
            var a = rect.sizeDelta.y * rect.localScale.y;

            var pos = pivotPosFirst.anchoredPosition;
            pos.y += a * i;
            rect.anchoredPosition = pos;

            bulletGages[i] = tmpObj.GetComponent<BulletGageController>();
/*            tmpObj.GetComponent<RectTransform>().anchoredPosition += pos;*/
        }
    }

    public void BulletGageDown()
    {
        var free = GetFreeGageIndex();
        //Debug.Log(free);
        bulletGages[free].GageDown();
        
        
        StartCoroutine(GageRecovery());

    }

    private IEnumerator GageRecovery()
    {
        yield return _recoveryTime;
        var busy = GetBusyGageIndex();
        if (busy != -1)
        {
            bulletGages[busy].GageUp();
        }
    }

    //一番下の非アクティブなゲージを返す
    private int GetBusyGageIndex()
    {
        for (int i = 0; i < bulletGages.Length; i++)
        {
            if (!bulletGages[i].GetGageIsActive())
            {
                return i;
            }
        }

        return -1;
    }
    
    //一番上のアクティブなゲージを返す
    private int GetFreeGageIndex()
    {
        for (var i = bulletGages.Length-1; i >= 0; i--)
        {
            if (bulletGages[i].GetGageIsActive())
            {
                return i;
            }
        }

        return -1;
    }
    
}
