using UnityEngine;
using UnityEngine.Events;
using static UKit.Utils.Output;

namespace UKit.Core{
    public class GC : MonoBehaviour
    {
        public UnityAction m_Callback;
        public AsyncOperation m_FirstOperation;
        public AsyncOperation m_SecondOperation;

        /// <summary>
        /// 卸载无用资源
        /// 在C#中，可能还会有很多临时对象引用游戏资源，这可能会导致 Resources.UnloadUnusedAssets 无法释放掉
        /// 因此在卸载无用资源前，需要保证C#完成垃圾收集工作，而且有时候进行一遍垃圾回收工作是没用的，最好调用两遍
        /// </summary>
        /// <param name="callback">卸载无用资源完成回调</param>
        public void UnloadUnusedAssets(UnityAction callback){
            m_Callback = callback;
            System.GC.Collect();
            m_FirstOperation = Resources.UnloadUnusedAssets();
        }

        // Update is called once per frame
        void Update()
        {
            if(m_SecondOperation != null && m_SecondOperation.isDone){
                m_SecondOperation = null;
                m_Callback();
                // 删除自身
                DestroyImmediate(this);
                return;
            }
            if(m_FirstOperation != null && m_FirstOperation.isDone){
                m_FirstOperation = null;
                System.GC.Collect();
                m_SecondOperation = Resources.UnloadUnusedAssets();
            }
        }
    }
}


