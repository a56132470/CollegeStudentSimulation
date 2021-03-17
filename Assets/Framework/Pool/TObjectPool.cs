using System;
using System.Collections.Generic;

namespace Framework.Pool
{
    public class TObjectPool<T> where T : IClear, new()
    {
        private static Stack<T> _stack;

        static TObjectPool()
        {
            _stack = new Stack<T>();
        }
        
        public T AutoCreate()
        {
            if (_stack!=null && _stack.Count != 0)
            {
                return _stack.Pop();
            }
        
            return Activator.CreateInstance<T>();
        }

        public static void Recycle(T obj)
        {
            if (obj!=null)
            {
                obj.Clear();
                if (_stack==null) _stack = new Stack<T>(1);
                
                _stack.Push(obj);
            }
        }
    }
}