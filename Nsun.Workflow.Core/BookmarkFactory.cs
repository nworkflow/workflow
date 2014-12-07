using Nsun.Workflow.Core.Activities;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core
{
    public class BookmarkFactory
    {
        public static BookmarkBase GetBookmark(ActivityTypeEnum typeEnum)
        {
            switch (typeEnum)
            {
                case ActivityTypeEnum.Process:
                    return new StandBookmark();
                case ActivityTypeEnum.Decision:
                    return new SwitchBookmark();
                case ActivityTypeEnum.Start:
                    return new StartBookmark();
                case ActivityTypeEnum.Parallel:
                    return new ParalleBookmark();
                case ActivityTypeEnum.SubRouting:
                    return new SubRoutingBookmark();
                case ActivityTypeEnum.SubRoutings:
                    return new SubRoutingsBookmark();
                case ActivityTypeEnum.NotifyMsg:
                    return new NotifyMsgBookmark();
            }

            return null;
        }


        public static Type GetBookmarkType(ActivityTypeEnum typeEnum)
        {
            switch (typeEnum)
            {
                case ActivityTypeEnum.Process:
                    return typeof(StandBookmark);
                case ActivityTypeEnum.Decision:
                    return typeof(SwitchBookmark);
                case ActivityTypeEnum.Start:
                    return typeof(StartBookmark);
                case ActivityTypeEnum.Parallel:
                    return typeof(ParalleBookmark);
                case ActivityTypeEnum.SubRouting:
                    return typeof(SubRoutingBookmark);
                case ActivityTypeEnum.SubRoutings:
                    return typeof(SubRoutingsBookmark);
                case ActivityTypeEnum.NotifyMsg:
                    return typeof(NotifyMsgBookmark);
            }
            return null;
        }
    }
}
