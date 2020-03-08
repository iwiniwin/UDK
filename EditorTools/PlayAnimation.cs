using UnityEngine;
using static UKit.Utils.Output;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;
#endif

[RequireComponent(typeof(Animator))]
public class PlayAnimation : MonoBehaviour
{
    
}
#if UNITY_EDITOR
/// <summary>
/// 用于在编辑器模式下预览对象动画
/// </summary>
[CustomEditor(typeof(PlayAnimation))]
public class ScriptEditor : Editor{
    private AnimationClip[] m_Clips = null;
    private PlayAnimation m_Script = null;
    private void OnEnable() {
        m_Script = (target as PlayAnimation);
        Animator animator = m_Script.gameObject.GetComponent<Animator>();
        AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;
        m_Clips = controller.animationClips;
    }

    private int m_SelcetIndex = 0;
    private float m_SliderValue = 0;
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        m_SelcetIndex = EditorGUILayout.Popup("动画", m_SelcetIndex, m_Clips.Select(pkg => pkg.name).ToArray());
        m_SliderValue = EditorGUILayout.Slider("进度", m_SliderValue, 0f, 1f);
        if(EditorGUI.EndChangeCheck()){
            AnimationClip clip = m_Clips[m_SelcetIndex];
            float time = clip.length * m_SliderValue;
            // 用于采样动画
            clip.SampleAnimation(m_Script.gameObject, time);
        }
    }
}
#endif