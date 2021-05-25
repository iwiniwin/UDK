using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace UDK.Localization
{
    public class DefaultDataProvider : IDataProvider
    {
        public virtual void Load(CultureInfo cultureInfo, string assetName, Action<Dictionary<uint, string>> onLoadComplete)
        {
            assetName = assetName.ToLower();
            Dictionary<uint, string> dic = new Dictionary<uint, string>();
            try
            {
                var filePath = GetPath(cultureInfo, assetName);
                var asset = Resources.Load<LanguageSourceAsset>(filePath);
                if(asset == null || asset.Source == null)
                {
                    Debug.LogError("load asset error : " + filePath);
                }
                dic = asset.Source.GetData();
            }
            catch(Exception e)
            {
                DebugEx.LogWarning(e.ToString());
            }
            finally
            {
                onLoadComplete(dic);
            }

        }

        public virtual string GetPath(CultureInfo cultureInfo, string assetName)
        {
            string name = cultureInfo.Name;
            string path = System.IO.Path.Combine("LocalizationTest", name, assetName);
            path = path.Replace(@"\", "/");
            return path;
        }
    }
}