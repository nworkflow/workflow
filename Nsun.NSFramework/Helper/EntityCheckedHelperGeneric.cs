using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.NSFramework.Helper
{
    public class EntityCheckedHelperGeneric<T> :ViewModelBase where T:class,new()
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    RaisePropertyChanged("IsChecked");
                }
            }
        }

        private T _tEntity;
        public T TEntity
        {
            get { return _tEntity; }
            set {
                if(_tEntity != value)
                {
                    _tEntity = value;
                    RaisePropertyChanged("TEntity");
                }
            }
        }


        public EntityCheckedHelperGeneric(T tEntity, bool isChecked = false)
        {
            this._tEntity = tEntity;
            this._isChecked = isChecked;
        }
    }
}
