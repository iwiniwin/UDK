using UnityEngine;
using System.Collections.Generic;
using static UKit.Utils.Output;

[System.Serializable]
public class BundleList : ScriptableObject
{
    public List<BundleData> bundleDatas = new List<BundleData>();

    // 保存每个 res 路径对应的 Bundle 路径
    [System.Serializable]
    public class BundleData{
        public string resPath = string.Empty;
        public string bundlePath = string.Empty;
    }
}
