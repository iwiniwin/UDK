using UnityEngine;
using System;
using System.Collections.Generic;
using static UKit.Utils.Output;

public class ErrorDisplay : MonoBehaviour
{
    // 错误详情
    private List<String> m_LogEntries = new List<String>();
    // 是否显示错误窗口
    private bool m_IsVisible = false;
    // 窗口显示区域
    private Rect m_WindowRect = new Rect(0, 0, Screen.width, Screen.height);
    // 窗口滚动区域
    private Vector2 m_ScrollPositionText = Vector2.zero;

    void Start()
    {
       // 监听错误
       Application.logMessageReceived += (condition, stackTrace, type) => {
           if(type == LogType.Exception || type == LogType.Error){
               if(!m_IsVisible){
                   m_IsVisible = true;
               }
               m_LogEntries.Add(String.Format("{0}\n{1}", condition, stackTrace));
           }
       };

        // 创建异常以及错误
        // for (int i = 0; i < 10; i++)
        // {
        //     Debug.LogError("mom\nlslfl\n\n\nsflslffo");
        // }
        // int[] a = null;
        // a[1] = 3;
    }

    private void OnGUI() {
        if(m_IsVisible){
            m_WindowRect = GUILayout.Window(0, m_WindowRect, ConsoleWindow, "Console");
        }
    }

    // 日志窗口
    void ConsoleWindow(int windowID){
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Clear")){
            m_LogEntries.Clear();
        }
        if(GUILayout.Button("Close")){
            m_IsVisible = false;
        }
        GUILayout.EndHorizontal();

        m_ScrollPositionText = GUILayout.BeginScrollView(m_ScrollPositionText);
        foreach (var entry in m_LogEntries)
        {
            Color currentColor = GUI.contentColor;
            GUI.contentColor = Color.red;
            GUILayout.TextArea(entry);
            GUI.contentColor = currentColor;
        }
        GUILayout.EndScrollView();
    }
}
