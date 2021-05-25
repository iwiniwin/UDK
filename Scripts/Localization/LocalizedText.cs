using UnityEngine;

namespace UDK.Localization
{
    public abstract class LocalizedText : MonoBehaviour
    {
        [SerializeField]
        private string m_Key;
        [SerializeField]
        private string m_AssetName;

        private bool enableTranslation;

        public abstract string Text
        {
            get;
            set;
        }

        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        public string AssetName
        {
            get { return m_AssetName; }
            set { m_AssetName = value; }
        }

        private void Awake()
        {
            if(string.IsNullOrEmpty(Key))
                return;
            if(string.IsNullOrEmpty(AssetName))
                this.Text = Key;
            if(!LocalizationManager.Instance.IsLoadedAsset(AssetName))
            {
                LocalizationManager.Instance.LoadLanguageAsset(AssetName);
            }
            this.Text = LocalizationManager.Instance.GetLocalizationValue(Key);
        }

        public void SetText(string key)
        {
            string str = LocalizationManager.Instance.GetLocalizationValue(key);
            this.Text = str;
        }
    }
}