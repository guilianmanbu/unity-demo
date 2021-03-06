using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/11/2018 2:45:49 PM
/// </summary>

namespace tx.ui
{
    public class UIManager 
    {
        private readonly List<UIConfig> _stack = new List<UIConfig>();

        private readonly Dictionary<UIConfig, UIViewBase> _cachedViews = new Dictionary<UIConfig, UIViewBase>();
        private readonly Dictionary<UIConfig, UIViewBase> _constViews = new Dictionary<UIConfig, UIViewBase>();

        private Dictionary<UIConfig, List<UIConfig>> _alertListOfPanelDic = new Dictionary<UIConfig, List<UIConfig>>();  // 面板的弹窗队列
        private Dictionary<UIConfig, List<UIConfig>> _alertListOfAlertDic = new Dictionary<UIConfig, List<UIConfig>>();      // 弹窗所在的队列

        private UIConfig _currentPanelConfig;
        public UIConfig CurrentPanelConfig { get { return _currentPanelConfig; } }



        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                return instance ?? (instance = new UIManager());
            }
        }

        public void Init()
        {

        }

        public void OpenUI(UIConfig vo,UIModelBase model)
        {
            OpenUI(vo, model, false);
        }

        public void OpenUI(UIConfig vo,UIModelBase model,bool closePrevPanel)
        {
            if (vo.State!=1)
            {
                LoadUI(vo, model, closePrevPanel);
            }
        }

        public void Back()
        {
            UIConfig vo = GetTopUI();
            if (vo != null)
            {
                CloseUI(vo);

                vo = GetTopUI();
                if (vo != null)
                {
                    if (vo.State == 0)  // 被隐藏过，就从一级UI开始
                    {
                        ShowUI(_currentPanelConfig);
                    }
                    else
                    {
                        _cachedViews[vo].transform.SetAsLastSibling();
                        _cachedViews[vo].ReTop();
                    }
                }
                else
                {
                    // 空栈了
                    AddToStack(UIConfigSet.MainView, false);
                    ShowUI(UIConfigSet.MainView);
                }
            }
            else
            {
                AddToStack(UIConfigSet.MainView, false);
                ShowUI(UIConfigSet.MainView);
            }

            vo = GetTopUI();
            SetUIState(vo);
        }

        public UIConfig GetTopUI()
        {
            UIConfig vo = null;
            if (_currentPanelConfig != null)
            {
                List<UIConfig> alertList;
                if(_alertListOfPanelDic.TryGetValue(_currentPanelConfig,out alertList))
                {
                    if (alertList.Count > 0)
                    {
                        vo = alertList[alertList.Count - 1];
                    }
                    else
                    {
                        vo = _currentPanelConfig;
                    }
                }
                else
                {
                    vo = _currentPanelConfig;
                }
            }

            return vo;
        }



        private void LoadUI(UIConfig vo,UIModelBase model,bool closePrevPanel)
        {
            UIViewBase view = null;
            if (!_cachedViews.TryGetValue(vo,out view))
            {
                GameObject prefab = TxResource.Instance.LoadGameObject(vo.MainModuleResPath);
                view = prefab.GetComponent<UIViewBase>();
                _cachedViews.Add(vo, view);
            }

            AddToStack(vo, closePrevPanel);

            LoadUIOk(vo, model,view);
        }

        private void LoadUIOk(UIConfig vo,UIModelBase model,UIViewBase view)
        {
            UILayerManager.Instance.AddUIToLayer(view.gameObject, vo.PopUpType);
            
            vo.State = 1;

            view.gameObject.SetActive(true);
            view.Open(model);

            SetUIState(vo);
        }

        private void AddToStack(UIConfig vo,bool closePrevPanel)
        {
            if (vo.PopUpType == LayerType.Panel)
            {
                if (_currentPanelConfig != null)
                {
                    if (closePrevPanel)
                    {
                        CloseUI(_currentPanelConfig);
                    }
                    else
                    {
                        HideUI(_currentPanelConfig);
                    }
                }
                
                PushStack(vo);
            }
            else
            {
                if (_currentPanelConfig != null)
                {
                    PushStack(vo);
                }
                else
                {
                    throw new System.Exception("没有一级面板,怎么打开了一个二级面板" + vo.MainModuleResPath);
                }
            }
        }

        private void PushStack(UIConfig vo)
        {
            if (vo.PopUpType == LayerType.Panel)
            {
                // 不重复,移除栈中以前的
                if (_stack.Contains(vo))
                {
                    List<UIConfig> alertList;
                    if (_alertListOfPanelDic.TryGetValue(vo, out alertList))
                    {
                        while (alertList.Count > 0)
                        {
                            PopStack(alertList[0]);
                        }
                    }
                    _stack.Remove(vo);
                }

                // 入栈
                _stack.Add(vo);
                _currentPanelConfig = vo;
            }
            else
            {
                // 以前的退栈
                PopStack(vo);

                List<UIConfig> alertList;
                if(!_alertListOfPanelDic.TryGetValue(_currentPanelConfig,out alertList))
                {
                    alertList = new List<UIConfig>();
                    _alertListOfPanelDic.Add(_currentPanelConfig, alertList);
                }

                // 入栈
                alertList.Add(vo);
                _alertListOfAlertDic.Add(vo, alertList);
            }
        }

        private void PopStack(UIConfig vo)
        {
            if (vo.PopUpType==LayerType.Panel)
            {
                _stack.Remove(vo);

                _currentPanelConfig = _stack.Count > 0 ? _stack[_stack.Count - 1] : null;
            }
            else
            {
                List<UIConfig> alertList;
                if (_alertListOfAlertDic.TryGetValue(vo, out alertList))
                {
                    alertList.Remove(vo);
                    _alertListOfAlertDic.Remove(vo);
                }
            }
        }

        private void CloseUI(UIConfig vo)
        {
            if (vo.PopUpType==LayerType.Panel)
            {
                List<UIConfig> alertList;
                if(_alertListOfPanelDic.TryGetValue(vo,out alertList))
                {
                    while (alertList.Count > 0)
                    {
                        CloseUI(alertList[0]);
                    }
                }
            }

            PopStack(vo);

            vo.State = 0;

            UIViewBase view = _cachedViews[vo];
            view.gameObject.SetActive(false);
            view.Close();
        }

        private void HideUI(UIConfig vo)
        {
            if (vo.PopUpType == LayerType.Panel)
            {
                List<UIConfig> alertList;
                if(_alertListOfPanelDic.TryGetValue(vo,out alertList))
                {
                    for (int i = 0; i < alertList.Count; i++)
                    {
                        HideUI(alertList[i]);
                    }
                }
            }

            vo.State = 0;

            UIViewBase view = _cachedViews[vo];
            view.gameObject.SetActive(false);
            view.Hide();
        }

        private void ShowUI(UIConfig vo)
        {
            vo.State = 1;

            UIViewBase view = _cachedViews[vo];
            view.gameObject.SetActive(true);
            view.Show();
            view.transform.SetAsLastSibling();

            if (vo.PopUpType == LayerType.Panel)
            {
                _currentPanelConfig = vo;

                List<UIConfig> alertList;
                if(_alertListOfPanelDic.TryGetValue(vo,out alertList))
                {
                    for (int i = 0; i < alertList.Count; i++)
                    {
                        ShowUI(alertList[i]);
                    }
                }
            }
        }

        private void SetUIState(UIConfig vo)
        {
            // 背景遮罩
            UIConfig needMaskAlert = null;
            if (vo.PopUpType == LayerType.Alert)
            {
                List<UIConfig> alertList;
                if(_alertListOfPanelDic.TryGetValue(_currentPanelConfig,out alertList))
                {
                    for (int i = alertList.Count-1; i >= 0; i--)
                    {
                        if (alertList[i].NeedMask)
                        {
                            needMaskAlert = alertList[i];
                            break;
                        }
                    }
                }
            }
            if (needMaskAlert != null)
            {
                UILayerManager.Instance.AddMaskForAlert(_cachedViews[needMaskAlert].transform);
                UILayerManager.Instance.AlertMask.gameObject.SetActive(true);
            }
            else
            {
                UILayerManager.Instance.AlertMask.gameObject.SetActive(false);
            }

            // 返回键
            UILayerManager.Instance.BackButtonLayer.gameObject.SetActive(vo.ShowBackBtn);
        }
    }
}

