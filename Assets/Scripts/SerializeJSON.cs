using System;
using UnityEngine;

public class SerializeJSON : MonoBehaviour
{
    private string _nameJson = "note1.txt";
    private string _json;

    public void  LoadJsonFile()
    {
        _json = Resources.Load<TextAsset>("note1").text;
        // string url = "file://" + Directory.GetCurrentDirectory() + "\\" + _nameJson;
        // using (WWW www = new WWW(url))
        // {
        //     yield return www;
        //     _json = www.text;
        // }

        if (_json != "")
        {
            GetComponent<GameManager>().Json = JsonUtility.FromJson<JsonClass>(_json);
            GetComponent<GameManager>().JsonText = _json;
        }
        Debug.Log("Json загружен");
    }
    
    
    
}

[Serializable]
public class JsonClass
{
    public Building[] buildings;
    public string developmentprojectId;
    public string name;
    public string address;
    public string deadlineDate;
    public string districtName;
    public string[] realtyobjectTypes;
    public string description;
    public string developmentProjectClass;
    public TransportAccess[] transportAccess;
    public Offices[] offices;
    public string dvizh_id;
    public bool isMrPrivate;
    public string availableOpenList;
}

[Serializable]
public class TransportAccess
{
    public string metroStation;
    public float distance;
    public float tripTime;
}

[Serializable]
public class Offices
{
    public string id;
    public string name;
    public string phone;
    public string mobile;
    public string officeType;
    public string address;
    public WorkingHours workingHours;
}

[Serializable]
public class WorkingHours
{
    public string friday;
    public string monday;
    public string sunday;
    public string tuesday;
    public string saturday;
    public string thursday;
    public string wednesday;
}


[Serializable]
public class Building
{
    public Section[] sections;
    public string buildingId;
    public string developmentphase;
    public string name;
    public string address;
    public string deadline;
    public string marketingName;
    public string bankId;
    public int newNumberOfSections;
    public int floor;
}

[Serializable]
public class Section
{
    public RealtyObject[] realtyObjects;
    public string sectionId;
    public string buildingId;
    public string name;
    public int storeys;
    public int maxObjectsOnFloor;
}

[Serializable]
public class RealtyObject
{
    public string realtyobjectId;
    public string[] additionalFunction;
    public string name;
    public string developmentprojectId;
    public string buildingId;
    public string typicalrealtyobjectName;
    public string typicalrealtyobjectId;
    public string realtyobjecttype;
    public string realtyobjectsubtype;
    public string realtyobjecttypestatus;
    public int number;
    public string sectionId;
    public int sectionNumber;
    public int floor;
    public int roomQuantityName;
    public float area;
    public int amount;
    public float price;
    public float discountPercent;
    public float amountDiscount;
    public string decorationName;
    public LayoutUrls[] layoutUrls;
    public string direction;
    public string reservationType;
    public int numberOnFloor;
    public int numberOfBedrooms;
    public float ceilingHeightM;
    public string viewLiquidity;
    public string realtyEstateSize;
    public string realtyEstateFormat;
    public string viewFromWindowTypology;
    public float areaProject;
    public string areaBTI;
    public string numberBTI;
    public string createdOn;
    public string modifiedOn;
    public string queueName;
    public int queueNumber;
    public string[] space;
    public string availableOpenList;
    public string saleTermsDateTo;
    public string websiteLink;
    public string layoutType;

    public string GetTypeRoom()
    {
        if (roomQuantityName == 0) return "Студия";
        else if (roomQuantityName == 1) return "1-комнатная";
        else if (roomQuantityName == 2) return "2-комнатная";
        else if (roomQuantityName == 3) return "3-комнатная";
        else if (roomQuantityName >= 4) return roomQuantityName + "-комнатная";
        return "Студия";
    }
}

[Serializable]
public class LayoutUrls
{
    public string siteLink;
    public string layoutName;
}