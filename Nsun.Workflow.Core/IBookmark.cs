using Nsun.Workflow.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core
{
    public interface IBookmark
    {
        string GetSerialContent();
        bool CanPersistence { get; }
        bool IsPersistence { get; set; }
        event EventHandler Starting;
        event EventHandler Ending;
        event EventHandler Transacting;
        void Start(TransInfoDto transInfoDto);
        void End(TransInfoDto transInfoDto);
        void Transact(TransInfoDto transInfoDto);
    }
}
