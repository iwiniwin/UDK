using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBlur : PostEffectsBase
{
    public Shader gaussianBlurShader;
    private Material gaussianBlurMaterial = null;
    public Material material {
        get {
            gaussianBlurMaterial = CreateMaterial(gaussianBlurShader, gaussianBlurMaterial);
            return gaussianBlurMaterial;
        }
    }

    // 高斯模糊迭代次数
    [Range(0, 4)]
    public int iterations = 3;

    // 模糊范围 blurSpread越大，模糊程度越高，但采样数不会受到影响，但过大会造成虚影
    [Range(0.2f, 3.0f)]
    public float blurSpread = 0.6f;

    // 缩放系数 downSample越大，需要处理的像素数越少，同时能进一步提高模糊程度，但过大可能会使图像像素化
    [Range(1, 8)]
    public int downSample = 2;

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (material != null) {
            // 利用缩放对图像进行降采样，从而减少需要处理的像素个数，提高性能
            int rtW = src.width / downSample;
            int rtH = src.height / downSample;

            // 因为高斯模糊需要调用两个Pass，获取临时缓冲区存储第一个Pass执行完毕后的模糊结果
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear; // 滤波模式设置为双线性
            Graphics.Blit(src, buffer0);
            for(int i = 0; i < iterations; i ++){
                material.SetFloat("_BlurSize", 1.0f + i * blurSpread);
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

                // 使用竖直方向的一维高斯核进行滤波
                Graphics.Blit(buffer0, buffer1, material, 0);

                RenderTexture.ReleaseTemporary(buffer0);

                buffer0 = buffer1;
                buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

                // 使用水平方向的一维高斯核进行滤波
                Graphics.Blit(buffer0, buffer1, material, 1);

                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }

            Graphics.Blit(buffer0, dest);
            RenderTexture.ReleaseTemporary(buffer0);
        }else{
            Graphics.Blit(src, dest);
        }
    }
}
