using UnityEngine;

namespace UDK.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        public string text;
        [SerializeField]
        private string m_Key;
        [SerializeField]
        private string m_AssetName;

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
    }
}