using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

namespace UDK.Localization.Editor
{
    public class LanguageSourceAssetBuilder
    {
        private static string rootPath = "Assets/LocalizationTest/{0}.asset";
        [MenuItem("Assets/Test Spawn", false, 1)]
        public static void GenerateUILanguage()
        {
            DirectoryInfo directory = new DirectoryInfo("Assets/UI/Layouts");
            DirectoryInfo[] directories = directory.GetDirectories();
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            foreach (DirectoryInfo item in directories)
            {
                List<string> names = new List<string>();
                LanguageSource source = new LanguageSource();
                Dictionary<uint, bool> content = new Dictionary<uint, bool>();
                FileInfo[] files = item.GetFiles("*.prefab", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    string path = file.FullName.Substring(file.FullName.IndexOf("Assets"));
                    var obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    CheckLocalizedTextInPrefab(obj, item.Name, source);
                }
                if (source.EntryCount > 0)
                {
                    CreateLanguageAsset(source, item.Name);
                    AssetDatabase.Refresh();
                }
            }
        }

        private static void CheckLocalizedTextInPrefab(GameObject obj, string assetName, LanguageSource source)
        {
            var components = obj.GetComponentsInChildren<LocalizedText>(true);
            bool modifyTag = false;
            foreach (var component in components)
            {
                string text = GetComponentText(component.gameObject);
                if (text.Length > 0)
                {
                    var tComponent = component.gameObject.GetComponent<Text>();
                    if (!StringUtil.ContainChinese(text))  // 不包含汉字
                    {
                        ModifyLocalizedText(component, "", "");
                        modifyTag = true;
                    }
                    else
                    {
                        uint key = StringUtil.ToUInt32(assetName + text);
                        if (!key.Equals(component.Key) || !assetName.Equals(component.AssetName))
                        {
                            ModifyLocalizedText(component, key.ToString(), assetName);
                            modifyTag = true;
                        }
                        if (!source.ContainEntry(key))
                        {
                            source.AddEntry(new LanguageEntry(key, text));
                        }
                    }

                    if (modifyTag)
                    {
                        PrefabUtility.SavePrefabAsset(obj);
                    }
                }
            }
        }

        private static string GetComponentText(GameObject obj)
        {
            var component = obj.GetComponent<Text>();
            if (component != null)
            {
                return component.text;
            }
            return "";
        }

        private static void CreateLanguageAsset(LanguageSource source, string assetName)
        {
            assetName = assetName.ToLower();
            var sourceAsset = ScriptableObject.CreateInstance<LanguageSourceAsset>();
            sourceAsset.Source = source;
            AssetDatabase.CreateAsset(sourceAsset, string.Format(rootPath, assetName));
        }

        private static string GetPropertyValue(string name, object obj)
        {
            var value = obj.GetType().GetProperty(name).GetValue(obj, null);
            return value?.ToString();
        }

        private static void ModifyLocalizedText(LocalizedText text, string key, string assetName)
        {
            SerializedObject so = new SerializedObject(text);
            so.FindProperty("m_Key").stringValue = key;
            so.FindProperty("m_AssetName").stringValue = assetName;
            so.ApplyModifiedProperties();
        }
    }
}
