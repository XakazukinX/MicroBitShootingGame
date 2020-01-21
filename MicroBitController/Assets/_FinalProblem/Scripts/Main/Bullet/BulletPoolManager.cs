using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    [SerializeField] private PlayerBulletBase BulletPrefabs;
    private PlayerBulletBase[] poolBullets = new PlayerBulletBase[0];

    private void OnEnable()
    {

//        Debug.Log($"There are {poolBullets.Length} player bullet in the pool");
    }

    public void InitBulletPool(int bulletCount)
    {
        poolBullets = new PlayerBulletBase[bulletCount];
        for (int i = 0; i < bulletCount; i++)
        { 
            var tmp = Instantiate(BulletPrefabs.gameObject, this.transform, true);
            //念のため非表示
            tmp.SetActive(false);
        }
        
        poolBullets = GetComponentsInChildren<PlayerBulletBase>(true);        
    }


    public void SetBulletAuthorId(int authorConnId)
    {
        foreach (var b in poolBullets)
        {
            b.bulletAuthorPlayerId = authorConnId;
        }
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
