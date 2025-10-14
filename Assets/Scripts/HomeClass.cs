using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Выбор квартала
//Выбор Башни
//Появляется попап информации о башне
//Кнопка на попапе переводит на Выбор Этажа
//Генплан этажа
//После выбора квартиры появляется попап с кнопкой Далее
//Кнопка Далее переводит на последншее окно выбора квартиры по параметрам

public class HomeClass : MonoBehaviour
{

    public Button b_BackMenu;
    public Button b_ChoseRoomParametrs; //Выбрать квартиру по параметрам
    public Button b_ChoseRoomParametrs2; //Выбрать квартиру по параметрам
    
    [Header("Подсветка")]
    public GameObject LightPanel;
    public Button b_LightDvor;
    public Button b_LightAll;
    public Button b_LightFitnes;
    public Button b_AllPlan;
    public Button b_SoldApartments;
    public Button b_Light3DTour;
    public Button b_LightVideo;
    public Sprite LightDvor_Active;
    public Sprite LightDvor_NotActive;
    public Sprite LightAll_Active;
    public Sprite LightAll_NotActive;
    public Sprite LightFitnes_Active;
    public Sprite LightFitnes_NotActive;

    [Header("Выбор квартала")]
    public GameObject KvartalPanel;
    public Button b_Indigo;
    
    [Header("Выбор корпуса")]
    public GameObject KorpusPanel;
    public Button b_Korpus1;
    public Button b_Korpus2;
    public Button b_Korpus3;
    public Button b_Korpus4;
    public Button b_Korpus5;
    public Button b_Korpus6;
    public Sprite Korpus1Render;
    public Sprite Korpus2Render;
    public Sprite Korpus3Render;
    public Sprite Korpus4Render;
    public Sprite Korpus5Render;
    public Sprite Korpus6Render;
    
    public GameObject DarkPanel;
    public GameObject PopapKorpus;
    public TMP_Text Korpus_NumberKorpus;
    public TMP_Text Korpus_StatusBilding;
    public TMP_Text Korpus_EndData;
    public TMP_Text PriceStudio;
    public TMP_Text PriceOne;
    public TMP_Text PriceTwo;
    public TMP_Text PriceThree;
    public TMP_Text PriceFour;
    public TMP_Text AreaStudio;
    public TMP_Text AreaOne;
    public TMP_Text AreaTwo;
    public TMP_Text AreaThree;
    public TMP_Text AreaFour;
    public Button b_Korpus_VibratFloor;

    [Header("Выбор этажа")] 
    public GameObject FloorPanel;
    public Button Floor_Back;
    public Button Floor_BackToMenu;
    public TMP_Text Floor_NumberKorpus;
    public TMP_Text Floor_EndData;
    public TMP_Text Floor_ContRoom;
    public Transform Floor_FloorParent;
    public GameObject Floor_FloorPrefab;
    public Image RenderImage;
    
    [Header("Выбор квартиры")]
    public GameObject RoomPanel;
    public Button b_Room_Back;
    public TMP_Text Room_NumberFloor;
    public TMP_Text Room_CountRoom;
    public Button Room_Down;
    public Button Room_Up;
    public Button Room_GoHomePanel;
    public GameObject Room_Dark;
    public GameObject Room_Popap;
    public TMP_Text Room_Price;
    public TMP_Text Room_Area;
    public TMP_Text Room_EndData;
    public Image Room_ImageHorizontal;
    public Image Room_ImageVertical;
    public Button Room_Button_ChoseRoom;
    public TMP_Text Room_Button_Text;
    public Button Room_Button_Next;

    [Header("Панель подсветки")] 
    public HP_PopapLight PopapLight;

    [Header("Видео панель")] 
    public HP_VideoPanel VideoPanel;

    [Header("LastPanel")] 
    public RP_LastPanel LastPanel;
    
    private UbManager _manager;
    private MySection _currentSection;
    private Sprite _currentRender;
    private int[] _mass = new int[100]; //Этажи считаем от 1 элеманта, 
    private List<ButtonFloorPrefab> _buttonFloorPrefabs = new List<ButtonFloorPrefab>();
    private RealtyObject _currentRealtyObject;
    private RealtyObject _room_CurrentRoom;
    private int _room_currentChoseRoom;
    private List<RealtyObject> _realtyObjects = new List<RealtyObject>();
    private RealtyObject _room_CurrentRealityObject;

    public void Init(UbManager manager)
    {
        _manager = manager;
        b_BackMenu.onClick.AddListener(OnBackMenu);
        b_ChoseRoomParametrs.onClick.AddListener(OnChoseRoomParametrs);
        b_ChoseRoomParametrs2.onClick.AddListener(OnChoseRoomParametrs2);
        b_Korpus_VibratFloor.onClick.AddListener(OnSelectFloor);
        Floor_Back.onClick.AddListener(OnFloor_Back);
        Floor_BackToMenu.onClick.AddListener(OnBackMenu);
        b_Room_Back.onClick.AddListener(Room_Back);
        Room_GoHomePanel.onClick.AddListener(Room_GoToMenu);
        Room_Down.onClick.AddListener(Room_OnDown);
        Room_Up.onClick.AddListener(Room_OnUp);
        Room_Button_ChoseRoom.onClick.AddListener(Room_ActivatePopap);
        Room_Button_Next.onClick.AddListener(Room_OnNext);
        b_LightDvor.onClick.AddListener(OnSwitchLightDvor);
        b_LightAll.onClick.AddListener(OnSwitchLightAll);
        b_LightFitnes.onClick.AddListener(OnSwitchLightFitnes);
        PopapLight.Init(_manager);
        b_AllPlan.onClick.AddListener(PopapLight.Open);
        b_SoldApartments.onClick.AddListener(OnSoldApartments);
        VideoPanel.Init(_manager);
        b_LightVideo.onClick.AddListener(VideoPanel.Open);
        LastPanel.Init(_manager);
        LastPanel.B_Back.onClick.AddListener(OnBackToFloor);
        LastPanel.B_GoToMenu.onClick.AddListener(Close);
        LastPanel.SetOffAll(false);
        b_Light3DTour.onClick.AddListener(Open3Dtour);

        b_Korpus1.gameObject.SetActive(false);
        b_Korpus2.gameObject.SetActive(false);
        b_Korpus3.gameObject.SetActive(false);
        b_Korpus4.gameObject.SetActive(false);
        b_Korpus5.gameObject.SetActive(false);
        b_Korpus6.gameObject.SetActive(false);
        foreach (var section in _manager.gameManager.MySections)
        {
            string[] split = section.Section.name.Split("-");
            switch (split[split.Length-1])
            {
                case "1":
                {
                    b_Korpus1.gameObject.SetActive(true);
                    b_Korpus1.onClick.AddListener(()=>OnKorpus(section, Korpus1Render));
                    break;
                }
                case "2":
                {
                    b_Korpus2.gameObject.SetActive(true);
                    b_Korpus2.onClick.AddListener(()=>OnKorpus(section, Korpus2Render));
                    break;
                }
                case "3":
                {
                    b_Korpus3.gameObject.SetActive(true);
                    b_Korpus3.onClick.AddListener(()=>OnKorpus(section, Korpus3Render));
                    break;
                }
                case "4":
                {
                    b_Korpus4.gameObject.SetActive(true);
                    b_Korpus4.onClick.AddListener(()=>OnKorpus(section, Korpus4Render));
                    break;
                }
                case "5":
                {
                    b_Korpus5.gameObject.SetActive(true);
                    b_Korpus5.onClick.AddListener(()=>OnKorpus(section, Korpus5Render));
                    break;
                }
                case "6":
                {
                    b_Korpus6.gameObject.SetActive(true);
                    b_Korpus6.onClick.AddListener(()=>OnKorpus(section, Korpus6Render));
                    break;
                }
                
            }
        }
        
        //Убираем Попап
        PopapKorpus.SetActive(false);
        b_Indigo.onClick.AddListener(OnSelectKorpus);
        
        for (int i = 0; i < _mass.Length; i++) //Создаём кнопки этажей
        {
            ButtonFloorPrefab obj = Instantiate(Floor_FloorPrefab, Floor_FloorParent).GetComponent<ButtonFloorPrefab>();
            obj.Init();
            _buttonFloorPrefabs.Add(obj);
        }
        
        OnSelectKvartal();
    }

    private void Open3Dtour()
    {
        Application.OpenURL("https://kuula.co/share/5WvZK/collection/7czHf?logo=0&info=1&fs=1&vr=0&sd=1&thumbs=1");
    }

    public void Open()
    {
        OffAll();
        gameObject.SetActive(true);
        KvartalPanel.SetActive(true);
        b_BackMenu.gameObject.SetActive(true);
        LightPanel.SetActive(true);
        OnSaveLight();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    #region 1_Kvartal

    private void OnSelectKvartal()
    {
        OffAll();
        KvartalPanel.SetActive(true);
        b_BackMenu.gameObject.SetActive(true);
        LightPanel.SetActive(true);
        
    }

    private void OnSwitchLightDvor()
    {
        if (b_LightDvor.image.sprite == LightDvor_NotActive)
        {
            b_LightDvor.image.sprite = LightDvor_Active;
            //TODO отправляем команду зажечь двор
            _manager.gameManager.UdpClient.AddMessage("0180050A00000000", "OnDvor"); //Включить NN - канал
            Debug.Log("Включили двор");
        }
        else
        {
            b_LightDvor.image.sprite = LightDvor_NotActive;
            //TODO отправляем команду выключить двор
            _manager.gameManager.UdpClient.AddMessage("0180051400000000", "OffDvor"); //Выключить NN - канал
            Debug.Log("Выключили двор");
        }
    }

    private void OnSwitchLightAll()
    {
        if (b_LightAll.image.sprite == LightAll_NotActive)
        {
            b_LightAll.image.sprite = LightAll_Active;
            _manager.gameManager.UdpClient.AddMessage("0064010000000000", "On Life"); //Лайф режим
        }
        else
        {
            b_LightAll.image.sprite = LightAll_NotActive;
            _manager.gameManager.SendMessageToServer.OffAll();
        }
    }

    private void OnSwitchLightFitnes()
    {
        if (b_LightFitnes.image.sprite == LightFitnes_NotActive)
        {
            b_LightFitnes.image.sprite = LightFitnes_Active;
            _manager.gameManager.UdpClient.AddMessage("0102040300000000", "OnSection 7"); //Включить 7 корпус
        }
        else
        {
            b_LightFitnes.image.sprite = LightFitnes_NotActive;
            _manager.gameManager.UdpClient.AddMessage("0102040000000000", "OffSection 7"); //Включить 7 корпус
        }
    }

    private void OnSaveLight()
    {
        if (b_LightAll.image.sprite == LightAll_NotActive)
        {
            _manager.gameManager.SendMessageToServer.OffAll();
        }
        else
        {
            _manager.gameManager.UdpClient.AddMessage("0064010000000000", "Life"); //Лайф режим
        }
        if (b_LightFitnes.image.sprite == LightFitnes_NotActive)
        {
            _manager.gameManager.UdpClient.AddMessage("0102040000000000", "OffSection 7"); //Включить 7 корпус
        }
        else
        {
            _manager.gameManager.UdpClient.AddMessage("0102040300000000", "OnSection 7"); //Включить 7 корпус
        }
        if (b_LightDvor.image.sprite == LightDvor_NotActive)
        {
            _manager.gameManager.UdpClient.AddMessage("0180051400000000", "OffDvor"); //Выключить NN - канал
        }
        else
        {
            _manager.gameManager.UdpClient.AddMessage("0180050A00000000", "OnDvor"); //Включить NN - канал
        }
    }

    private void OnSoldApartments()
    {
        //TODO зажигаем проданные квартиры
        _manager.gameManager.SendMessageToServer.OffAll();
        foreach (var realtyObject in _manager.gameManager.RealtyObjectsSold)
        {
            _manager.gameManager.SendMessageToServer.OnRoom(realtyObject);
        }
    }

    #endregion

    #region 2_Korpus

    private void OnSelectKorpus()
    {
        OffAll();
        KorpusPanel.SetActive(true);
        b_BackMenu.gameObject.SetActive(true);
        LightPanel.SetActive(true);
        OnOffKorpusButton(true);
    }

    private void OnOffKorpusButton(bool isTrue=true)
    {
        b_Korpus1.gameObject.SetActive(isTrue);
        b_Korpus2.gameObject.SetActive(isTrue);
        b_Korpus3.gameObject.SetActive(isTrue);
        b_Korpus4.gameObject.SetActive(isTrue);
        b_Korpus5.gameObject.SetActive(isTrue);
        b_Korpus6.gameObject.SetActive(isTrue);
    }

    private void OnKorpus(MySection section, Sprite render)
    {
        
        _currentSection = section;
        
        //Debug.Log(_currentSection.MinPrice[0] + " " + _currentSection.MinPrice[1] + " " + _currentSection.MinPrice[2] +
         //         " " + _currentSection.MinPrice[3] + " " + _currentSection.MinPrice[4]);
        
        _currentRender = render;
        PopapKorpus.SetActive(true); //Показываем попап
        DarkPanel.SetActive(true); //Показываем затемнение
        OnOffKorpusButton(false);
        Korpus_NumberKorpus.text = _currentSection.Number + " корпус"; //И наполняем его инфой
        //Korpus_StatusBilding.text = "";
        Korpus_EndData.text = _currentSection.EndData;
        if (section.ApartmentDictionary[0].Count != 0)
        {
            PriceStudio.text = "от " + GetShortPrice(_currentSection.MinPrice[0]) + " млн";
            AreaStudio.text = "от " + _currentSection.MinArea[0] + " м";
        }
        else
        {
            PriceStudio.text = "";
            AreaStudio.text = "";
        }

        if (section.ApartmentDictionary[1].Count != 0)
        {
            PriceOne.text = "от " + GetShortPrice(_currentSection.MinPrice[1]) + " млн";
            AreaOne.text = "от " + _currentSection.MinArea[1] + " м";
        }
        else
        {
            PriceOne.text = "";
            AreaOne.text = "";
        }

        if (section.ApartmentDictionary[2].Count != 0)
        {
            PriceTwo.text = "от " + GetShortPrice(_currentSection.MinPrice[2]) + " млн";
            AreaTwo.text = "от " + _currentSection.MinArea[2] + " м";
        }
        else
        {
            PriceTwo.text = "";
            AreaTwo.text = "";
        }

        if (section.ApartmentDictionary[3].Count != 0)
        {
            PriceThree.text = "от " + GetShortPrice(_currentSection.MinPrice[3]) + " млн";
            AreaThree.text = "от " + _currentSection.MinArea[3] + " м";
        }
        else
        {
            PriceThree.text = "";
            AreaThree.text = "";
        }

        if (section.ApartmentDictionary[4].Count != 0)
        {
            PriceFour.text = "от " + GetShortPrice(_currentSection.MinPrice[4]) + " млн";
            AreaFour.text = "от " + _currentSection.MinArea[4] + " м";
        }
        else
        {
            PriceFour.text = "";
            AreaFour.text = "";
        }
    }

    #endregion

    #region 3_Floor

    private void OnSelectFloor()
    {
        OffAll();
        FloorPanel.SetActive(true);
        //Заполняем данные о секторе

        Floor_ContRoom.text = _currentSection.ApartmentDictionary.Values.Count.ToString();
        Floor_EndData.text = _currentSection.EndData;
        Floor_NumberKorpus.text = _currentSection.Number + " Корпус";
        RenderImage.sprite = _currentRender;
        RenderImage.SetNativeSize();
        
        //Заполняем этажами блок
        for (int i = 0; i < _mass.Length; i++)
        {
            _mass[i] = 0;
        }
        foreach (var key in _currentSection.ApartmentDictionary) //Вычисляем все имеющиеся этажи
        {
            foreach (var apartment in key.Value)
            {
                _mass[apartment.Floor] = 1;
            }
        }

        foreach (var floorPrefab in _buttonFloorPrefabs)
        {
            floorPrefab.Deactivate();
        }
        
        for (int i = 0; i < _mass.Length; i++) //Создаём кнопки этажей
        {
            if (_mass[i] == 1)
            {
                _buttonFloorPrefabs[i].Activate(i);
                int k = i;
                _buttonFloorPrefabs[i].b_Click.onClick.AddListener(()=>OnClickFloor(k,_currentSection));
            }
        }

        _manager.gameManager.SendMessageToServer.OnSection(_currentSection.Number);
        StartCoroutine(UpdateUI());
    }

    private void OnClickFloor(int numberFloor, MySection section)
    {
        List<RealtyObject> realtyObjects = new List<RealtyObject>();
        foreach (var realtyObject in section.Section.realtyObjects)
        {
            //Debug.Log(realtyObject.floor+" " + numberFloor);
            if(realtyObject.floor==numberFloor && realtyObject.realtyobjecttypestatus!="Sold") realtyObjects.Add(realtyObject);
        }
        Room_Activate(realtyObjects);
    }

    IEnumerator UpdateUI()
    {
        var grid = Floor_FloorParent.GetComponent<RectTransform>();
        var top = grid.sizeDelta;
        grid.sizeDelta = Vector2.one;
        yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(0.1f);
        grid.sizeDelta = top;
    }

    private void OnFloor_Back()
    {
        OnSelectKorpus();
        //_manager.SendMessageToServer.OnSection(_currentSection.Number);
        _manager.gameManager.SendMessageToServer.OffAll();
        OnSaveLight();
    }

    #endregion

    #region Room

    private void Room_Activate(List<RealtyObject> realtyObjects)
    {
        _realtyObjects.Clear();
        _realtyObjects = new List<RealtyObject>(realtyObjects);
        _room_currentChoseRoom = 0;
        RoomPanel.SetActive(true);
        Room_Dark.SetActive(false);
        Room_Popap.SetActive(false);

        Room_NumberFloor.text = realtyObjects[0].floor+" этаж";
        Room_CountRoom.text = realtyObjects.Count+" квартир";
        
        _manager.gameManager.SendMessageToServer.OffAll();
        _manager.gameManager.SendMessageToServer.OnFloor(realtyObjects[0]);
        _room_CurrentRealityObject = realtyObjects[0];
        Room_ChangeRoom();
    }

    private void Room_ChangeRoom()
    {
        _room_CurrentRoom = _realtyObjects[_room_currentChoseRoom];
        Debug.Log(_room_CurrentRoom.number);
        Sprite sprite = Resources.Load<Sprite>("PlansFloor/" + _room_CurrentRoom.realtyobjectId);
        if (sprite.texture.width > sprite.texture.height)
        {
            Room_ImageHorizontal.gameObject.SetActive(true);
            Room_ImageHorizontal.sprite = sprite;
            Room_ImageHorizontal.SetNativeSize();
            Room_ImageVertical.gameObject.SetActive(false);
            Room_Button_ChoseRoom.transform.parent = Room_ImageHorizontal.transform;
            Vector2 vector2 = _manager.gameManager.SearchRoomClass.GetVector(_room_CurrentRoom.realtyobjectId);
            vector2.x -= sprite.texture.width/2;
            vector2.y -= sprite.texture.height/2;
            float ws = 2470f / sprite.texture.width;
            float hs = 1090f / sprite.texture.height;
            if (ws > hs) Room_ImageHorizontal.transform.localScale = Vector3.one * hs;
            else Room_ImageHorizontal.transform.localScale = Vector3.one * ws;
            Room_Button_ChoseRoom.GetComponent<RectTransform>().localPosition = vector2;
        }
        else
        {
            Room_ImageVertical.gameObject.SetActive(true);
            Room_ImageHorizontal.gameObject.SetActive(false);
            Room_ImageVertical.sprite = sprite;
            Room_ImageVertical.SetNativeSize();
            Room_Button_ChoseRoom.transform.parent = Room_ImageVertical.transform;
            Vector2 vector2 = _manager.gameManager.SearchRoomClass.GetVector(_room_CurrentRoom.realtyobjectId);
            
            vector2.x -= sprite.texture.width/2;
            vector2.y -= sprite.texture.height/2;
            float ws = 1090f / sprite.texture.width;
            float hs = 2470f / sprite.texture.height;
            if (ws > hs) Room_ImageVertical.transform.localScale = Vector3.one * hs;
            else Room_ImageVertical.transform.localScale = Vector3.one * ws;
            Room_Button_ChoseRoom.GetComponent<RectTransform>().localPosition = vector2;
        }
        
            

        if(_room_CurrentRoom.roomQuantityName==0)
            Room_Button_Text.text = "Студия, " + _room_CurrentRoom.area + " <sprite index=1>";
        else
        {
            Room_Button_Text.text =
                _room_CurrentRoom.roomQuantityName + "-комнатная, " + _room_CurrentRoom.area + " <sprite index=1>";
        }
    }

    private void Room_Back()
    {
        RoomPanel.SetActive(false);
        _manager.gameManager.SendMessageToServer.OffAll();
        if(_room_CurrentRealityObject!=null)
            _manager.gameManager.SendMessageToServer.OnSection(_currentSection.Number);
    }

    private void Room_GoToMenu()
    {
        OnBackMenu();
    }

    private void Room_OnUp()
    {
        _room_currentChoseRoom++;
        if (_room_currentChoseRoom >= _realtyObjects.Count)
            _room_currentChoseRoom = 0;
        Room_ChangeRoom();
    }

    private void Room_OnDown()
    {
        _room_currentChoseRoom--;
        if (_room_currentChoseRoom < 0)
            _room_currentChoseRoom = _realtyObjects.Count - 1;
        Room_ChangeRoom();
    }

    private void Room_ActivatePopap()
    {
        Room_Popap.SetActive(true);
        Room_Dark.SetActive(true);
        
        Room_Price.text = _manager.gameManager.GetSplitPrice(_room_CurrentRoom.amount.ToString());
        
        if(_room_CurrentRoom.roomQuantityName==0)
            Room_Area.text = "Студия, " + _room_CurrentRoom.area + " <sprite index=1>";
        else
        {
            Room_Area.text =
                _room_CurrentRoom.roomQuantityName + "-комнатная, " + _room_CurrentRoom.area + " <sprite index=1>";
        }
        
        Room_EndData.text = _manager.gameManager.GetMySection(_room_CurrentRoom.sectionId).EndData;
    }

    private void Room_OnNext() //Переходим на последнюю страницу выбора квартир по параметрам
    {
        //gameObject.SetActive(false);
        LastPanel.OnOpenLastPanel(_room_CurrentRoom);
    }

    private void OnBackToFloor()
    {
        Room_Dark.SetActive(false);
        Room_Popap.SetActive(false);
    }

    #endregion

    private void OnBackMenu()
    {
        gameObject.SetActive(false);
        _manager.gameManager.SendMessageToServer.OffAll();
    }

    private void OnChoseRoomParametrs()
    {
        Close();
        _manager.ParametrsRoom.Open();
    }
    
    private void OnChoseRoomParametrs2()
    {
        Close();
        _manager.ParametrsRoom.Open();
    }

    private void OnSelectRoom()
    {
        OffAll();
        RoomPanel.SetActive(true);
    }

    private void OffAll()
    {
        KvartalPanel.SetActive(false);
        KorpusPanel.SetActive(false);
        b_BackMenu.gameObject.SetActive(false);
        LightPanel.SetActive(false);
        DarkPanel.SetActive(false);
        PopapKorpus.SetActive(false);
        FloorPanel.SetActive(false);
        RoomPanel.SetActive(false);
    }

    #region Support

    private string GetNumberKorpus(string nameKorpus)
    {
        string[] strings = nameKorpus.Split("-");
        return strings[strings.Length-1];
    }

    private string GetShortPrice(float price)
    {
        //Debug.Log(price);
        if (price <= 1) return "0";
        string str = "";
        if (price < 10000001f)
        {
            str = ((int) price).ToString().Substring(0, 1) + "," +
                  ((int) price).ToString().Substring(1, 1);
            return str;
        }
        
        str = ((int) price).ToString().Substring(0, 2) + "," +
            ((int) price).ToString().Substring(2, 1);
        return str;
    }

    #endregion
    

}
