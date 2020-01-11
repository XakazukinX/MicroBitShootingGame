using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class MicroBitCommands
{
    // : 以前にマッチ。Accel,Commandを見分ける
    private static readonly Regex ButtonMatchPattern = new Regex("(Button)(?=:)", RegexOptions.Compiled);
    private static readonly Regex GestureMatchPattern = new Regex("(Gesture)(?=:)", RegexOptions.Compiled);

    private static readonly Regex AcceleratorMatchPattern = new Regex("(Accelerator)(?=:)", RegexOptions.Compiled);

    private static readonly Regex MessagePattern = new Regex("(?<=:)(.*)", RegexOptions.Compiled);
    
    private static Vector3 _accel = Vector3.zero;
    public enum ButtonCommand
    {
        //Buttons
        PushA,
        PushB,
        //
        None
    }

    public enum GestureCommand
    {
        //Gestures
        up,
        down, 
        left,
        right,
        faceUp,
        faceDown, 
        freeFall, 
        threeG,
        sixG,
        eightG, 
        shake,
        //
        None
    }

    public static ButtonCommand GetButton(string serialMessage)
    {
        var commandName = ButtonMatchPattern.Match(serialMessage);
        
        if (!commandName.Success)
        {
            return ButtonCommand.None;
        }
        
        
        var messageName = MessagePattern.Match(serialMessage);
//        Debug.Log(messageName);
        if (messageName.Success)
        {
            var ret = (ButtonCommand) Enum.Parse(typeof(ButtonCommand), messageName.Value, true);
            return ret;
        }

        Debug.LogError("設定されていないコマンドの入力を受けました");
        return ButtonCommand.None;
    }
    
    
    public static GestureCommand GetGesture(string serialMessage)
    {
        var commandName = GestureMatchPattern.Match(serialMessage);
        
        if (!commandName.Success)
        {
            return GestureCommand.None;
        }
        
        
        var messageName = MessagePattern.Match(serialMessage);
      //  Debug.Log(messageName);
        
        if (messageName.Success)
        {
            var ret = (GestureCommand) Enum.Parse(typeof(GestureCommand), messageName.Value, true);
            return ret;
        }

        Debug.LogError("設定されていないコマンドの入力を受けました");
        return GestureCommand.None;
    }


    public static Vector3 GetAcceleratorValue(string serialMessage)
    {
        var commandName = AcceleratorMatchPattern.Match(serialMessage);
        
        if (commandName.Success)
        {
            var messageName = MessagePattern.Match(serialMessage);
            Debug.Log("ACC__" + messageName);
        }

        return Vector3.zero;
    }
    
    
    
    
/*    public static Vector3 GetAccelerometerValue(string serialMessage)
    {
        var commandName = MessagePattern.Match(serialMessage);
        Debug.Log(commandName);
        if (commandName.Success)
        {
            var ret = (ButtonCommand) Enum.Parse(typeof(ButtonCommand), commandName.Value, true);
            return ret;
        }

        Debug.LogError("設定されていないコマンドの入力を受けました");
        return ButtonCommand.None;
    }*/
    
}
