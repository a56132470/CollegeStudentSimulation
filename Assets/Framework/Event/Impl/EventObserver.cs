using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Framework.Macro;
using Framework.Pool;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Framework.Event
{
    public class EventObserver<T> : IObservable<T>,IClear where T: IEvent
    {
        private Dictionary<System.Delegate, MethodInfo> _observers;

        private Dictionary<Delegate, MethodInfo> Observers 
            => _observers ?? (_observers = new Dictionary<Delegate, MethodInfo>(1));

        private object[] _args;
        private object[] Args => _args ?? (_args = new object[1]);
        public bool AddObserver<TEvent>(Action<TEvent> observer) where TEvent : T, new()
        {
            if (observer == null) return false;
            CheckAddObserver(observer);
            Observers.Add(observer,observer.Method);
            return true;
        }

        public bool RemoveObserver<TEvent>(Action<TEvent> observer) where TEvent : T, new()
        {
            if (observer == null) return false;
            CheckRemoveObserver(observer);
            Observers.Remove(observer);
            return true;
        }

        public bool Notify<TEvent>(TEvent e) where TEvent : T, new()
        {
            // TODO：
            if (_observers == null || _observers.Count <= 0) return false;
            object[] dto;
            if (Args[0] == null)
            {
                Args[0] = e;
                dto = Args;
            }
            else
            {
                dto = new object[]{e};
            }

            var enumerator = Observers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                try
                {
                    enumerator.Current.Value.Invoke(enumerator.Current.Key.Target, dto);
                }
                catch (Exception exception)
                {
                    var last = Application.GetStackTraceLogType(LogType.Error);
                    Application.SetStackTraceLogType(LogType.Error,StackTraceLogType.None);
                    Debug.LogError($"EventObserver.Notify Error \n" +
                                   $"Occur :{enumerator.Current.Value.DeclaringType?.FullName}:{enumerator.Current.Value?.Name},\n" +
                                   $"StackTrace :{StackTraceUtility.ExtractStringFromException(exception)}");
                    Application.SetStackTraceLogType(LogType.Error,last);
                }
            }
            enumerator.Dispose();
            Args[0] = null;
            return true;
        }
        
        public void Clear()
        {
            _observers?.Clear();
            _observers = null;
            _args = null;
        }
        
        [Conditional(MacroDefine.DEBUG),Conditional(MacroDefine.UNITY_EDITOR)]
        private void CheckAddObserver<TEvent>(Action<TEvent> observer) where TEvent : T, new()
        {
            if (Observers.ContainsKey(observer))
            {
                throw new Exception(
                    $"Observer Already Exist!!{observer.Method.DeclaringType?.FullName} : {observer.Method.Name}");
            }
        }
        
        [Conditional(MacroDefine.DEBUG),Conditional(MacroDefine.UNITY_EDITOR)]
        private void CheckRemoveObserver<TEvent>(Action<TEvent> observer) where TEvent : T, new()
        {
            if (!Observers.ContainsKey(observer))
            {
                throw new Exception(
                    $"Observer Doesn't Exist!!{observer.Method.DeclaringType?.FullName} : {observer.Method.Name}");
            }
        }
    }
}