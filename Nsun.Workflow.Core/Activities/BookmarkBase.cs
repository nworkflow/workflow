using Nsun.Workflow.Core.Args;
using Nsun.Workflow.Core.Dtos;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Models
{
   public abstract class BookmarkBase : IBookmark, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _name = "标准节点";
        [System.ComponentModel.Category("主属性")]
        [System.ComponentModel.DisplayName("节点名称")]
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }

        private string _nodeType;
        [System.ComponentModel.Category("主属性")]
        [System.ComponentModel.DisplayName("节点类型")]
        [System.ComponentModel.ReadOnly(true)]
        public string NodeType
        {
            get { return _nodeType; }
            set
            {
                _nodeType = value;
                RaisePropertyChanged("NodeType");
            }
        }

        private KeyValuePair<int, int> _size = new KeyValuePair<int, int>(50, 50);
        /// <summary>
        /// 节点尺寸
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public KeyValuePair<int, int> Size
        {
            get { return _size; }
            set
            {
                _size = value;
                RaisePropertyChanged("Size");
            }
        }

        [System.ComponentModel.Category("主属性")]
        [System.ComponentModel.DisplayName("节点标识")]
        [System.ComponentModel.ReadOnly(true)]
        public Guid? NodeID { get; set; }

        private string _groupName { get; set; }
        [System.ComponentModel.DisplayName("分组名称")]
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
                RaisePropertyChanged("GroupName");
            }
        }


        protected string GetSerialContent(Type type)
        {
            return System.Text.RegularExpressions.Regex.Replace(DesingnerItemStyle.GetDesignerContext(type), "^[^<]", "");
        }


        public virtual string GetSerialContent()
        {
            return string.Empty;
        }

        private bool _isPersistence=true;
        [System.ComponentModel.DisplayName("生成节点")]
        [System.ComponentModel.Browsable(false)]
        public virtual bool IsPersistence
        {
            get
            {
                return CanPersistence && _isPersistence;
            }
            set
            {
                _isPersistence = value;
                RaisePropertyChanged("IsPersistence");
            }
        }


        private bool _canPersistence;
        [System.ComponentModel.Browsable(false)]
        public  bool CanPersistence
        {
            get { return _canPersistence; }
            protected set
            {
                _canPersistence = value;
            }
        }

        public virtual void StartBuiness(TransInfoDto transInfoDto)
        {
            if (Starting != null)
                Starting(this,new WFArgs(transInfoDto));
        }

        public void Start(TransInfoDto transInfoDto)
        {
            try
            {
                StartBuiness(transInfoDto);
            }
            catch (Exception ex)
            {
                throw new WFRoutingException(this.ToString() + "在创建节点时候出现异常：" + ex.ToString());
            }
        }


        public virtual void EndBuiness(TransInfoDto transInfoDto)
        {
            if (Ending != null)
                Ending(this, new WFArgs(transInfoDto));
        }

        public void End(TransInfoDto transInfoDto)
        {
            try
            {
                EndBuiness(transInfoDto);
            }
            catch (Exception ex)
            {
                throw new WFRoutingException(this.ToString() + "在结束节点时候出现异常：" + ex.ToString());
            }
        }
        
        public void Transact(TransInfoDto transInfoDto)
        {
            TransactBuiness(transInfoDto);
        }

        public virtual void TransactBuiness(TransInfoDto transInfoDto)
        {
            try
            {
                if (Transacting != null)
                    Transacting(this, new WFArgs(transInfoDto));
            }
            catch (Exception ex)
            {
                throw new WFRoutingException(this.ToString() + "在执行的时候出现异常：" + ex.ToString());
            }
        }


        public virtual string Validate()
        {
            return string.Empty;
        }


        public event EventHandler Starting;

        public event EventHandler Ending;

        public event EventHandler Transacting;
    }
}
