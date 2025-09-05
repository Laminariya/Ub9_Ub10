using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public const string Korpus1 = "Indigo  1 Ub9";
    public const string Korpus2 = "Indigo  2 Ub9";
    public const string Korpus3 = "Indigo  3 Ub9";
    public const string Korpus4 = "Indigo  4 Ub9";
    public const string Korpus5 = "Indigo  5 Ub9";
    public const string Korpus6 = "Indigo  6 Ub9";
    
    [HideInInspector] public List<string> KorpusName = new List<string>();
    
    [HideInInspector] public JsonClass Json;
    [HideInInspector] public string JsonText;
    
    public HomeClass HomePage;
    public KomplexClass Komplex;
    public ParametrsRoomsClass ParametrsRoom;
    public SearchRoomClass SearchRoomClass;
    public InputIpPanel InputIpPanel;
    public MyUdpClient UdpClient;
    public SendMessageToServer SendMessageToServer;

    public Button b_Home;
    public Button b_Komplex;
    public Button b_Rooms;
//https://s3.mastertel.ru/crm-content/Main/projects/VR/buildings/VR-Ub9/realEstates/VR-1-1-%D0%9A-5-14-4-514175/fp/VR-1-1-%D0%9A-5-14-4-514175.PNG
    private string _domen = "https://s3.mastertel.ru/";
    private string _planeFloor = "//Planer//PlansFloor//";
    private string _planeRoom = "//Planer//PlansRoom//";

    private LoadJsonClass _loadJsonClass;
    [HideInInspector] public CreateImagePNG CreateImagePng;
    private SerializeJSON _serializeJson;

    public Image TestImage;
    public List<Sprite> _sprites = new List<Sprite>();

    public ListRooms Studiya = new ListRooms();
    public ListRooms One = new ListRooms();
    public ListRooms Two = new ListRooms();
    public ListRooms Three = new ListRooms();
    public ListRooms Four = new ListRooms();

    public List<MySection> MySections = new List<MySection>();
    public List<RealtyObject> RealtyObjectsFree = new List<RealtyObject>();
    public List<RealtyObject> RealtyObjectsSold = new List<RealtyObject>();

    void Start()
    {
        b_Home.onClick.AddListener(OpenHome);
        b_Komplex.onClick.AddListener(OpenKomplex);
        b_Rooms.onClick.AddListener(OpenRooms);
        _loadJsonClass = GetComponent<LoadJsonClass>();
        _loadJsonClass.Init(this);
        CreateImagePng = GetComponent<CreateImagePNG>();
        _serializeJson = GetComponent<SerializeJSON>();
        
        KorpusName.Add(Korpus1);
        KorpusName.Add(Korpus2);
        KorpusName.Add(Korpus3);
        KorpusName.Add(Korpus4);
        KorpusName.Add(Korpus5);
        KorpusName.Add(Korpus6);
        
        StartCor();
    }

    private async Task StartCor()
    {
        //Грузим джейсон с сервака
        
        await _loadJsonClass.PostZapros(); //Должны получить стрингу
        //Делаем класс джесона
        if (JsonText != "")
            Json = JsonUtility.FromJson<JsonClass>(JsonText);
        //Если получается класс из стринги, то меняем текущие префсы на эту стрингу
        if (JsonText != "")
        {
            PlayerPrefs.SetString("json", JsonText);
            Debug.Log("Save json");
        }
        else  //Если нет, то берём из префсов, нет префсов, берём из текстового файла и кидаем в префсы
        {
            if (PlayerPrefs.HasKey("json"))
            {
                JsonText = PlayerPrefs.GetString("json");
                Debug.Log("Load json prefs");
            }
            else
            {
                JsonText = Resources.Load<TextAsset>("note1").text; //Загрузка сохранённого джесона
                PlayerPrefs.SetString("json", JsonText);
                Debug.Log("Load json txt file");
            }
            if (JsonText != "")
            {
                Json = JsonUtility.FromJson<JsonClass>(JsonText);
            } 
        }
        Debug.Log("111");
        CreateListFreeSoldRoom(); //Убирает из основного джесона проданные квартиры
        Debug.Log("222");
        CreateListRooms();
        Debug.Log("333");
        CreateMySection();
        Debug.Log("444");
        
        
        SendMessageToServer.Init(this);
        UdpClient.Init(this);
        ParametrsRoom.Init(this);
        Komplex.Init(this);
        HomePage.Init(this);
        CreateImagePng.Init(this);
        SearchRoomClass.Init(this);
        InputIpPanel.Init(this);
        
        ClosePanels();

        //yield return StartCoroutine(LoadAllPlane());
        //yield return StartCoroutine(ResizeImage());
        //Проверяем изменился ли джейсон
        //Если изменился, то меняем его и начинаем проверять есть ли новые схемы и загружаем их.
        
    }

    private void CheckNewPlans()
    {
        foreach (var building in Json.buildings)
        {
            foreach (var section in building.sections)
            {
                foreach (var realtyObject in section.realtyObjects)
                {
                    var sprite = Resources.Load<Sprite>("PlansRoom/" + realtyObject.realtyobjectId);
                    if(sprite==null) Debug.Log("PlansRoom " + realtyObject.name); //PlansFloor
                    var sprite2 = Resources.Load<Sprite>("PlansRoom/" + realtyObject.realtyobjectId);
                    if(sprite2==null) Debug.Log("PlansFloor " + realtyObject.name); //PlansFloor
                }
            }
        }
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

    private void ClosePanels()
    {
        HomePage.Close();
        Komplex.Close();
        ParametrsRoom.Close();
    }

    IEnumerator ResizeImage()
    {
        foreach (var building in Json.buildings)
        {
            foreach (var section in building.sections)
            {
                foreach (var realtyObject in section.realtyObjects)
                {
                    CreateImagePng.Resize(realtyObject.realtyobjectId, true);
                    yield return null;
                    CreateImagePng.Resize(realtyObject.realtyobjectId, false);
                    yield return null;
                    Debug.Log("Resize");
                }
            }
        }
    }

    IEnumerator LoadAllPlane()
    {
        foreach (var building in Json.buildings)
        {
            int count = 0;
            foreach (var section in building.sections)
            {
                count += section.realtyObjects.Length;
            }
            Debug.Log("All " + count);
            foreach (var section in building.sections)
            {
                Debug.Log("Start Section " + section.realtyObjects.Length);
                foreach (var realtyObject in section.realtyObjects)
                {
                    foreach (var layoutUrl in realtyObject.layoutUrls)
                    {
                        if (layoutUrl.layoutName == "LayoutForTheContract")
                        {
                            yield return StartCoroutine(CreateImagePng.LoadFileFromUrl(
                                _domen + layoutUrl.siteLink,
                                Directory.GetCurrentDirectory() + _planeFloor + realtyObject.realtyobjectId));
                        }
                        
                        if (layoutUrl.layoutName == "FunctionalLayout")
                        {
                            yield return StartCoroutine(CreateImagePng.LoadFileFromUrl(
                                _domen + layoutUrl.siteLink,
                                Directory.GetCurrentDirectory() + _planeRoom + realtyObject.realtyobjectId));
                        }
                    }
                }
            }
        } 
    }

    private void CreateListRooms()
    {
        foreach (var building in Json.buildings)
        {
            bool isTrueName = false;
            foreach (var korpus in KorpusName)
            {
                if (building.name == korpus) isTrueName = true;
            }
            if(!isTrueName) continue;
            foreach (var section in building.sections)
            {
                foreach (var realtyObject in section.realtyObjects)
                {
                    if(realtyObject.realtyobjecttypestatus=="Sold" || realtyObject.direction=="Commercial") continue;
                    if(realtyObject.roomQuantityName==0) Studiya.Rooms.Add(realtyObject);
                    else if(realtyObject.roomQuantityName==1) One.Rooms.Add(realtyObject);
                    else if(realtyObject.roomQuantityName==2) Two.Rooms.Add(realtyObject);
                    else if(realtyObject.roomQuantityName==3) Three.Rooms.Add(realtyObject);
                    else Four.Rooms.Add(realtyObject);
                }
            }
            Studiya.EndData = building.deadline;
            One.EndData = building.deadline;
            Two.EndData = building.deadline;
            Three.EndData = building.deadline;
            Four.EndData = building.deadline;
        }
        Studiya.Init();
        One.Init();
        Two.Init();
        Three.Init();
        Four.Init();
    }

    private void CreateMySection()
    {
        foreach (var building in Json.buildings)
        {
            bool isTrueName = false;
            foreach (var korpus in KorpusName)
            {
                if (building.name == korpus) isTrueName = true;
            }
            if(!isTrueName) continue;
            foreach (var section in building.sections)
            {
                MySection mySection = new MySection(section, GetEndData(building.deadline));
                MySections.Add(mySection);
            }
            //Debug.Log(building.name);
        }
    }

    IEnumerator LoadJson(string url)
    {
        var client = new WebClient();
        client.Encoding = Encoding.UTF8;
        var text = client.DownloadString(url);

        yield return null;
    }

    private void LoadText(string url)
    {
        var client = new WebClient();
        client.Encoding = Encoding.UTF8;
        var text = client.DownloadString(url);
        Debug.Log(text);
    }
    
    public string GetEndData(string data)
    {
        if (data.Length < 7) return " ";
        string month = data.Substring(5, 2);
        switch (month)
        {
            case "01":
            {
                month = "I";
                break;
            }
            case "02":
            {
                month = "I";
                break;
            }
            case "03":
            {
                month = "I";
                break;
            }
            case "04":
            {
                month = "II";
                break;
            }
            case "05":
            {
                month = "II";
                break;
            }
            case "06":
            {
                month = "II";
                break;
            }
            case "07":
            {
                month = "III";
                break;
            }
            case "08":
            {
                month = "III";
                break;
            }
            case "09":
            {
                month = "III";
                break;
            }
            case "10":
            {
                month = "IV";
                break;
            }
            case "11":
            {
                month = "IV";
                break;
            }
            case "12":
            {
                month = "IV";
                break;
            }
            default:
            {
                month = "";
                break;
            }
        }

        month += " " + data.Substring(0,4);
        return month;
    }
    
    public string GetMarketingName(string buildingId)
    {
        foreach (var building in Json.buildings)
        {
            if (building.buildingId == buildingId) return building.marketingName;
        }

        return "";
    }

    public MySection GetMySection(string sectionId)
    {
        foreach (var section in MySections)
        {
            if (section.Section.sectionId == sectionId)
                return section;
        }

        return null;
    }
    
    public string GetSplitPrice(string str)
    {
        string price = str.Insert(str.Length-3, " ");
        price = price.Insert(price.Length - 7, " ");
        price += " <sprite index=0>";
        return price;
    }

    private void CreateListFreeSoldRoom()
    {
        foreach (var building in Json.buildings)
        {
            bool isTrueName = false;
            foreach (var korpus in KorpusName)
            {
                if (building.name == korpus) isTrueName = true;
            }
            if(!isTrueName) continue;
            foreach (var section in building.sections)
            {
                foreach (var realtyObject in section.realtyObjects)
                {
                    if (realtyObject.direction == "Commercial") continue;
                    if (realtyObject.realtyobjecttypestatus != "Sold")
                    {
                        RealtyObjectsFree.Add(realtyObject);
                    }
                    else
                    {
                        RealtyObjectsSold.Add(realtyObject);
                        //Debug.Log(realtyObject.number + " " + realtyObject.floor + " " + realtyObject.name);
                    }
                }
            }
        }

        Debug.Log("Free "+ RealtyObjectsFree.Count);
        Debug.Log("Sold "+RealtyObjectsSold.Count);
    }
}

[Serializable]
public class ListRooms
{
    public List<RealtyObject> Rooms = new List<RealtyObject>();
    public float PriceMin;
    public float PriceMax;
    public float PloshadMin;
    public float PloshadMax;
    public string EndData;


    public void Init()
    {
        PriceMin = 1000000000f;
        PloshadMin = 10000000f;
        foreach (var room in Rooms)
        {
            if(room.amount==0) Debug.Log(room.buildingId + " "  + room.number + " " + room.name + " " + room.direction);
            if (PriceMin > room.amount) PriceMin = room.amount;
            if (PriceMax < room.amount) PriceMax = room.amount;
            if (PloshadMin > room.area) PloshadMin = room.area;
            if (PloshadMax < room.area) PloshadMax = room.area;
        }
        Debug.Log((int)PriceMin + " " + (int)PriceMax + "//" + PloshadMin + " " + PloshadMax);
    }
}

[Serializable]
public class MySection
{
    public Section Section;
    public Dictionary<int, List<MyApartment>> ApartmentDictionary = new Dictionary<int, List<MyApartment>>();
    public Dictionary<int, float> MinPrice = new Dictionary<int, float>();
    public Dictionary<int, float> MaxPrice = new Dictionary<int, float>();
    public Dictionary<int, float> MinArea = new Dictionary<int, float>();
    public Dictionary<int, float> MaxArea = new Dictionary<int, float>();
    public string Number;

    public string EndData;

    private List<int> _countRoomQuantity = new List<int>();

    public MySection(Section section, string endData)
    {
        Section = section;
        
        _countRoomQuantity.Add(0);
        _countRoomQuantity.Add(1);
        _countRoomQuantity.Add(2);
        _countRoomQuantity.Add(3);
        _countRoomQuantity.Add(4);
       
        foreach (var roomQuantity in _countRoomQuantity)
        {
            ApartmentDictionary[roomQuantity] = new List<MyApartment>();
        }
        
        foreach (var realtyObject in Section.realtyObjects)
        {
            if(realtyObject.realtyobjecttypestatus=="Sold" || realtyObject.direction=="Commercial") continue;
            MyApartment apartment = new MyApartment(realtyObject);
            if (apartment.RoomQuantity >= 4)
                ApartmentDictionary[4].Add(apartment);
            else
                ApartmentDictionary[apartment.RoomQuantity].Add(apartment);
        }
        
        foreach (var roomQuantity in _countRoomQuantity)
        {
            if (ApartmentDictionary.ContainsKey(roomQuantity))
            {
                MinPrice[roomQuantity] = MinPriceApartment(roomQuantity);
                MaxPrice[roomQuantity] = MaxPriceApartment(roomQuantity);
                MinArea[roomQuantity] = MinAreaApartment(roomQuantity);
                MaxArea[roomQuantity] = MaxAreaApartment(roomQuantity);
            }
        }
        
        Number = GetNumberKorpus(Section.name);
        EndData = endData;
    }

    private float MinPriceApartment(int roomQuantity)
    {
        float minPrice = float.MaxValue;

        foreach (var apartment in ApartmentDictionary[roomQuantity])
        {
            if(apartment.Price<=1) Debug.Log(apartment.Price+ " " + apartment.RealtyObject.direction);
            if (apartment.Price < minPrice) minPrice = apartment.Price;
        }
        //Debug.Log("XX "+ minPrice);
        return minPrice;
    }
    
    private float MinAreaApartment(int roomQuantity)
    {
        float minArea = float.MaxValue;

        foreach (var apartment in ApartmentDictionary[roomQuantity])
        {
            if (apartment.Area < minArea) minArea = apartment.Area;
        }
        
        return minArea;
    }
    
    private float MaxPriceApartment(int roomQuantity)
    {
        float maxPrice = 0;

        foreach (var apartment in ApartmentDictionary[roomQuantity])
        {
            if (apartment.Price > maxPrice) maxPrice = apartment.Price;
        }
        
        return maxPrice;
    }
    
    private float MaxAreaApartment(int roomQuantity)
    {
        float maxArea = 0;

        foreach (var apartment in ApartmentDictionary[roomQuantity])
        {
            if (apartment.Area > maxArea) maxArea = apartment.Area;
        }
        
        return maxArea;
    }
    
    private string GetNumberKorpus(string nameKorpus)
    {
        string[] strings = nameKorpus.Split("-");
        return strings[strings.Length-1];
    }

}

public class MyApartment
{
    public RealtyObject RealtyObject;
    public float Area;
    public float Price;
    public float PriceMeter;
    public int Floor;
    public int RoomQuantity;

    public MyApartment(RealtyObject realtyObject)
    {
        RealtyObject = realtyObject;
        Area = RealtyObject.area;
        Price = RealtyObject.amount;
        PriceMeter = RealtyObject.price;
        Floor = RealtyObject.floor;
        RoomQuantity = RealtyObject.roomQuantityName;
    }

}