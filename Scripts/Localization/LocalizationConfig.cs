using UnityEngine;
using UDK;
using System.Collections.Generic;
using System.Globalization;

namespace UDK.Localization
{
    public abstract class LocalizationConfig
    {
        public virtual string RootPath
        {
            get; set;
        }
        
        public virtual string GetPath(string assetName)
        {
            string name = LocalizationManager.Instance.CultureInfo.Name;
            string path = System.IO.Path.Combine(RootPath, name + ".asset");
            path = path.Replace(@"\", "/");
            return path;
        }
    }
}