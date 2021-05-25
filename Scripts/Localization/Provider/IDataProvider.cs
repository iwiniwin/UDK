using System;
using System.Collections.Generic;
using System.Globalization;

namespace UDK.Localization
{
    public interface IDataProvider 
    {
        void Load(CultureInfo cultureInfo, string assetName, Action<Dictionary<uint, string>> onLoadComplete);
    }
}