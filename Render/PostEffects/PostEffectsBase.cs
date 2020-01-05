using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{

    void Start()
    {
        CheckResources();
    }

    protected void CheckResources() {
        bool isSupported = CheckSupport();
        if(isSupported == false) {
            NotSupported();
        }
    }

    // 判断平台是否支持屏幕特效
    protected bool CheckSupport() {
        // render textures 已经默认支持
        if(SystemInfo.supportsImageEffects == false) {
            Debug.LogWarning("This platform does not support image effects");
            return false;
        }
        return true;
    }

    protected void NotSupported() {
        enabled = false;
    }

    // 创建使用指定Shader的材质
    protected Material CreateMaterial(Shader shader, Material material) {
        if(shader == null || !shader.isSupported){
            return null;
        }

        if(material && material.shader == shader) {
            return material;
        }

        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        if(material){
            return material;
        }
        return null;
    }
}
