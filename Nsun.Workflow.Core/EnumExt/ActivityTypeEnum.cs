using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.EnumExt
{
    /// <summary>
    /// 活动类型枚举
    /// </summary>
    public enum ActivityTypeEnum
    {
        Process,  // 可执行节点
        Decision, // 分支
        Start,    // 启动节点
        Parallel,  // 并行节点
        SubRouting, // 调用子流程
        Connection,  // 连线
        SubRoutings, // 调用多子流程
        NotifyMsg, // 消息
        Process_Parallel // 并发开始
    }
}
