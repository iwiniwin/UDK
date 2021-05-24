using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDK.Localization
{
    [Serializable]
    public class LanguageSource
    {
        [SerializeField]
        private List<LanguageEntry> entries = new List<LanguageEntry>();

        private Dictionary<uint, bool> keys = new Dictionary<uint, bool>();

        public void AddEntry(LanguageEntry entry)
        {
            if (!keys.ContainsKey(entry.Key))
            {
                keys.Add(entry.Key, true);
            }
            entries.Add(entry);
        }

        public int EntryCount
        {
            get { return entries.Count; }
            private set { }
        }

        public bool ContainEntry(uint key)
        {
            return keys.ContainsKey(key);
        }
    }

    [Serializable]
    public class LanguageEntry
    {
        [SerializeField]
        private uint key;
        [SerializeField]
        private string value;

        public LanguageEntry(uint key, string value)
        {
            this.key = key;
            this.Value = value;
        }

        public uint Key
        {
            get { return key; }
            set { key = value; }
        }

        public string Value
        {
            get { return StringUtil.FromBase64(value); }
            set { this.value = StringUtil.ToBase64(value); }
        }
    }
}