using System.Collections;
using System.Collections.Generic;
using GameMain.Scripts.MyUI;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

public class UIFormExtend : UIFormLogic
{
    public RectTransform CacheRectTransform => GetComponent<RectTransform>();
    public Dictionary<string,List<UIItemStatic>> UIItemStaticDic = new Dictionary<string, List<UIItemStatic>>();
    public Dictionary<string,List<UIItemDynamic>> UIItemDynamicDic = new Dictionary<string, List<UIItemDynamic>>();
    // 用于保存每个组绑定的数据列表引用
    private Dictionary<string, IList> boundDataLists = new Dictionary<string, IList>();

    public CanvasGroup CanvasGroup
    {
        get
        {
          return gameObject.GetOrAddComponent<CanvasGroup>();
        }
    }

   
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        //获取所有静态的UIItem,根据类型分组
        UIItemStatic[] uiItemStatics = GetComponentsInChildren<UIItemStatic>();
        foreach (var uiItemStatic in uiItemStatics)
        {
            if (!UIItemStaticDic.ContainsKey(uiItemStatic.name))
            {
                UIItemStaticDic.Add(uiItemStatic.name,new List<UIItemStatic>());
            }
            UIItemStaticDic[uiItemStatic.name].Add(uiItemStatic);
            //初始化UIItem
            uiItemStatic.OnInit(userData);
        }
    }

    // 绑定列表的方法
    public void BindList<T>(string groupName, List<T> dataList)
    {
        // 检查是否为静态UI项组
        if (UIItemStaticDic.ContainsKey(groupName))
        {
            // 保存数据列表引用
            if (!boundDataLists.ContainsKey(groupName))
            {
                boundDataLists.Add(groupName, dataList);
            }
            else
            {
                boundDataLists[groupName] = dataList;
            }
            // 这里可以添加更多的逻辑来初始化或更新UI项
            // 例如，根据dataList更新UIItemStaticDic中对应组的UI项
        }
        // 检查是否为动态UI项组
        else if (UIItemDynamicDic.ContainsKey(groupName))
        {
            // 保存数据列表引用
            if (!boundDataLists.ContainsKey(groupName))
            {
                boundDataLists.Add(groupName, dataList);
            }
            else
            {
                boundDataLists[groupName] = dataList;
            }
            // 这里可以添加更多的逻辑来初始化或更新UI项
            // 例如，根据dataList更新UIItemDynamicDic中对应组的UI项
        }
        else
        {
            Log.Warning($"GroupName {groupName} not found in UIItemStaticDic or UIItemDynamicDic.");
        }
    }

    // 更新所有项的方法
    public void UpdateAllItems()
    {
        foreach (var groupName in boundDataLists.Keys)
        {
            IList dataList = boundDataLists[groupName];
            // 这里根据groupName和dataList更新UI项
            // 你需要根据实际的UIItemStatic和UIItemDynamic的实现来编写更新逻辑
            // 检查是否为静态UI项组
            if (UIItemStaticDic.ContainsKey(groupName))
            {
                List<UIItemStatic> uiItemStatics = UIItemStaticDic[groupName];
                for (int i = 0; i < uiItemStatics.Count; i++)
                {
                    uiItemStatics[i].UpdateItem(dataList[i]);
                }
            }
            // 检查是否为动态UI项组
            else if (UIItemDynamicDic.ContainsKey(groupName))
            {
                List<UIItemDynamic> uiItemDynamics = UIItemDynamicDic[groupName];
                for (int i = 0; i < uiItemDynamics.Count; i++)
                {
                    uiItemDynamics[i].UpdateItem(dataList[i]);
                }
            }
        }
    }
  
    public void ShowUIItem(string path,RectTransform parent,object data)
    {
        //加载UIItem,通过AssetDataBase加载
        UIItemDynamic uiItemDynamic = null;
        var assetAtPath = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        //判断是否已经存在该组,如果不存在则创建
        if (!UIItemDynamicDic.ContainsKey(assetAtPath.name))
        {
            UIItemDynamicDic.Add(assetAtPath.name,new List<UIItemDynamic>());
        }
        var item = Instantiate(assetAtPath).GetComponent<UIItemDynamic>();
        //调用UIItem的OnInit方法
        item.OnInit(data);
        //调用UIItem的OnShow方法
        item.OnShow(data);
        //将UIItem添加到对应的组中
        UIItemDynamicDic[assetAtPath.name].Add(item);
        //设置父物体
        item.transform.SetParent(parent);
    }
  
    protected override void OnRecycle()
    {
        base.OnRecycle();
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        //调用动态UIItem的OnClose方法
        foreach (var uiItemDynamic in UIItemDynamicDic.Values)
        {
            foreach (var uiItem in uiItemDynamic)
            {
                uiItem.OnRelease();
            }
        }
        //调用静态UIItem的OnClose方法
        foreach (var uiItemStatic in UIItemStaticDic.Values)
        {
            foreach (var uiItem in uiItemStatic)
            {
                uiItem.OnClose();
            }
        }
    }
}
