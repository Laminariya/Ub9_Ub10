using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UbManager : MonoBehaviour
{
    
    [HideInInspector] public HomeClass HomePage;
    [HideInInspector] public KomplexClass Komplex;
    [HideInInspector] public ParametrsRoomsClass ParametrsRoom;
    
    public Button b_Home;
    public Button b_Komplex;
    public Button b_Rooms;

    [HideInInspector] public GameManager gameManager;
    
    public void Init(GameManager gameManager)
    {
        Debug.Log("UbManager init");
        this.gameManager = gameManager;
        HomePage = GetComponentInChildren<HomeClass>(true);
        Komplex = GetComponentInChildren<KomplexClass>(true);
        ParametrsRoom = GetComponentInChildren<ParametrsRoomsClass>(true);
        HomePage.Init(this);
        Komplex.Init(this);
        ParametrsRoom.Init(this);
        b_Home.onClick.AddListener(OpenHome);
        b_Komplex.onClick.AddListener(OpenKomplex);
        b_Rooms.onClick.AddListener(OpenRooms);
    }

    private void OpenHome()
    {
        HomePage.Open();
    }

    private void OpenKomplex()
    {
        Komplex.Open();
    }

    private void OpenRooms()
    {
        ParametrsRoom.Open();
    }

    public void Show()
    {
        HomePage.Close();
        Komplex.Close();
        ParametrsRoom.Close();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        HomePage.Close();
        Komplex.Close();
        ParametrsRoom.Close();
        gameObject.SetActive(false);
    }

}
