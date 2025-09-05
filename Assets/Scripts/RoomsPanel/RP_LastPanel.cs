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
    private GameManager _manager;
    private RealtyObject _currentRealtyObject;
    private Coroutine _coroutineLight;
    private bool _isOffAll;

    public void Init(GameManager manager)
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
        _manager.SendMessageToServer.OffAll();
    }

    public void OnOpenLastPanel(RealtyObject realtyObject)
    {
        gameObject.SetActive(true);

        roomSprite = Resources.Load<Sprite>("PlansRoom/" + realtyObject.realtyobjectId);
        floorSprite = Resources.Load<Sprite>("PlansFloor/" + realtyObject.realtyobjectId);
        OnPlaner();
        NameRoom.text = realtyObject.GetTypeRoom() + ", " + realtyObject.area + " <sprite index=1>";
        Price.text = _manager.GetSplitPrice(realtyObject.amount.ToString()) + " <sprite index=0>";
        Korpus.text = _manager.GetMarketingName(realtyObject.buildingId);
        Otdelka.text = realtyObject.decorationName;
        NumberFloor.text = realtyObject.floor.ToString();
        RoomNumber.text = "№" + realtyObject.number;

        _currentRealtyObject = realtyObject;
        
        //Подсветка

        OnLight(realtyObject);
    }

    private void OnLight(RealtyObject realtyObject)
    {
        _manager.UdpClient.ClearQueue();
        _manager.SendMessageToServer.OffAll();
        _manager.SendMessageToServer.StartRoomOnFloor(realtyObject);
    }

    private void OnBackLastPanel()
    {
        gameObject.SetActive(false);
        _manager.SendMessageToServer.OffAll();
        if(_currentRealtyObject!=null && !_isOffAll)
            _manager.SendMessageToServer.OnFloor(_currentRealtyObject);
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
