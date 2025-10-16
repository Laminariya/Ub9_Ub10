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

    [HideInInspector] public MyApartment myApartment;
    private UbManager _manager;
    private ParametrsRoomsClass _parametrsRoomsClass;

    public void Init(MyApartment flat, string endData, UbManager manager, ParametrsRoomsClass parametrsRoomsClass)
    {
        _parametrsRoomsClass = parametrsRoomsClass;
        _manager = manager;
        myApartment = flat;
        B_Click.onClick.AddListener(() => _parametrsRoomsClass.LastPanelClass.OnOpenLastPanel(myApartment));
        B_Click.onClick.AddListener(_parametrsRoomsClass.StopLoadPlans);
        EndDate.text = _manager.gameManager.GetEndData(endData);
        NumberFloor.text = "|  " + flat.Floor + " этаж  |";
        PriceOneMeter.text = flat.PriceMeter + " <sprite index=2>";
        NameAppartment.text = flat.GetTypeRoom() + ", " + flat.Area + " <sprite index=1>";
        Price.text = GetSplitPrice(flat.Price.ToString());
        Plane.sprite = LoadPlane(flat.RealtyObject.realtyobjectId);
    }

    private Sprite LoadPlane(string id)
    {
        Sprite sprite;
        if (myApartment.NumberUB == 9)
            sprite = Resources.Load<Sprite>("PlansRoom/" + id);
        else
            sprite = Resources.Load<Sprite>("PlansRoom10/" + id);
        // if (sprite == null)
        // {
        //     //Загружаем из папки на харде.
        //     //Примерно так: yield return StartCoroutine(_manager.CreateImagePng.LoadPNG(_realtyObject.realtyobjectId, false, Plane));
        // }
        
        return sprite;
    }

    private string GetSplitPrice(string str)
    {
        if (str.Length < 5) return str;
        string price = str.Insert(str.Length-3, " ");
        price = price.Insert(price.Length - 7, " ");
        price += " <sprite index=0>";
        return price;
    }

    private void OnDestroy()
    {
        B_Click.onClick.RemoveListener(() => _parametrsRoomsClass.LastPanelClass.OnOpenLastPanel(myApartment));
    }
}
