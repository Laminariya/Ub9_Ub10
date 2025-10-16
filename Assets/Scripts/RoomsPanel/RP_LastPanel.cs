using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RP_LastPanel : MonoBehaviour
{
    
    public Button B_Planer;
    public Button B_PlanFloor;
    public Button B_Back;
    public Button B_GoToMenu;
    public Image I_Plan;
    public TMP_Text NameRoom;
    public TMP_Text Price;
    public TMP_Text Korpus;
    public TMP_Text Otdelka;
    public TMP_Text NumberFloor;
    public TMP_Text RoomNumber;
    public Sprite ActivePlaner;
    public Sprite NotActivePlaner;
    public Sprite ActiveFloor;
    public Sprite NotActiveFloor;
    
    private Sprite roomSprite;
    private Sprite floorSprite;
    private UbManager _manager;
    private MyApartment _myApartment;
    private Coroutine _coroutineLight;
    private bool _isOffAll;

    public void Init(UbManager manager)
    {
        _manager = manager;
        B_Planer.onClick.AddListener(OnPlaner);
        B_PlanFloor.onClick.AddListener(OnPlanerFloor);
        B_Back.onClick.AddListener(OnBackLastPanel);
        B_GoToMenu.onClick.AddListener(Close);
        gameObject.SetActive(false);
        _isOffAll = true;
    }

    public void SetOffAll(bool isOffAll)
    {
        _isOffAll = isOffAll;
    }

    private void Close()
    {
        gameObject.SetActive(false);
        _manager.gameManager.SendMessageToServer.OffAll();
    }

    public void OnOpenLastPanel(MyApartment realtyObject)
    {
        gameObject.SetActive(true);

        if(realtyObject.NumberUB==9)
        {
            roomSprite = Resources.Load<Sprite>("PlansRoom/" + realtyObject.RealtyObject.realtyobjectId);
            floorSprite = Resources.Load<Sprite>("PlansFloor/" + realtyObject.RealtyObject.realtyobjectId);
        }
        else
        {
            roomSprite = Resources.Load<Sprite>("PlansRoom10/" + realtyObject.RealtyObject.realtyobjectId);
            floorSprite = Resources.Load<Sprite>("PlansFloor10/" + realtyObject.RealtyObject.realtyobjectId);
        }
        
        OnPlaner();
        NameRoom.text = realtyObject.GetTypeRoom() + ", " + realtyObject.Area + " <sprite index=1>";
        Price.text = _manager.gameManager.GetSplitPrice(realtyObject.Price.ToString()) + " <sprite index=0>";
        Korpus.text = _manager.gameManager.GetMarketingName(realtyObject.RealtyObject.buildingId);
        if (realtyObject.RealtyObject.decorationName == "WithoutDecoration") Otdelka.text = "Без отделки";
        else Otdelka.text = realtyObject.RealtyObject.decorationName;
        NumberFloor.text = realtyObject.Floor.ToString();
        RoomNumber.text = "№" + realtyObject.Number + " " + realtyObject.RealtyObject.number;

        _myApartment = realtyObject;
        
        //Подсветка

        OnLight(realtyObject);
    }

    private void OnLight(MyApartment realtyObject)
    {
        _manager.gameManager.UdpClient.ClearQueue();
        _manager.gameManager.SendMessageToServer.OffAll();
        _manager.gameManager.SendMessageToServer.StartRoomOnFloor(realtyObject);
    }

    private void OnBackLastPanel()
    {
        gameObject.SetActive(false);
        _manager.gameManager.SendMessageToServer.OffAll();
        if(_myApartment!=null && !_isOffAll)
            _manager.gameManager.SendMessageToServer.OnFloor(_myApartment);
    }

    private void OnPlaner()
    {
        I_Plan.sprite = roomSprite;
        B_Planer.image.sprite = ActivePlaner;
        B_PlanFloor.image.sprite = NotActiveFloor;
    }

    private void OnPlanerFloor()
    {
        I_Plan.sprite = floorSprite;
        B_Planer.image.sprite = NotActivePlaner;
        B_PlanFloor.image.sprite = ActiveFloor;
    }

}
