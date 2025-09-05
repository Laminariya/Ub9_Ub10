using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaneButtonPrefab : MonoBehaviour
{
    public Button B_Click;
    public TMP_Text EndDate;
    public TMP_Text NumberFloor;
    public TMP_Text PriceOneMeter;
    public TMP_Text NameAppartment; //Название квартиры и площадь
    public TMP_Text Price;
    public Image Plane;

    [HideInInspector] public RealtyObject RealtyObject;
    private GameManager _manager;
    private ParametrsRoomsClass _parametrsRoomsClass;

    public void Init(RealtyObject realtyObject, string endData, GameManager manager, ParametrsRoomsClass parametrsRoomsClass)
    {
        _parametrsRoomsClass = parametrsRoomsClass;
        _manager = manager;
        RealtyObject = realtyObject;
        B_Click.onClick.AddListener(() => _parametrsRoomsClass.LastPanelClass.OnOpenLastPanel(RealtyObject));
        B_Click.onClick.AddListener(_parametrsRoomsClass.StopLoadPlans);
        EndDate.text = _manager.GetEndData(endData);
        NumberFloor.text = "|  " + realtyObject.floor + " этаж  |";
        PriceOneMeter.text = realtyObject.price + " <sprite index=2>";
        NameAppartment.text = realtyObject.GetTypeRoom() + ", " + realtyObject.area + " <sprite index=1>";
        Price.text = GetSplitPrice(realtyObject.amount.ToString());
        Plane.sprite = LoadPlane(RealtyObject.realtyobjectId);
    }

    private Sprite LoadPlane(string id)
    {
        Sprite sprite;
        sprite = Resources.Load<Sprite>("PlansRoom/" + id);
        // if (sprite == null)
        // {
        //     //Загружаем из папки на харде.
        //     //Примерно так: yield return StartCoroutine(_manager.CreateImagePng.LoadPNG(_realtyObject.realtyobjectId, false, Plane));
        // }
        
        return sprite;
    }

    private string GetSplitPrice(string str)
    {
        string price = str.Insert(str.Length-3, " ");
        price = price.Insert(price.Length - 7, " ");
        price += " <sprite index=0>";
        return price;
    }

    private void OnDestroy()
    {
        B_Click.onClick.RemoveListener(() => _parametrsRoomsClass.LastPanelClass.OnOpenLastPanel(RealtyObject));
    }
}
