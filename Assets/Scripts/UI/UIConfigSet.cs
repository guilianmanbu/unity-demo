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
    public class UIConfigSet
    {
        /// <summary>
        /// 主界面
        /// </summary>
        public static readonly UIConfig MainView = 
            new UIConfig(
                "MainView",
                new string[]
                {

                },
                LayerType.Panel,
                false,
                false);

        public static readonly UIConfig BagView =
            new UIConfig(
                "BagView",
                new string[]
                {

                },
                LayerType.Panel,
                false,
                true);

        public static readonly UIConfig ShopView =
            new UIConfig(
                "ShopView",
                new string[]
                {

                },
                LayerType.Panel,
                false,
                true);

        public static readonly UIConfig FriendView =
            new UIConfig(
                "FriendView",
                new string[]
                {

                },
                LayerType.Panel,
                false,
                true);

        public static readonly UIConfig AlertView1 =
           new UIConfig(
               "AlertView1",
               new string[]
               {

               },
               LayerType.Alert,
               true,
               true);

        public static readonly UIConfig AlertView2 =
           new UIConfig(
               "AlertView2",
               new string[]
               {

               },
               LayerType.Alert,
               true,
               true);

        public static readonly UIConfig AlertView3 =
           new UIConfig(
               "AlertView3",
               new string[]
               {

               },
               LayerType.Alert,
               false,
               true);
    }
}
