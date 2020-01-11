using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    private PlayerBulletBase[] poolBullets = new PlayerBulletBase[0];

    private void OnEnable()
    {
        poolBullets = GetComponentsInChildren<PlayerBulletBase>(true);
//        Debug.Log($"There are {poolBullets.Length} player bullet in the pool");
    }

    public bool GetFreeBullet(out PlayerBulletBase bullet)
    {
        //Debug.Log("TryGet");
        foreach (var b in poolBullets)
        {
            if (b.gameObject.activeSelf) continue;
            bullet = b;
            return true;
        }
        
        bullet = null;
        return false;
    }
    
}
