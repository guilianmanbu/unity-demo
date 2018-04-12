using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/12/2018 11:44:36 AM
/// </summary>


namespace tx.ui
{
    public enum LayerType
    {
        Panel,
        Alert,
        None,
    }

    public class UILayerManager
    {
        public const int AlertNum = 5;

        public Canvas rootCanvas;

        public RectTransform PanelLayer;
        public RectTransform AlertLayer;

        public List<RectTransform> AlertLayers;
        public List<RectTransform> AlertMaskLayers;

        public RectTransform MainResourceLayer;
        public RectTransform BackButtonLayer;

        public RectTransform ServerLayer;

        public RectTransform NoneUILayer;

        public RectTransform AlertMask;

        private static UILayerManager instance;
        public static UILayerManager Instance { get { return instance ?? (instance = new UILayerManager()); } }

        public void Init()
        {
            rootCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            PanelLayer = rootCanvas.transform.FindChild("PanelLayer") as RectTransform;
            AlertLayer = rootCanvas.transform.FindChild("AlertLayer") as RectTransform;
            MainResourceLayer = rootCanvas.transform.FindChild("MainResourceLayer") as RectTransform;
            BackButtonLayer = rootCanvas.transform.FindChild("BackButtonLayer") as RectTransform;
            ServerLayer = rootCanvas.transform.FindChild("ServerLayer") as RectTransform;
            NoneUILayer = rootCanvas.transform.FindChild("NoneUILayer") as RectTransform;
            AlertMask = AlertLayer.FindChild("AlertMask") as RectTransform;


            BackButtonLayer.gameObject.SetActive(false);
            AlertMask.gameObject.SetActive(false);
        }

        public void AddUIToLayer(GameObject go,LayerType type)
        {
            RectTransform layer = GetLayerOfType(type);
            go.transform.SetParent(layer, false);
            go.transform.SetAsLastSibling();
        }

        public RectTransform GetLayerOfType(LayerType type)
        {
            switch (type)
            {
                case LayerType.Panel: return PanelLayer;
                case LayerType.Alert: return AlertLayer;
                case LayerType.None: return NoneUILayer;
                default: return NoneUILayer;
            }
        }

        public void AddMaskForAlert(Transform alert)
        {
            int index = alert.GetSiblingIndex() - 1;
            if (index < 0) index = 0;
            AlertMask.SetSiblingIndex(index);

            //txDebug.Log(alert.name+"  "+index);
        }

        public void ShowBackButton(bool show)
        {
            //BackButtonLayer
        }
    }
}
