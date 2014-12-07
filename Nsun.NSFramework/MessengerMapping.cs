using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nsun.NSFramework
{
    internal class MessengerMapping
    {
        readonly Dictionary<string, List<WeakAction>> _maps = new Dictionary<string, List<WeakAction>>();

        internal MessengerMapping()
        {

        }


        internal void Register(string msg, object target, MethodInfo method, Type type)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");
            if (method == null)
                throw new ArgumentNullException("method");

            lock (_maps)
            {
                if (!_maps.ContainsKey(msg))
                    _maps[msg] = new List<WeakAction>();

                _maps[msg].Add(new WeakAction(target, method, type));
            }
        }


        internal List<Delegate> GetWeakAction(string msg)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");
            List<Delegate> actions = null;
            List<WeakAction> weakActions = _maps[msg];

            actions = new List<Delegate>(weakActions.Count);
            for (int i = weakActions.Count - 1; i > -1; i--)
            {
                WeakAction weakAction = weakActions[i];
                if (weakAction == null)
                    continue;

                Delegate @delegate = weakAction.CreateAction();
                if (@delegate != null)
                    actions.Add(@delegate);
                else
                    weakActions.Remove(weakAction);
            }

            if(weakActions.Count==0)
                _maps.Remove(msg);

            actions.Reverse();
            return actions;
        }


        internal bool TryGetParameterType(string msg,out Type parameterType)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");

            parameterType = null;
            List<WeakAction> weakActions = null;
            lock (_maps)
            {
                if (!_maps.TryGetValue(msg, out weakActions)||weakActions.Count==0)
                {
                    return false;
                }

                parameterType = weakActions[0]._parameterType;
                return true;
            }
        }
    }
}
