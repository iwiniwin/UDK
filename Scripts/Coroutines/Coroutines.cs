using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace UDK.Coroutines
{
    public sealed class Coroutines
    {
        private static Coroutines Instance = null;

        private Dictionary<string, List<Coroutine>> m_CoroutineDict = new Dictionary<string, List<Coroutine>>();
        private Coroutines()
        {

        }
        public static Coroutine Start(IEnumerator routine, object target = null)
        {
            return InternalStart(new Coroutine(routine, target));
        }

        public static Coroutine Start(string methodName, object target)
        {
            return Start(methodName, null, target);
        }

        public static Coroutine Start(string methodName, object value, object target)
        {
            MethodInfo methodInfo = target.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
            {
                Debug.LogError($"Coroutine '{methodName}' couldn't be started!");
                return null;
            }
            object returnValue;
            if (value == null)
            {
                returnValue = methodInfo.Invoke(target, null);
            }
            else
            {
                returnValue = methodInfo.Invoke(target, new object[] { value });
            }
            if (returnValue is IEnumerator)
            {
                return InternalStart(new Coroutine((IEnumerator)returnValue, methodName, target));
            }
            return null;
        }

        private static Coroutine InternalStart(Coroutine coroutine)
        {
            if(Instance == null)
            {
                Instance = new Coroutines();
            }
            Instance.Start(coroutine);
            return coroutine;
        }

        private void Start(Coroutine coroutine)
        {

        }

        public static void Stop(IEnumerator routine, object target)
        {

        }

        public static void Stop(Coroutine routine)
        {

        }

        public static void Stop(string methodName, object target)
        {

        }

        public static void StopAll(object target)
        {

        }

        public static void StopAll()
        {

        }
    }

    public sealed class Coroutine : YieldInstruction
    {
        private int m_Key = 0;
        private int m_TargetKey = 0;

        public int Key => m_Key;
        public int TargetKey => m_TargetKey; 
        
        private IEnumerator m_Routine;
        internal IEnumerator Routine => m_Routine; 

        internal ICoroutineYield CurrentYield = new YieldDefault();

        internal Coroutine(IEnumerator routine, string methodName, object target)
        {
            this.m_Key = methodName.GetHashCode();
            this.m_TargetKey = target.GetHashCode();
            this.m_Routine = routine;
        }

        internal Coroutine(IEnumerator routine, object target)
        {
            this.m_Key = routine.GetHashCode();
            if(target != null)
            {
                this.m_TargetKey = target.GetHashCode();
            }
            this.m_Routine = routine;
        }
    }

    public interface ICoroutineYield
    {
        bool IsDone();
    }

    public class YieldDefault : ICoroutineYield
    {
        public bool IsDone()
        {
            return true;
        }
    }
}