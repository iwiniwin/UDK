using UnityEngine;
using System.IO;
using System.Collections.Generic;
using static UKit.Utils.Output;


public class Assets
{
    static Dictionary<string, string> m_ResABDic = new Dictionary<string, string>();
    static Dictionary<string, AssetBundle> m_BundleCache = new Dictionary<string, AssetBundle>();

    static Assets(){
        // 读取依赖关系
        BundleList list = Resources.Load<BundleList>("bundleList");
        foreach (var bundleData in list.bundleDatas)
        {
            m_ResABDic[bundleData.resPath] = bundleData.bundlePath;
        }
    }

    /// <summary>
    /// 统一的资源加载方式
    /// AssetBundle 与 Resources 资源加载的无缝切换
    /// 资源加载策略
    /// 1. 从 Application.persistentDataPath 目录中查找读写目录下是否有要加载的 AssetBundle
    /// 2. 从 Application.streamingAssetsPath 目录中查找是否有需要加载的 AssetBundle
    /// 2. 在 Resources 目录中加载文件
    /// </summary>
    public static T Load<T>(string path) where T : Object{
        // 从 AssetBundle中加载资源，最好提供后缀名，不然无法区分同名文件
        string resPath = Path.Combine("Assets/Resources", path);
        if(typeof(T) == typeof(GameObject)){
            resPath = Path.ChangeExtension(resPath, "prefab");
        }
        // 如果 Bundle 有这个资源，则从 Bundle 中加载
        string bundlePath;
        if(m_ResABDic.TryGetValue(resPath, out bundlePath)){
            AssetBundle assetBundle;
            if(!m_BundleCache.TryGetValue(bundlePath, out assetBundle)){
                // 读取 Bundle
                assetBundle = m_BundleCache[bundlePath] = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundlePath));
            }
            // 从 Bundle 中读取资源
            return assetBundle.LoadAsset<T>(resPath);
        }
        // 如果 Bundle 中没有这个资源，则从 Resources 目录中加载
        return Resources.Load<T>(path);
    }
}
