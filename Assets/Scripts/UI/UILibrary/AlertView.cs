using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using tx.ui;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/12/2018 2:59:29 PM
/// </summary>


public class AlertView : UIViewBase
{
    public Button bagBtn;
    public Button shopBtn;
    public Button friendBtn;

    private void Awake()
    {
        bagBtn = transform.FindChild("Panel/BagBtn").GetComponent<Button>();
        shopBtn = transform.FindChild("Panel/ShopBtn").GetComponent<Button>();
        friendBtn = transform.FindChild("Panel/FriendBtn").GetComponent<Button>();

        bagBtn.onClick.AddListener(OnBagBtnClick);
        shopBtn.onClick.AddListener(OnShopBtnClick);
        friendBtn.onClick.AddListener(OnFriendBtnClick);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Open(UIModelBase model)
    {
        base.Open(model);
    }

    public override void ReTop()
    {
        base.ReTop();
        //txDebug.Log(gameObject.name + "  重聚焦");
    }

    private void OnBagBtnClick()
    {
        UIManager.Instance.OpenUI(UIConfigSet.BagView, null);
    }
    private void OnShopBtnClick()
    {
        UIManager.Instance.OpenUI(UIConfigSet.ShopView, null);
    }
    private void OnFriendBtnClick()
    {
        UIManager.Instance.OpenUI(UIConfigSet.FriendView, null);
    }
}
