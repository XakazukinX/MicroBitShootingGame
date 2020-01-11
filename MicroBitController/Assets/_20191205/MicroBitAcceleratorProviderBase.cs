using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MicroBitAcceleratorProviderBase : MonoBehaviour
{

    private static readonly Regex AcceleratorXMatchPattern = new Regex("(AccelX)(?=:)", RegexOptions.Compiled);
    private static readonly Regex AcceleratorYMatchPattern = new Regex("(AccelY)(?=:)", RegexOptions.Compiled);
    private static readonly Regex AcceleratorZMatchPattern = new Regex("(AccelZ)(?=:)", RegexOptions.Compiled);
    private static readonly Regex MessagePattern = new Regex("(?<=:)(.*)", RegexOptions.Compiled);
    

    enum AcceleratorType
    {
        XAcc,
        YAcc,
        ZAcc,
        None
    }
    
    private void OnEnable()
    {
        SerialPortReadLineReader.OnGetReadLine += OnGetReadLine;
    }

    private void OnGetReadLine(string readData)
    {
        if (CommandChecker(readData, out var _accType))
        {
            var value = 0;
            if (_accType == AcceleratorType.XAcc)
            {
                if (AccStringToInt(readData, out value))
                {
                    OnAccXGet(value);
                }
            }
            else if (_accType == AcceleratorType.YAcc)
            {
                if (AccStringToInt(readData, out value))
                {
                    OnAccYGet(value);
                }
            }
            else if (_accType == AcceleratorType.ZAcc)
            {
                if (AccStringToInt(readData, out value))
                {
                    OnAccZGet(value);
                }
            }
            else if (_accType == AcceleratorType.None)
            {
                //none
            }
        }
    }

    public virtual void OnAccXGet(int accValue){}
    public virtual void OnAccYGet(int accValue){}
    public virtual void OnAccZGet(int accValue){}


    private bool CommandChecker(string serialMessage, out AcceleratorType accType)
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

    private bool AccStringToInt(string accString, out int ret)
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