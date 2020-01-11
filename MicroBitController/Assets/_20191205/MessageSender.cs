using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _20191205
{
    public class MessageSender : MonoBehaviour
    {
        [SerializeField] private float height = 1.0f;
        [SerializeField] private GameObject damageObject;
        
        void Update () 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();


            if (Input.GetMouseButtonDown (0)) 
            {
                if (Physics.Raycast(ray, out hit))
                {
                    var pos = hit.point;
                    pos.y = height;
                    Debug.Log(pos);
                    var message = new DamageObjectMessage
                    {
                        damageObjectFallingPos = pos,
                    };
                    
                    var tmpObj = GameObject.Instantiate(damageObject, pos, Quaternion.identity);
                    NetworkClient.Send(message);
                }

            }

        }
    }
}