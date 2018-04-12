using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using tx.ui;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/12/2018 2:13:19 PM
/// </summary>


public class BagView : UIViewBase
{

    public Button bagBtn;
    public Button shopBtn;
    public Button friendBtn;
    public Button alertBtn1;
    public Button alertBtn2;
    public Button alertBtn3;


    private void Awake()
    {
        bagBtn = transform.FindChild("Panel/BagBtn").GetComponent<Button>();
        shopBtn = transform.FindChild("Panel/ShopBtn").GetComponent<Button>();
        friendBtn = transform.FindChild("Panel/FriendBtn").GetComponent<Button>();
        alertBtn1 = transform.FindChild("Panel/AlertBtn1").GetComponent<Button>();
        alertBtn2 = transform.FindChild("Panel/AlertBtn2").GetComponent<Button>();
        alertBtn3 = transform.FindChild("Panel/AlertBtn3").GetComponent<Button>();

        bagBtn.onClick.AddListener(OnBagBtnClick);
        shopBtn.onClick.AddListener(OnShopBtnClick);
        friendBtn.onClick.AddListener(OnFriendBtnClick);
        alertBtn1.onClick.AddListener(OnAlertBtn1Click);
        alertBtn2.onClick.AddListener(OnAlertBtn2Click);
        alertBtn3.onClick.AddListener(OnAlertBtn3Click);
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
    private void OnAlertBtn1Click()
    {
        UIManager.Instance.OpenUI(UIConfigSet.AlertView1, null);
    }
    private void OnAlertBtn2Click()
    {
        UIManager.Instance.OpenUI(UIConfigSet.AlertView2, null);
    }
    private void OnAlertBtn3Click()
    {
        UIManager.Instance.OpenUI(UIConfigSet.AlertView3, null);
    }
}
