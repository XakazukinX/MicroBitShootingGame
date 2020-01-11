using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MicroBitPlayerController : MicroBitControllerBase
{ 
    public GameObject player;
    public GameObject bullet;
    public float weight;
    public override void OnDataReceived(string msg)
    {
        var gesture = MicroBitCommands.GetGesture(msg);
     //   Debug.Log(gesture);
        if (gesture == MicroBitCommands.GestureCommand.right)
        {
            player.transform.position += new Vector3(weight, 0, 0);
        }
        if (gesture == MicroBitCommands.GestureCommand.left)
        {
            player.transform.position -= new Vector3(weight, 0, 0);
        }
/*        if (gesture == MicroBitCommands.GestureCommand.up || gesture == MicroBitCommands.GestureCommand.faceUp )
        {
            player.transform.position += new Vector3(0, weight, 0);;
        }
        if (gesture == MicroBitCommands.GestureCommand.down || gesture == MicroBitCommands.GestureCommand.faceDown )
        {
            player.transform.position -= new Vector3(0, weight, 0);;
        }*/

        var button = MicroBitCommands.GetButton(msg);
        if (button == MicroBitCommands.ButtonCommand.PushA || button == MicroBitCommands.ButtonCommand.PushB)
        {
            Instantiate(bullet, player.gameObject.transform);
        }

        MicroBitCommands.GetAcceleratorValue(msg);
    }
}
