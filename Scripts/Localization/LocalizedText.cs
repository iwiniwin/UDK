using UnityEngine;

namespace UDK.Localization
{
    public abstract class LocalizedText : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        public string Key;
        [ReadOnly]
        [SerializeField]
        public string AssetName;

        private bool enableTranslation;

        public abstract string Text
        {
            get;
            set;
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