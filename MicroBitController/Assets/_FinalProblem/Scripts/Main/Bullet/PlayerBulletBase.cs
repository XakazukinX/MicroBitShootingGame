using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBase : MonoBehaviour
{
    private Animator bulletAnimator;
    public int bulletAuthorPlayerId = 0;
    [SerializeField] private float bulletSpeed = 1.0f;
    [SerializeField] private float disableTime = 3.0f;
    
    private Vector3 bulletDirection;
    private WaitForSeconds _disableWait;

    private bool isActive;
    
    public virtual void OnEnable()
    {
        bulletAnimator = GetComponent<Animator>();
        
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
    
    public virtual void HitBullet()
    {
//        Debug.Log("hit");
        bulletAnimator.Play("Hit",0,0);
        StartCoroutine(AnimationWait());
    }

    private IEnumerator AnimationWait()
    {
        while (bulletAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime<1.0f)
        {
           // Debug.Log("Hit Animation Playing");
            yield return null;
        }

        bulletAnimator.Play("default", 0, 0);
        //Debug.Log("Return Default");
        this.gameObject.SetActive(false);
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
