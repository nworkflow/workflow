using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nsun.NSFramework
{
    public class Messenger
    {

        public void Register(string msg, Action callBack)
        {
            this.Register(msg, callBack, null);
        }


        public void Register<T>(string msg,Action callBack)
        {
            this.Register(msg, callBack, typeof(T));
        }


        public void Send(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                throw new ArgumentNullException("msg can't null");

            Type registerMsg;
            if (_mapping.TryGetParameterType(msg, out registerMsg))
            {
                if (registerMsg == null)
                    throw new InvalidOperationException("");
            }

            var actions = _mapping.GetWeakAction(msg);
            if (actions != null)
                actions.ForEach(p => p.DynamicInvoke());
        }


        public void Send(string msg,object parameter)
        {
            if (string.IsNullOrEmpty(msg))
                throw new ArgumentNullException("msg can't null");

            Type registerMsg;
            if (_mapping.TryGetParameterType(msg, out registerMsg))
            {
                if (registerMsg == null)
                    throw new InvalidOperationException("");
            }

            var actions = _mapping.GetWeakAction(msg);
            if (actions != null)
                actions.ForEach(p => p.DynamicInvoke(parameter));
        }


        private MessengerMapping _mapping = new MessengerMapping();
        private void Register(string msg, Delegate callBack, Type parameterType)
        {
            if (string.IsNullOrEmpty(msg))
                throw new ArgumentNullException("msg");

            if (callBack == null)
                throw new ArgumentNullException("callback");

            IsValidate(msg, parameterType);
            _mapping.Register(msg, callBack.Target, callBack.Method, parameterType);
            
        }


        private void IsValidate(string msg,Type parameterType)
        {
            Type alreadyRegisterType = null;
            if (_mapping.TryGetParameterType(msg, out alreadyRegisterType))
            {
                if (alreadyRegisterType != null && parameterType != null)
                {
                    if (!alreadyRegisterType.Equals(parameterType))
                    {
                        throw new InvalidOperationException(string.Format("The registered action's parameter type is inconsistent with the previously registered actions for message '{0}'.\nExpected: {1}\nAdding: {2}",
                            msg,
                            alreadyRegisterType.FullName,
                            parameterType.FullName));
                    }
                }
            }
            else
            {
                if (alreadyRegisterType != parameterType)
                {
                    throw new InvalidOperationException(string.Format("The registered action's parameter type is inconsistent with the previously registered actions for message '{0}'.\nExpected: {1}\nAdding: {2}",
                            msg,
                            alreadyRegisterType.FullName,
                            parameterType.FullName));
                }
            }
        }
    }
}
