using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParametrsRoomsClass : MonoBehaviour
{

    public enum TypeRoom
    {
        Note,
        Studio,
        One,
        Two,
        Three,
        Four
    }

    public int NumberUb;
    
    public Button b_BackMenu;
    public Button b_Studio;
    public Button b_One;
    public Button b_Two;
    public Button b_Three;
    public Button b_Four;
    public Button b_Use;
    public Button b_Reset;
    
    public Button b_Korpus_1;
    public Button b_Korpus_2;
    public Button b_Korpus_3;
    public Button b_Korpus_4;
    public Button b_Korpus_5;
    public Button b_Korpus_6;

    public TMP_Dropdown SortDrop;

    public TMP_Text PriceOt;
    public TMP_Text PriceDo;
    public TMP_Text PloshadOt;
    public TMP_Text PloshadDo;
    public DubleSlider PriceSlider;
    public DubleSlider PloshadSlider;

    public Color ActiveColor;

    public GameObject RoomPrefab;
    public Transform ContentParent;

    public RP_LastPanel LastPanelClass;

    private TypeRoom _typeRoom;
    private UbManager _ubManager;
    private float _deltaPrice;
    private float _deltaPloshad;
    private ListRooms _currentListRoom;
    private List<PlaneButtonPrefab> _prefabs = new List<PlaneButtonPrefab>();
    private float _minPrice;
    private float _maxPrice;
    private float _minPloshad;
    private float _maxPloshad;
    private RealtyObject _currentRealtyObject;
    private Coroutine _loadCoroutine;
    private int _currentKorpus;
    
    public void Init(UbManager manager)
    {
        _ubManager = manager;
        b_BackMenu.onClick.AddListener(OnBackMenu);
        b_Studio.onClick.AddListener(OnStudio);
        b_One.onClick.AddListener(OnOne);
        b_Two.onClick.AddListener(OnTwo);
        b_Three.onClick.AddListener(OnThree);
        b_Four.onClick.AddListener(OnFour);
        b_Use.onClick.AddListener(OnUse);
        b_Reset.onClick.AddListener(OnReset);
        _typeRoom = TypeRoom.Note;
        PriceSlider.Action += OnValueChangePrice;
        PloshadSlider.Action += OnValueChangeArea;
        LastPanelClass.Init(manager);
        LastPanelClass.B_GoToMenu.onClick.AddListener(OnBackMenu);
        LastPanelClass.B_Back.onClick.AddListener(OnReset);
        
        b_Korpus_1.onClick.AddListener(OnKorpus1);
        b_Korpus_2.onClick.AddListener(OnKorpus2);
        b_Korpus_3.onClick.AddListener(OnKorpus3);
        b_Korpus_4.onClick.AddListener(OnKorpus4);
        b_Korpus_5.onClick.AddListener(OnKorpus5);
        b_Korpus_6.onClick.AddListener(OnKorpus6);
        SortDrop.onValueChanged.AddListener(OnSortDrop);
        
    }

    public void Open()
    {
        gameObject.SetActive(true);
        LastPanelClass.gameObject.SetActive(false);
        OnStudio();
        OnKorpus1();
        _currentKorpus = 1;
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
        LastPanelClass.gameObject.SetActive(false);
        _ubManager.gameManager.SendMessageToServer.OffAll();
    }

    private void OnBackMenu()
    {
        StopLoadPlans();
        OnReset();
        Close();
    }

    private void OnReset()
    {
        OnStudio();
        OnKorpus1();
        _ubManager.gameManager.SendMessageToServer.OffAll();
        for (int i = 0; i < _prefabs.Count; i++)
        {
            Destroy(_prefabs[i].gameObject);
        }
        _prefabs.Clear();
    }

    //Применить фильтр
    private void OnUse()
    {
        for (int i = 0; i < _prefabs.Count; i++)
        {
            Destroy(_prefabs[i].gameObject);
        }
        _prefabs.Clear();
        SortDrop.value = 0;
        if(_loadCoroutine!=null) StopCoroutine(_loadCoroutine);
        _loadCoroutine = StartCoroutine(UseCor());
    }

    IEnumerator UseCor()
    {
        foreach (var flat in _currentListRoom.Rooms)
        {
            //Debug.Log(_currentKorpus + " " + room.roomQuantityName + " " + room.sectionNumber + " "  + _ubManager.gameManager.GetUbName(room.buildingId));
            if (flat.NumberUB != NumberUb) continue;
            //Debug.Log(_currentKorpus + " " + room.roomQuantityName + " " + room.sectionNumber);
            if (flat.SectionNumber != _currentKorpus) continue;
            if (flat.Price > _minPrice - 1 && flat.Price < _maxPrice + 1)
            {
                if (flat.Area > _minPloshad - 0.1f && flat.Area < _maxPloshad + 0.1f)
                {
                    PlaneButtonPrefab obj = Instantiate(RoomPrefab, ContentParent).GetComponent<PlaneButtonPrefab>();
                    obj.Init(flat, _currentListRoom.EndData, _ubManager, this);
                    _prefabs.Add(obj);
                    _ubManager.gameManager.SendMessageToServer.OnRoom(flat);
                    //yield return new WaitForSeconds(0.1f);
                }
            }
        }

        yield return null;
    }

    public void StopLoadPlans()
    {
        if(_loadCoroutine!=null)
            StopCoroutine(_loadCoroutine);
    }

    private void OnStudio()
    {
        _typeRoom = TypeRoom.Studio;
        ClickTypeRoom(b_Studio.image);
        ActiveListRoom(_ubManager.gameManager.Studiya);
    }

    private void OnOne()
    {
        _typeRoom = TypeRoom.One;
        ClickTypeRoom(b_One.image);
        ActiveListRoom(_ubManager.gameManager.One);
    }

    private void OnTwo()
    {
        _typeRoom = TypeRoom.Two;
        ClickTypeRoom(b_Two.image);
        ActiveListRoom(_ubManager.gameManager.Two);
    }

    private void OnThree()
    {
        _typeRoom = TypeRoom.Three;
        ClickTypeRoom(b_Three.image);
        ActiveListRoom(_ubManager.gameManager.Three);
    }

    private void OnFour()
    {
        _typeRoom = TypeRoom.Four;
        ClickTypeRoom(b_Four.image);
        ActiveListRoom(_ubManager.gameManager.Four);
    }

    private void OnKorpus1()
    {
        _currentKorpus = 1;
        ClickNumberKorpus(b_Korpus_1.image);
    }
    
    private void OnKorpus2()
    {
        _currentKorpus = 2;
        ClickNumberKorpus(b_Korpus_2.image);
    }
    
    private void OnKorpus3()
    {
        _currentKorpus = 3;
        ClickNumberKorpus(b_Korpus_3.image);
    }
    
    private void OnKorpus4()
    {
        _currentKorpus = 4;
        ClickNumberKorpus(b_Korpus_4.image);
    }
    
    private void OnKorpus5()
    {
        _currentKorpus = 5;
        ClickNumberKorpus(b_Korpus_5.image);
    }
    
    private void OnKorpus6()
    {
        _currentKorpus = 6;
        ClickNumberKorpus(b_Korpus_6.image);
    }

    private void OnSortDrop(int value)
    {
        Debug.Log("OnSortDrop " + value);
        if(value==1) SortPrice();
        if(value==2) SortPrice(false);
        if(value==3) SortArea();
        if(value==4) SortArea(false);
    }

    private void SortPrice(bool valueUp=true)
    {
        List<PlaneButtonPrefab> apartaments = new List<PlaneButtonPrefab>(_prefabs);
        if (valueUp)
        {
            for (int i = 0; i < _prefabs.Count; i++)
            {
                PlaneButtonPrefab apartament = apartaments[0];
                
                foreach (var prefab in apartaments)
                {
                    if (apartament.myApartment.RealtyObject.amountDiscount > prefab.myApartment.RealtyObject.amountDiscount)
                    {
                        apartament = prefab;
                    }
                }

                apartaments.Remove(apartament);
                apartament.transform.SetSiblingIndex(i); //Меняем порядок в потомках
            }
        }
        else
        {
            for (int i = 0; i < _prefabs.Count; i++)
            {
                PlaneButtonPrefab apartament = apartaments[0];
                
                foreach (var prefab in apartaments)
                {
                    if (apartament.myApartment.RealtyObject.amountDiscount < prefab.myApartment.RealtyObject.amountDiscount)
                    {
                        apartament = prefab;
                    }
                }

                apartaments.Remove(apartament);
                apartament.transform.SetSiblingIndex(i);
            }
        }
    }

    private void SortArea(bool valueUp=true)
    {
        List<PlaneButtonPrefab> apartaments = new List<PlaneButtonPrefab>(_prefabs);
        if (valueUp)
        {
            for (int i = 0; i < _prefabs.Count; i++)
            {
                PlaneButtonPrefab apartament = apartaments[0];
                
                foreach (var prefab in apartaments)
                {
                    if (apartament.myApartment.Area > prefab.myApartment.Area)
                    {
                        apartament = prefab;
                    }
                }

                apartaments.Remove(apartament);
                apartament.transform.SetSiblingIndex(i); //Меняем порядок в потомках
            }
        }
        else
        {
            for (int i = 0; i < _prefabs.Count; i++)
            {
                PlaneButtonPrefab apartament = apartaments[0];
                
                foreach (var prefab in apartaments)
                {
                    if (apartament.myApartment.Area < prefab.myApartment.Area)
                    {
                        apartament = prefab;
                    }
                }

                apartaments.Remove(apartament);
                apartament.transform.SetSiblingIndex(i);
            }
        }
    }

    private void ActiveListRoom(ListRooms listRooms)
    {
        _currentListRoom = listRooms;
        if (((int) listRooms.PriceMin).ToString().Length > 3)
        {
            PriceOt.text = ((int) listRooms.PriceMin).ToString().Substring(0, 2) + "," +
                           ((int) listRooms.PriceMin).ToString().Substring(2, 1);
        }
        else
        {
            PriceOt.text = "0";
        }

        if (((int) listRooms.PriceMax).ToString().Length > 3)
        {
            PriceDo.text = ((int) listRooms.PriceMax).ToString().Substring(0, 2) + "," +
                           ((int) listRooms.PriceMax).ToString().Substring(2, 1);
        }
        else
        {
            PriceDo.text = "0";
        }

        PloshadOt.text = listRooms.PloshadMin.ToString();
        PloshadDo.text = listRooms.PloshadMax.ToString();
        PriceSlider.Init();
        PloshadSlider.Init();
        _deltaPrice = listRooms.PriceMax - listRooms.PriceMin;
        _deltaPloshad = listRooms.PloshadMax - listRooms.PloshadMin;
        _minPrice = listRooms.PriceMin;
        _minPloshad = listRooms.PloshadMin;
        _maxPrice = listRooms.PriceMax;
        _maxPloshad = listRooms.PloshadMax;
    }

    private void OnValueChangePrice()
    {
        _minPrice = _currentListRoom.PriceMin + PriceSlider.LeftSlider.value * _deltaPrice;
        _maxPrice= _currentListRoom.PriceMax - (1 - PriceSlider.RightSlider.value) * _deltaPrice;
        if (((int) _minPrice).ToString().Length > 3)
        {
            PriceOt.text = ((int) _minPrice).ToString().Substring(0, 2) + "," +
                           ((int) _minPrice).ToString().Substring(2, 1);
        }
        else
        {
            PriceOt.text = "0";
        }

        if (((int) _maxPrice).ToString().Length > 3)
        {
            PriceDo.text = ((int) _maxPrice).ToString().Substring(0, 2) + "," +
                           ((int) _maxPrice).ToString().Substring(2, 1);
        }
        else
        {
            PriceDo.text = "0";
        }

        
    }

    private void OnValueChangeArea()
    {
        _minPloshad = _currentListRoom.PloshadMin + PloshadSlider.LeftSlider.value * _deltaPloshad;
        _maxPloshad = _currentListRoom.PloshadMax - (1 - PloshadSlider.RightSlider.value) * _deltaPloshad;
        float deltaMin = _minPloshad - (int) _minPloshad;
        float deltaMax = _maxPloshad - (int) _maxPloshad;
        PloshadOt.text = ((int)_minPloshad).ToString() + deltaMin.ToString().Substring(1,2);
        PloshadDo.text = ((int)_maxPloshad).ToString() + deltaMax.ToString().Substring(1,2);
    }

    private void ClickTypeRoom(Image image)
    {
        b_Four.image.color = Color.white;
        b_One.image.color = Color.white;
        b_Studio.image.color = Color.white;
        b_Three.image.color = Color.white;
        b_Two.image.color = Color.white;
        image.color = ActiveColor;
    }
    
    private void ClickNumberKorpus(Image image)
    {
        b_Korpus_1.image.color = Color.white;
        b_Korpus_2.image.color = Color.white;
        b_Korpus_3.image.color = Color.white;
        b_Korpus_4.image.color = Color.white;
        b_Korpus_5.image.color = Color.white;
        b_Korpus_6.image.color = Color.white;
        image.color = ActiveColor;
    }

}
