using UnityEngine;
using static UKit.Utils.Output;

public class FPS : MonoBehaviour
{
    public float updateInterval = 0.5f;
    private float accum = 0;
    private int frames = 0;
    private float timeleft;
    private string stringFps;

    void Start()
    {
       timeleft = updateInterval;

       Application.targetFrameRate = 30;  // 强制设置FPS最高30帧
       // Additionally if the QualitySettings.vSyncCount property is set, 
       // the targetFrameRate will be ignored and instead the game will use the vSyncCount 
       // and the platform's default render rate to determine the target frame rate
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        // Time.timeScale 影响的是Unity的游戏时间缩放比例，比如 Time.timeScale = 2，则 Time.time 的增长速度会变成2倍
        // Time.deltaTime 表示距离上一帧所经过的时间，单位秒。假如1秒30帧，则增量时间就是 1 / 30
        accum += Time.timeScale / Time.deltaTime;  // 表示每调用一次Update，统计一次帧数
        ++frames;
        if(timeleft <= 0.0){
            float fps = accum / frames;  // 取帧数的平均值
            string format = System.String.Format("{0:F1} FPS", fps);
            stringFps = format;
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }

    private void OnGUI() {
        GUIStyle guiStyle = GUIStyle.none;
        guiStyle.fontSize = 30;
        guiStyle.normal.textColor = Color.red;
        // guiStyle.alignment = TextAnchor.LowerRight;
        Rect rt = new Rect(0, 0, 100, 100);
        GUI.Label(rt, stringFps, guiStyle);
    }
}
