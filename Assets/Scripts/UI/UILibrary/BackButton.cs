using UnityEngine;
using System.Collections;
using tx.ui;
using UnityEngine.UI;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/12/2018 11:56:11 AM
/// </summary>


public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetComponent<Button>().onClick.AddListener(Click);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
        UIManager.Instance.Back();
    }
}
