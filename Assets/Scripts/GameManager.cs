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

    public bool IsLoadWeb;
    
    public const string Korpus1 = "Indigo  1 Ub9";
    public const string Korpus2 = "Indigo  2 Ub9";
    public const string Korpus3 = "Indigo  3 Ub9";
    public const string Korpus4 = "Indigo  4 Ub9";
    public const string Korpus5 = "Indigo  5 Ub9";
    public const string Korpus6 = "Indigo  6 Ub9";
    public const string Korpus1_10 = "1 Ub10";
    public const string Korpus2_10 = "2 Ub10";
    public const string Korpus3_10 = "3 Ub10";
    public const string Korpus5_10 = "5 Ub10";
    
    [HideInInspector] public List<string> KorpusName = new List<string>();
    
    [HideInInspector] public JsonClass Json;
    [HideInInspector] public string JsonText;
    
    public SearchRoomClass SearchRoomClass;
    public InputIpPanel InputIpPanel;
    public MyUdpClient UdpClient;
    public SendMessageToServer SendMessageToServer;
    
//https://s3.mastertel.ru/crm-content/Main/projects/VR/buildings/VR-Ub9/realEstates/VR-1-1-%D0%9A-5-14-4-514175/fp/VR-1-1-%D0%9A-5-14-4-514175.PNG
    private string _domen = "https://s3.mastertel.ru/";
    private string _planeFloor = "//Planer//PlansFloor//";
    private string _planeRoom = "//Planer//PlansRoom//";

    private LoadJsonClass _loadJsonClass;
    [HideInInspector] public CreateImagePNG CreateImagePng;
    private SerializeJSON _serializeJson;

    [HideInInspector] public ListRooms Studiya = new ListRooms();
    [HideInInspector] public ListRooms One = new ListRooms();
    [HideInInspector] public ListRooms Two = new ListRooms();
    [HideInInspector] public ListRooms Three = new ListRooms();
    [HideInInspector] public ListRooms Four = new ListRooms();

    [HideInInspector] public List<MySection> MySections = new List<MySection>();
    [HideInInspector] public List<MyApartment> RealtyObjectsFree = new List<MyApartment>();
    [HideInInspector] public List<MyApartment> RealtyObjectsSold = new List<MyApartment>();

    public UbManager UbManager9;
    public UbManager UbManager10;
    
    public List<int> Floors1 = new List<int>();
    public List<int> Floors2 = new List<int>();
    public List<int> Floors3 = new List<int>();
    public List<int> Floors5 = new List<int>();

    void Start()
    {
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
        KorpusName.Add(Korpus1_10);
        KorpusName.Add(Korpus2_10);
        KorpusName.Add(Korpus3_10);
        KorpusName.Add(Korpus5_10);
        
        StartCor();
    }

    private async Task StartCor()
    {
        //Грузим джейсон с сервака
        if (IsLoadWeb)
        {
            await _loadJsonClass.PostZapros(); //Должны получить стрингу
        }
        else
        {
            _serializeJson.LoadJsonFile(); //Загружает с ресурсов
        }
        
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
        CreateMySection();
        //Debug.Log("111");
        //CreateListFreeSoldRoom(); //Убирает из основного джесона проданные квартиры
        //Debug.Log("222");
        CreateListRooms();
        //Debug.Log("333");


        SendMessageToServer.Init(this);//Один
        UdpClient.Init(this);          //Один
        UbManager10.Init(this);
        UbManager9.Init(this);
        CreateImagePng.Init(this);     //Один
        SearchRoomClass.Init(this);
        InputIpPanel.Init(this);       //Один
        
        OpenChoseComplex();

        //StartCoroutine(LoadAllPlane());
        //yield return StartCoroutine(ResizeImage());
        //Проверяем изменился ли джейсон
        //Если изменился, то меняем его и начинаем проверять есть ли новые схемы и загружаем их.

        foreach (var mySection in MySections)
        {
            //Debug.Log(mySection.NumberUB + " " + mySection.Number + " " + mySection.Section.realtyObjects.Length);
           if(mySection.NumberUB!=10) continue;
           if(mySection.Number!=3) continue;
                foreach (var realtyObject in mySection.MyApartments)
                {
                   
                   Debug.Log(mySection.NumberUB + " " + realtyObject.RealtyObject.floor);
                    

                    //Debug.Log(realtyObject.amount);
                }
                Debug.Log(mySection.MyApartments.Count);
            
        }
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

    public void OpenChoseComplex()
    {
        UbManager9.Hide();
        UbManager10.Hide();
    }
    
    public void OpenUb9()
    {
        UbManager9.Show();
    }

    public void OpenUb10()
    {
        UbManager10.Show();
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

    IEnumerator LoadAllPlane() //Загрузка всех схем к себе на диск
    {
        int count = 0;
        foreach (var section in MySections)
        {
            count += section.Section.realtyObjects.Length;
        }

        Debug.Log("All " + count);
        foreach (var section in MySections)
        {
            if (section.NumberUB == 9) continue;
            Debug.Log("Start Section " + section.Section.realtyObjects.Length);
            foreach (var realtyObject in section.Section.realtyObjects)
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

    private void CreateListRooms()
    {
        foreach (var section in MySections)
        {
            //Debug.Log(section.Number + " " + section.NumberUB);
            int count = 0;
            foreach (var realtyObject in section.MyApartments)
            {
                if (realtyObject.RealtyObject.realtyobjecttypestatus == "Sold" ||
                    realtyObject.RealtyObject.direction == "Commercial") continue;
                if (realtyObject.RoomQuantity == 0) Studiya.Rooms.Add(realtyObject);
                else if (realtyObject.RoomQuantity == 1) One.Rooms.Add(realtyObject);
                else if (realtyObject.RoomQuantity == 2) Two.Rooms.Add(realtyObject);
                else if (realtyObject.RoomQuantity == 3) Three.Rooms.Add(realtyObject);
                else Four.Rooms.Add(realtyObject);
                count++;
            }

            Studiya.EndData = section.EndData;
            One.EndData = section.EndData;
            Two.EndData = section.EndData;
            Three.EndData = section.EndData;
            Four.EndData = section.EndData;
            //Debug.Log(count);
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
                MySection mySection = new MySection(section, GetEndData(building.deadline), building.name);
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
    
    public int GetUbName(string buildingId)
    {
        foreach (var building in Json.buildings)    
        {
            if (building.buildingId == buildingId)
            {
                string[] split = building.name.Split(" ");
                if (split[split.Length - 1] == "Ub9") return 9;
                else return 10;
            }
        }

        return 9;
    }
    
    public string GetSplitPrice(string str)
    {
        //Debug.Log(str);
        if (str.Length < 5) return "0";
        string price = str.Insert(str.Length-3, " ");
        price = price.Insert(price.Length - 7, " ");
        price += " <sprite index=0>";
        return price;
    }

    private void CreateListFreeSoldRoom()
    {

        foreach (var section in MySections)
        {
            foreach (var apartment in section.MyApartments)
            {
                if (apartment.RealtyObject.direction == "Commercial") continue;
                if (apartment.RealtyObject.realtyobjecttypestatus != "Sold")
                {
                    RealtyObjectsFree.Add(apartment);
                }
                else
                {
                    RealtyObjectsSold.Add(apartment);
                    //Debug.Log(realtyObject.number + " " + realtyObject.floor + " " + realtyObject.name);
                }
            }
        }

        Debug.Log("Free " + RealtyObjectsFree.Count);
        Debug.Log("Sold " + RealtyObjectsSold.Count);
    }
}

[Serializable]
public class ListRooms
{
    public List<MyApartment> Rooms = new List<MyApartment>();
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
            //if(room.Price==0) Debug.Log(room.SectionNumber + " "  + room.Number + " " + room.RealtyObject.direction);
            if (PriceMin > room.Price) PriceMin = room.Price;
            if (PriceMax < room.Price) PriceMax = room.Price;
            if (PloshadMin > room.Area) PloshadMin = room.Area;
            if (PloshadMax < room.Area) PloshadMax = room.Area;
        }
        //Debug.Log((int)PriceMin + " " + (int)PriceMax + "//" + PloshadMin + " " + PloshadMax);
    }
}

[Serializable]
public class MySection
{
    public int NumberUB;
    public int Number;
    public Section Section;
    public string BuildingName;
    public List<MyApartment> MyApartments = new List<MyApartment>();
    public Dictionary<int, List<MyApartment>> ApartmentDictionary = new Dictionary<int, List<MyApartment>>();
    public Dictionary<int, float> MinPrice = new Dictionary<int, float>();
    public Dictionary<int, float> MaxPrice = new Dictionary<int, float>();
    public Dictionary<int, float> MinArea = new Dictionary<int, float>();
    public Dictionary<int, float> MaxArea = new Dictionary<int, float>();

    public string EndData;

    private List<int> _countRoomQuantity = new List<int>();

    public MySection(Section section, string endData, string buildingName)
    {
        Section = section;
        BuildingName = buildingName;
        Number = GetNumberKorpus(Section.name);
        NumberUB = GetNumberUB(buildingName);
        EndData = endData;
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
            if(realtyObject.realtyobjecttypestatus=="Sold" || realtyObject.direction=="Commercial" || realtyObject.realtyobjecttype=="Pantry") continue; // 
            int numberFlat = GetNumberFlat(Section, realtyObject);
            MyApartment apartment = new MyApartment(realtyObject, Number, NumberUB, numberFlat);
            MyApartments.Add(apartment);
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
        //Debug.Log("Ub"+NumberUB + " " + Number);
    }

    private int GetNumberFlat(Section section, RealtyObject flat)
    {
        
        return 0;
    }

    private float MinPriceApartment(int roomQuantity)
    {
        float minPrice = float.MaxValue;

        foreach (var apartment in ApartmentDictionary[roomQuantity])
        {
            //if(apartment.Price<=1) Debug.Log(apartment.Price+ " " + apartment.RealtyObject.direction);
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
    
    private int GetNumberKorpus(string nameKorpus)
    {
        string[] strings = nameKorpus.Split("-");
        int n = int.Parse(strings[strings.Length - 1][strings[strings.Length - 1].Length - 1].ToString());
        return n;
    }

    private int GetNumberUB(string nameUb)
    {
        string[] split = nameUb.Split(" ");
        if (split[split.Length - 1] == "Ub9") return 9;
        else return 10;
    }

}

public class MyApartment
{
    public RealtyObject RealtyObject;
    public int Number;
    public float Area;
    public int Price;
    public float PriceMeter;
    public int Floor;
    public int RoomQuantity;
    public int SectionNumber;
    public int NumberUB;

    public MyApartment(RealtyObject realtyObject, int sectionNumber, int numberUb, int numberFlat)
    {
        RealtyObject = realtyObject;
        if (numberUb == 9)
            Number = int.Parse(realtyObject.number.ToString().Substring(3, realtyObject.number.ToString().Length - 3));
        else
        {
            Number = numberFlat;
        }
        Area = RealtyObject.area;
        Price = RealtyObject.amount;
        PriceMeter = RealtyObject.price;
        Floor = RealtyObject.floor;
        RoomQuantity = RealtyObject.roomQuantityName;
        SectionNumber = sectionNumber;
        NumberUB = numberUb;
    }
    
    public string GetTypeRoom()
    {
        if (RoomQuantity == 0) return "Студия";
        else if (RoomQuantity == 1) return "1-комнатная";
        else if (RoomQuantity == 2) return "2-комнатная";
        else if (RoomQuantity == 3) return "3-комнатная";
        else if (RoomQuantity >= 4) return RoomQuantity + "-комнатная";
        return RoomQuantity.ToString();
    }

}