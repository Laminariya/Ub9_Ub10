using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SearchRoomClass : MonoBehaviour
{
    public Color ColorGrey;
    private GameManager _manager;
    private const string _key = "SearchFloors";
    [HideInInspector] public FloorsImage FloorsImage;

    public void Init(GameManager manager)
    {
        _manager = manager;
        
        if (!PlayerPrefs.HasKey(_key))
        {
            //SearchPoints();
            PlayerPrefs.SetString(_key, Resources.Load<TextAsset>("floors").text);
            PlayerPrefs.Save();
            FloorsImage = LoadSearch();
        }
        else
        {
            FloorsImage = LoadSearch();
        }
    }

    private void SearchPoints()
    {
        Debug.Log("Start Search");
        FloorsImage = new FloorsImage();
        foreach (var building in _manager.Json.buildings)
        {
            foreach (var section in building.sections)
            {
                foreach (var realtyObject in section.realtyObjects)
                {
                    Vector2 vec1 = SearchPoint(realtyObject);

                    FloorPointImage floorPointImage = new FloorPointImage();
                    floorPointImage.Id = realtyObject.realtyobjectId;
                    floorPointImage.PointRoom = vec1;
                    FloorsImage.FloorPointImages.Add(floorPointImage);

                    //Debug.Log("Image Complete");
                }
            }
        }

        SaveSearch(FloorsImage);
    }

    private Vector2 SearchPoint(RealtyObject realtyObject)
    {
        Sprite sprite = Resources.Load<Sprite>("PlansFloor/" + realtyObject.realtyobjectId);
        Vector2 vec1 = Vector2.zero;
        int width = sprite.texture.width;
        int height = sprite.texture.height;
        var colors = sprite.texture.GetPixels();
        int count = 0;
        for (int i = 0; i < height; i += 10)
        {
            for (int j = 0; j < width; j += 10)
            {
                if (colors[width * i + j] == ColorGrey)
                {
                    vec1.x += j;
                    vec1.y += i;
                    count++;
                }
            }
        }

        vec1 /= count;
        return vec1;
    }

    private void SaveSearch(FloorsImage floorsImage)
    {
        //Debug.Log(JsonUtility.ToJson(floorsImage));
        PlayerPrefs.SetString(_key, JsonUtility.ToJson(floorsImage));
        PlayerPrefs.Save();
    }

    private FloorsImage LoadSearch()
    {
        if (!PlayerPrefs.HasKey(_key)) return null;
        string str = PlayerPrefs.GetString(_key);
        //WriteStr(str);

        FloorsImage floorsImage = JsonUtility.FromJson<FloorsImage>(str);
        return floorsImage;
    }

    // private async Task WriteStr(string str)
    // {
    //     using (StreamWriter writer = new StreamWriter("floors.txt", false))
    //     {
    //         Debug.Log("Start");
    //         await writer.WriteLineAsync(str);
    //         Debug.Log("End");
    //     }
    // }

    public Vector2 GetVector(string id)
    {
        foreach (var pointImage in FloorsImage.FloorPointImages)
        {
            if (pointImage.Id == id)
                return pointImage.PointRoom;
        }

        return Vector2.zero;
    }
}

[Serializable]
public class FloorsImage
{
    public List<FloorPointImage> FloorPointImages = new List<FloorPointImage>();
}

[Serializable]
public class FloorPointImage
{
    public string Id;
    public Vector2 PointRoom;
}