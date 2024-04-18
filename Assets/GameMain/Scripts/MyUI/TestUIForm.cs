using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain.Scripts.MyUI
{
    public class TestUIForm:UIFormExtend
    {
        public List<string> bundingData = new List<string>()
        {
            "1",
            "2",
            "3",
            "4"
        };
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            for (int i = 0; i < 4; i++)
            {
                ShowUIItem(
                    "Assets/GameMain/Scripts/MyUI/Text (TMP) (1).prefab",
                    transform.GetChild(0).GetComponent<RectTransform>(),
                    userData);  
            }
            BindList<string>("Text (TMP) (1)",bundingData);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Test();
            }
         
        }

        public void Test()
        {
            UpdateAllItems();
        }
    }
}