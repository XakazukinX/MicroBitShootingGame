using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGageController : MonoBehaviour
{
    [SerializeField] private GameObject Gage;

    public bool GetGageIsActive()
    {
        return Gage.activeSelf;
    }
    public void GageDown()
    {
        Gage.SetActive(false);
    }

    public void GageUp()
    {
        Gage.SetActive(true);
    }
}
