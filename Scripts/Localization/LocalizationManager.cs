using UnityEngine;
using UDK;
using System.Collections.Generic;
using System.Globalization;

namespace UDK.Localization
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private Dictionary<uint, string> localizationData = new Dictionary<uint, string>();
        private HashSet<string> loadedAssets = new HashSet<string>();
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
            m_CultureInfo = Locale.GetCultureInfo();
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

        public void ChangeLanguage(SystemLanguage language, bool autoRefresh = false)
        {
            var cultureInfo = Locale.GetCultureInfoByLanguage(language);
            if(cultureInfo == m_CultureInfo)
                return;
            m_CultureInfo = cultureInfo;
            ReloadAllAssets();
            if(autoRefresh)
                RefreshAllLocalizedText();
        }   

        public void ReloadAllAssets()
        {
            localizationData.Clear();
            foreach(var assetName in loadedAssets)
            {
                LoadLanguageAsset(assetName);
            }
        }

        public void LoadLanguageAsset(string assetName, IDataProvider provider = null)
        {   
            if(provider == null)
            {
                 provider = new DefaultDataProvider();
            }
            provider.Load(this.m_CultureInfo, assetName, OnLanguageAssetComplete);
            loadedAssets.Add(assetName);
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
            return loadedAssets.Contains(assetName);
        }

        public void RefreshAllLocalizedText()
        {
            foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                var components = rootObj.GetComponentsInChildren<LocalizedText>();
                foreach(var component in components)
                {
                    component.SetText(component.Key);
                }
            }
        }
    }
}