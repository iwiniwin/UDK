using UnityEngine;
using UnityEngine.UI;
using static UKit.Utils.Output;

public class RaycastTargetUGUI : MonoBehaviour
{
    #if UNITY_EDITOR

    public Color lineColor = Color.blue;

    static Vector3[] fourCorners = new Vector3[4];

    private void OnDrawGizmos() {
        foreach(MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>()){
            if(g.raycastTarget){
                RectTransform rectTransform = g.transform as RectTransform;
                rectTransform.GetWorldCorners(fourCorners);
                Gizmos.color = lineColor;
                for (int i = 0; i < 4; i++)
                {
                    Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
                }
            }
        }
    }
    #endif
}
