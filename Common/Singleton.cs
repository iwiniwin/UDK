/*
 * @Author: iwiniwin
 * @Date: 2020-11-06 00:52:28
 * @LastEditors: Please set LastEditors
 * @LastEditTime: 2021-04-26 17:50:09
 * @Description: 单例
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDK
{

    public abstract class Singleton<T> where T : new()
    {
        private static T _instance;
        private static object _lock = new object();
        public static T Instance
        {
            get
            {
                if(_instance == null){
                    lock(_lock){
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }
    }


    public class UnitySingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance{
            get{
                if(_instance == null){
                    _instance = FindObjectOfType(typeof(T)) as T;
                }
                if(_instance == null){
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent(typeof(T)) as T;
                    GameObject parent = GameObject.Find("Boot");
                    if(parent == null)
                    {
                        parent = new GameObject("Boot");
                        GameObject.DontDestroyOnLoad(parent);
                    }
                    obj.transform.parent = parent.transform;
                }
                return _instance;
            }
        }

        private void Awake() {
            DontDestroyOnLoad(this.gameObject);
            if(_instance == null){
                _instance = this as T;
            }else{
                Destroy(this.gameObject);
            }
        }

    }

}
