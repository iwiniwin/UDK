Shader "Unlit/GaussianBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        ZTest Always
        ZWrite Off
        Cull Off
        
        CGINCLUDE
        #include "UnityCG.cginc"
        sampler2D _MainTex;
        half4 _MainTex_TexelSize;
        float _BlurSize;
        struct v2f {
            float4 pos : SV_POSITION;
            half2 uv[5] : TEXCOORD0;
        };
        v2f vertBlurVertical(appdata_img v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            // 通过把计算采样纹理坐标的代码从片元着色器转移到顶点着色器中，可以减少运算，提高性能
            // 由于顶点着色器到片元着色器的插值是线性的，所以这样不会影响坐标的计算结果
            half2 uv = v.vertex;
            o.uv[0] = uv;
            o.uv[1] = uv + float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
            o.uv[2] = uv - float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
            o.uv[3] = uv + float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
            o.uv[4] = uv - float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
            return o;
        }
        v2f vertBlurHorizontal(appdata_img v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            // 通过把计算采样纹理坐标的代码从片元着色器转移到顶点着色器中，可以减少运算，提高性能
            half2 uv = v.vertex;
            o.uv[0] = uv;
            o.uv[1] = uv + float2( _MainTex_TexelSize.x * 1.0, 0.0) * _BlurSize;
            o.uv[2] = uv - float2( _MainTex_TexelSize.x * 1.0, 0.0) * _BlurSize;
            o.uv[3] = uv + float2( _MainTex_TexelSize.x * 2.0, 0.0) * _BlurSize;
            o.uv[4] = uv - float2( _MainTex_TexelSize.x * 2.0, 0.0) * _BlurSize;
            return o;
        }
        fixed4 fragBlur(v2f i) : SV_Target {
            // 一个5x5的二维高斯核拆分成的两个大小为5的一维高斯核的三个高斯权重
            float weight[3] = {0.4026, 0.2442, 0.0545};
            fixed3 sum = tex2D(_MainTex, i.uv[0]).rgb * weight[0];
            for(int it = 1; it < 3; it ++){
                sum += tex2D(_MainTex, i.uv[it *  2 - 1]).rgb * weight[it];
                sum += tex2D(_MainTex, i.uv[it * 2]).rgb * weight[it];
            }
            return fixed4(sum, 1.0);
        }
        ENDCG

        Pass
        {
            NAME "GAUSSIAN_BLUR_VERTICAL"
            CGPROGRAM
            #pragma vertex vertBlurVertical
            #pragma fragment fragBlur
            ENDCG
        }

        Pass
        {
            NAME "GAUSSIAN_BLUR_HORIZONTAL"
            CGPROGRAM
            #pragma vertex vertBlurHorizontal
            #pragma fragment fragBlur
            ENDCG
        }
    }
    Fallback Off
}
