using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Scripts.MyUI
{
    public class UIItemLogic : MonoBehaviour
    {
        public int SerialID;
        public object Owner;

        // 定义一个字典来存储UI元素的显隐状态
        private Dictionary<GameObject, bool> uiVisibility = new Dictionary<GameObject, bool>();

        // 定义一个结构体来存储UI元素的位置和缩放等属性
        private struct UIItemState
        {
            public Vector3 Position;
            public Vector3 Scale;
            public Vector2 AnchorPosition;
            public Vector3 LocalPosition;
        }

        // 用于保存当前UI元素的状态
        private UIItemState originalState;

        public virtual void OnInit(object data)
        {
            // 记录UI打开状态
            SaveUIVisibility();
            // 记录UI元素的初始状态
            SaveUIInitialState();
        }

        public virtual void UpdateItem(object data)
        {
        }

        protected void SaveUIVisibility()
        {
            foreach (Transform child in transform)
            {
                uiVisibility[child.gameObject] = child.gameObject.activeSelf;
            }
        }

        protected void ResetUIVisibility()
        {
            foreach (var uiElement in uiVisibility)
            {
                uiElement.Key.SetActive(uiElement.Value);
            }
        }

        /// <summary>
        /// 记录UI元素的初始状态
        /// </summary>
        protected void SaveUIInitialState()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            originalState = new UIItemState
            {
                Position = transform.position,
                Scale = transform.localScale,
                AnchorPosition = rectTransform.anchoredPosition,
                LocalPosition = transform.localPosition
            };
        }

        /// <summary>
        /// 重置UI元素到它的初始状态
        /// </summary>
        protected void ResetUIInitialState()
        {
            transform.position = originalState.Position;
            transform.localScale = originalState.Scale;
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = originalState.AnchorPosition;
            transform.localPosition = originalState.LocalPosition;
        }
    }
}