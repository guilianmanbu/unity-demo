using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/12/2018 11:44:36 AM
/// </summary>


namespace tx.ui
{
    public class UIViewBase : MonoBehaviour
    {
        public UIModelBase Model;

        public virtual void Open(UIModelBase model)
        {
            Model = model;
        }

        public virtual void Close()
        {

        }

        /// <summary>
        /// 从背景重新变为最前面
        /// </summary>
        public virtual void ReTop()
        {

        }

        /// <summary>
        /// 回退到此，重新显示
        /// </summary>
        public virtual void Show()
        {
            Open(Model);
        }

        public virtual void Hide()
        {
            Close();
        }

        public virtual void Back()
        {
            UIManager.Instance.Back();
        }

        public virtual void RefreshRedDot()
        {

        }
    }
}
