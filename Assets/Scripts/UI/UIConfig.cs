using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/11/2018 3:00:06 PM
/// </summary>


namespace tx.ui
{
    public class UIConfig
    {
        /// <summary>
        /// 主ui资源path
        /// </summary>
        public string MainModuleResPath;

        /// <summary>
        /// 需要的ui的path
        /// </summary>
        public string[] ResPathList;

        /// <summary>
        /// 弹出层级
        /// </summary>
        public LayerType PopUpType;

        /// <summary>
        /// 0：关闭，1：打开
        /// </summary>
        public int State;

        /// <summary>
        /// 是否显示返回按钮
        /// </summary>
        public bool ShowBackBtn;

        /// <summary>
        /// 是否需要遮背景
        /// </summary>
        public bool NeedMask;

        public UIConfig(string mainModuleResPath,
                        string[] resPathList,
                        LayerType popUpType,
                        bool needMask,
                        bool showBackBtn)
        {
            MainModuleResPath = mainModuleResPath;
            ResPathList = resPathList;
            PopUpType = popUpType;
            NeedMask = needMask;
            ShowBackBtn = showBackBtn;
        }
    }
}
