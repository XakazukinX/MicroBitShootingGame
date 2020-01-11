using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBase : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 1.0f;
    [SerializeField] private float disableTime = 3.0f;

    private Vector3 bulletDirection;
    private WaitForSeconds _disableWait;

    private bool isActive;
    
    public virtual void OnEnable()
    {
        _disableWait = new WaitForSeconds(disableTime);
        isActive = true;
        StartCoroutine(DisableCountDownCoroutine());
    }

    public virtual void OnDisable()
    {
        isActive = false;
    }
    
    private void Update()
    {
        UpdateBulletPosition();
    }


    public virtual void SpawnBullet(Vector3 spawnPos,Quaternion spawnRot)
    {
        var bulletTransform = this.transform;
        
        bulletTransform.position = spawnPos;
        bulletTransform.rotation = spawnRot;
/*        Debug.Log(spawnPos);
        Debug.Log(transform.position);*/
        
        gameObject.SetActive(true);
    }
    
    public virtual void UpdateBulletPosition()
    {
        if (!isActive)
        {
            return;
        }
        
        var moveDirection = GetBulletDirection(bulletSpeed);
        transform.localPosition += moveDirection;
    }
    
    private Vector3 GetBulletDirection(float moveSpeed)
    {
        var angleDir = transform.localEulerAngles.z * (Mathf.PI / 180.0f);
            
        bulletDirection.x = -Mathf.Sin(angleDir);
        bulletDirection.y = Mathf.Cos(angleDir);
        bulletDirection.z = 0;

        return (moveSpeed * Time.deltaTime * bulletDirection);
    }

    private IEnumerator DisableCountDownCoroutine()
    {
        yield return _disableWait;
        gameObject.SetActive(false);
    }
    
    
    
}
