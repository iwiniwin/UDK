using UnityEngine;
using UnityEngine.UI;

namespace UDK.Localization
{
    [RequireComponent(typeof(Text))]
    [DisallowMultipleComponent]
    public class LocalizedUGUIText : LocalizedText
    {
        private Text m_Target;
        public Text Target
        {
            get
            {
                if (m_Target == null)
                {
                    m_Target = GetComponent<Text>();
                }
                return m_Target;
            }
        }

        public override string Text
        {
            get
            {
                return Target.text;
            }
            set
            {
                Target.text = value;
            }
        }
    }


}