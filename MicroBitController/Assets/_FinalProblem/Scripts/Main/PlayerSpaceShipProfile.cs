using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace _PCFinal
{ 
    [CreateAssetMenu]
    public class PlayerSpaceShipProfile : ScriptableObject
    {
        public string spaceShipName;
        public string spaceShipExplanation = "サンプルの自機だよ";
        [Range(0,5)]
        public int moveSpeedLevel = 0;
        [Range(0,5)]
        public int rotateSpeedLevel = 0;
        
        public string bulletName;
        [TextArea(0,3)]
        public string bulletExplanation;
        public GameObject spaceShipObject;

        private StringBuilder moveSpeedbuilder = new StringBuilder("☆☆☆☆☆");
        private StringBuilder rotSpeedbuilder = new StringBuilder("☆☆☆☆☆");
        
        public string GetSpaceShipExplanation()
        {
            moveSpeedbuilder.Remove(0,moveSpeedLevel);
            for (int i = 0; i < moveSpeedLevel; i++)
            {
                moveSpeedbuilder.Insert(0, "★");
            }
            
            rotSpeedbuilder.Remove(0,rotateSpeedLevel);
            for (int i = 0; i < rotateSpeedLevel; i++)
            {
                rotSpeedbuilder.Insert(0, "★");
            }

            var result =
                $"{spaceShipExplanation}\n\n移動速度・・・{moveSpeedbuilder.ToString()}\n回転速度・・・{rotSpeedbuilder.ToString()}";
            //初期化
            moveSpeedbuilder = new StringBuilder("☆☆☆☆☆");
            rotSpeedbuilder = new StringBuilder("☆☆☆☆☆");
            return result;
        }

/*        [ContextMenu("TestGetString")]
        public void test()
        {
            Debug.Log(GetSpaceShipExplanation());
        }*/
    }
}