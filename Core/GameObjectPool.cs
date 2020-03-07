using UnityEngine;
using System.Collections.Generic;
using static UKit.Utils.Output;

/// <summary>
/// 游戏对象缓存池
/// </summary>
public class GameObjectPool : Object
{
    class PoolObject : MonoBehaviour{
        public string value;
    }

    private GameObject m_ObjectContainer;
    private readonly Dictionary<string, Stack<GameObject>> m_AssetObjects;

    public GameObjectPool(string name){
        m_AssetObjects = new Dictionary<string, Stack<GameObject>>();
        m_ObjectContainer = new GameObject("#POOL#" + name);
        m_ObjectContainer.SetActive(false);
    }

    /// <summary>
    /// 获取游戏对象
    /// </summary>
    /// <param name="assetPath">资源路径</param>
    public GameObject GetObject(string assetPath){
        GameObject go = GetObjectFromPool(assetPath);
        if(go == null){
            go = Object.Instantiate<GameObject>(Resources.Load<GameObject>(assetPath));
            go.AddComponent<PoolObject>().value = assetPath;
        }
        return go;
    }

    /// <summary>
    /// 释放游戏对象
    /// </summary>
    public void ReleaseObject(GameObject go){
        var component = go.GetComponent<PoolObject>();
        if(component != null && component.value != null){
            Stack<GameObject> objects;
            if(m_AssetObjects.TryGetValue(component.value, out objects)){
                objects.Push(go);
                go.transform.SetParent(m_ObjectContainer.transform, false);
                return;
            }
        }
        GameObject.Destroy(go);
    }

    /// <summary>
    /// 清空对象池
    /// </summary>
    public void Clear(){
        foreach(KeyValuePair<string, Stack<GameObject>> pair in m_AssetObjects){
            while(pair.Value.Count > 0){
                GameObject.Destroy(pair.Value.Pop());
            }
        }
        m_AssetObjects.Clear();
    }

    /// <summary>
    /// 清空对象池的同时销毁 ObjectContainer
    /// </summary>
    public void Clean(){
        Clear();
        GameObject.Destroy(m_ObjectContainer);
    }

    private GameObject GetObjectFromPool(string assetPath){
        Stack<GameObject> objects;
        if(!m_AssetObjects.TryGetValue(assetPath, out objects)){
            objects = new Stack<GameObject>();
            m_AssetObjects[assetPath] = objects;
        }
        return objects.Count > 0 ? objects.Pop() : null;
    }
}
