using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Common.Utils
{
    /// <summary>
    /// 提交节点等事件上锁，防止多次调用
    /// </summary>
    public class Locker:Dictionary<Guid,DateTime>
    {
        private int _outTime; // 超时时间设定
        List<Guid> lockers=null; // 锁定对象并且用于做清空处理
        public Locker(int outTime)
        {
            lockers = new List<Guid>();
            _outTime = outTime;
        }


        // 添加集合，重写父类方法
        /// <summary>
        /// 添加新节点
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <returns>操作结果</returns>
        public bool Add(Guid nodeId)
        {
            lock (lockers)
            {
                if (base.ContainsKey(nodeId))
                    return false;
                base.Add(nodeId, DateTime.Now);

                foreach (var item in base.Keys)
                {
                    if ((DateTime.Now - base[item]).TotalSeconds > _outTime)
                    {
                        lockers.Add(item);
                    }
                }
                Remove(lockers);
                lockers.Clear();
                return true;
            }
        }


        /// <summary>
        /// 移除集合，覆盖父类方法
        /// </summary>
        /// <param name="nodeIds">节点集合</param>
        private void Remove(List<Guid> nodeIds)
        {
            foreach (var nodeId in nodeIds)
            {
                base.Remove(nodeId);
            }
        }
    }
}
