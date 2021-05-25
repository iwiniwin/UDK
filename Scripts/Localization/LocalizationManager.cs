using UnityEngine;
using UDK;
using System.Collections.Generic;
using System.Globalization;

namespace UDK.Localization
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private static readonly CultureInfo DEFAULT_CULTUREINFO = new CultureInfo("zh-CN");
        private Dictionary<uint, string> localizationData = new Dictionary<uint, string>();
        private Dictionary<string, bool> loadedAssets = new Dictionary<string, bool>();
        private CultureInfo m_CultureInfo;

        public CultureInfo CultureInfo
        {
            get
            {
                return m_CultureInfo;
            }
        }

        public LocalizationManager()
        {
            m_CultureInfo = DEFAULT_CULTUREINFO;
        }

        public string GetLocalizationValue(string key)
        {
            if(!uint.TryParse(key, out uint k))
            {
                return key;
            }
            if(localizationData.ContainsKey(k))
            {
                return localizationData[k];
            }
            return key;
        }

        public void LoadLanguageAsset(string assetName, IDataProvider provider = null)
        {   
            if(provider == null)
            {
                 provider = new DefaultDataProvider();
            }
            provider.Load(this.m_CultureInfo, assetName, OnLanguageAssetComplete);
            loadedAssets[assetName] = true;
        }

        public void OnLanguageAssetComplete(Dictionary<uint, string> data)
        {
            if(data == null || data.Count == 0)
                return;
            foreach(var kv in data)
            {
                if(!localizationData.ContainsKey(kv.Key))
                {
                    localizationData[kv.Key] = kv.Value;
                }
            }
        }

        public bool IsLoadedAsset(string assetName)
        {
            return loadedAssets.ContainsKey(assetName);
        }
    }
}