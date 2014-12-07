using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nsun.NSFramework
{
    public class WeakAction
    {
        internal readonly Type _parameterType;
        readonly Type _delegateType;
        readonly MethodInfo _method;
        readonly WeakReference _targetRef;
        internal WeakAction(object target, MethodInfo method, Type parameterType)
        {
            if (target == null)
                _targetRef = null;
            else
                _targetRef = new WeakReference(target);

            _method = method;
            if (_parameterType == null)
                _parameterType = typeof(Action);
            else
                _parameterType = typeof(Action<>).MakeGenericType(parameterType);
        }


        internal Delegate CreateAction()
        {
            if (_targetRef == null)
                return Delegate.CreateDelegate(_parameterType, _method);
            else
            {
                try
                {
                    object obj = _targetRef.Target;
                    if (obj != null)
                    {
                        return Delegate.CreateDelegate(_parameterType,obj, _method);
                    }
                }
                catch 
                { }
                return null;
            }
        }
    }
}
