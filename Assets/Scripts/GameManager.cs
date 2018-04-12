using UnityEngine;
using System.Collections;
using tx.ui;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/12/2018 12:13:36 PM
/// </summary>


public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;


    }
    // Use this for initialization
    void Start () {
        Initialize();

        StartGame();
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    UIManager.Instance.OpenUI(UIConfigSet.MainView,null,false);
        //}
	}
    

    private void Initialize()
    {
        UILayerManager.Instance.Init();
        UIManager.Instance.Init();
    }

    public void StartGame()
    {

        UIManager.Instance.OpenUI(UIConfigSet.MainView, null, false);
    }
}
