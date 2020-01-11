using System;
 using System.Collections;
 using System.Collections.Generic;
 using System.Text.RegularExpressions;
 using UnityEngine;
 
 namespace _20191205
 {
     public class MicroBitSpherePusher : MonoBehaviour
     {
         private static readonly Regex AcceleratorXMatchPattern = new Regex("(AccelX)(?=:)", RegexOptions.Compiled);
         private static readonly Regex AcceleratorYMatchPattern = new Regex("(AccelY)(?=:)", RegexOptions.Compiled);
         private static readonly Regex AcceleratorZMatchPattern = new Regex("(AccelZ)(?=:)", RegexOptions.Compiled);
         private static readonly Regex MessagePattern = new Regex("(?<=:)(.*)", RegexOptions.Compiled);
 
         [SerializeField] private int threshold = 900;
         
         private Rigidbody targetRigidBody = new Rigidbody();
         private Vector3 _forceVector = new Vector3(0, 0, 0);
         
         
         enum AcceleratorType
         {
             XAcc,
             YAcc,
             ZAcc,
             None
         }
 
         private void Start()
         {
             targetRigidBody = this.gameObject.GetComponent<Rigidbody>();
             if (targetRigidBody == null)
             {
                 Debug.LogError("RigidBodyがアタッチされていません");
                 enabled = !enabled;
             }
         }
 
         private void OnEnable()
         {
             SerialPortReadLineReader.OnGetReadLine += OnGetReadLine;
         }
         
         private void OnGetReadLine(string readData)
         {
             if (CommandChecker(readData, out var _accType))
             {
                 if (_accType == AcceleratorType.XAcc)
                 {
                     if (AccStringToInt(readData, out var value))
                     {
 /*                        Debug.Log(value);*/
                         var absValue = Mathf.Abs(value);
                         if (absValue > threshold)
                         {
                             Debug.Log("Shake!");
                             _forceVector.z = absValue / 2000.0f * 100.0f;
                             Debug.Log(absValue);
                             targetRigidBody.AddForce(_forceVector);
                         }
                     }
                 }
             }
         }
         
         private bool CommandChecker(string serialMessage,out AcceleratorType accType)
         {
             var commandName = AcceleratorXMatchPattern.Match(serialMessage);
             if (commandName.Success)
             {
                 accType = AcceleratorType.XAcc;
                 return true;
             }
             
             commandName = AcceleratorYMatchPattern.Match(serialMessage);
             if (commandName.Success)
             {
                 accType = AcceleratorType.YAcc;
                 return true;
             }
             
             commandName = AcceleratorZMatchPattern.Match(serialMessage);
             if (commandName.Success)
             {
                 accType = AcceleratorType.ZAcc;
                 return true;
             }
 
             accType = AcceleratorType.None;
             return false;
         }
         private bool AccStringToInt(string accString ,out int ret)
         {
             var valueString = MessagePattern.Match(accString);
 //            Debug.Log(valueString);
             if (int.TryParse(valueString.Value, out ret))
             {
                 return true;
             }
             else
             {
                 Debug.LogError("不正な値です");
                 return false;
             }
         }
         
 
     }
 }