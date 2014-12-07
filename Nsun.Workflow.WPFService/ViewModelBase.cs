using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Nsun.Servcie.Common
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isBusy;
        /// <summary>
        /// 是否为等待状态
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set 
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                RaisePorpertyChange("IsBusy");
            }
        }

        private int _totalCount;
        /// <summary>
        /// 总计操作的数量
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            set 
            {
                if (_totalCount == value)
                    return;
                _totalCount = value;
                RaisePorpertyChange("TotalCount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePorpertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <summary>
        /// 多值变通知
        /// </summary>
        /// <param name="propertieNames">参数集合</param>
        protected void RaisePropertyChange(params string[] propertieNames)
        {
            if(PropertyChanged!=null)
            {
                foreach (var propertyName in propertieNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}


