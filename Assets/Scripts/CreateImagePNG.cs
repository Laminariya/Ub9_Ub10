using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Aspose.Imaging;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class CreateImagePNG : MonoBehaviour
{

    private string _domen = "https://s3.mastertel.ru/";
    private string _path = "crm-content/Main/projects/VR/buildings/VR-Ub9/realEstates/VR-1-1-%D0%9A-5-5-9-505054/ddu/VR-1-1-%D0%9A-5-5-9-505054.PNG";
    private string _nameFile = "C://Projects//PlanerRooms//Assets//Image.png";
    public Color ColorGray;
    public Image ImageTest;
    private string _planeFloor = "//Planer//PlansFloor//";
    private string _planeRoom = "//Planer//PlansRoom//";
    private string _convertFloor = "//Planer//ConvertFloor//";

    private GameManager _manager;
    private ImageResize _imageResize;
    private Aspose.Imaging.Image image;
    
    public void Init(GameManager manager)
    {
        _manager = manager;
        _imageResize = GetComponent<ImageResize>();
        //StartCoroutine(LoadFileFromUrl(_domen+_path));
        //StartCoroutine(LoadPNG(_nameFile));
    }

    //Загрузили с сервера картинку
    public IEnumerator LoadFileFromUrl(string path, string pathPlane) 
    {
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();
        // if (www.isHttpError || www.isNetworkError)
        // {
        //     Debug.Log("Error while Receiving: " + www.error);
        // }
        // else
        // {
            //_nameFile = Директория + папка + id квартиры
            Debug.Log("Изображение загружено");
            yield return StartCoroutine(CreatePNG(pathPlane, www.downloadHandler.data));
        //}
    }
    
    //Сохранили картинку у нас
    IEnumerator CreatePNG(string path, byte[] data)
    {
        // запись в файл
        using (FileStream fstream = new FileStream(path+".png", FileMode.OpenOrCreate))
        {
            // запись массива байтов в файл
            yield return fstream.WriteAsync(data, 0, data.Length);
            Debug.Log("Картинка создана записан в файл");
        }
    }

    public IEnumerator LoadPNG(string idRoom, bool isFloor, Image image)
    {
        string url = "file://";
        //Сюда передаём только id квартиры, остальную ссылку формируют тут
        if (isFloor)
        {
            url += Directory.GetCurrentDirectory() + _planeFloor + idRoom + ".png";
        }
        else
        {
            url += Directory.GetCurrentDirectory() + _planeRoom + idRoom + ".png";
        }
        
        using (WWW www = new WWW(url))
        {
            yield return www;
            //Debug.Log(www.texture);
            Texture2D texture2D = www.texture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            image.sprite = sprite;
        }
    }

    

    public void Resize(string idRoom, bool isFloor)
    {
        if (isFloor)
        {
            if(Directory.Exists(Directory.GetCurrentDirectory() + _convertFloor + idRoom + ".png")) return;
            image = Aspose.Imaging.Image.Load(Directory.GetCurrentDirectory() + "//Planer//PlansFloor//" + idRoom + ".png");
            image.Resize(image.Width/10, image.Height/10);
            image.Save(Directory.GetCurrentDirectory() + _convertFloor + idRoom + ".png");
            image.RemoveMetadata();
        }
        else
        {
            if(Directory.Exists(Directory.GetCurrentDirectory() + "//Planer//ConvertRoom//" + idRoom + ".png")) return;
            image = Aspose.Imaging.Image.Load(Directory.GetCurrentDirectory() + "//Planer//PlansRoom//" + idRoom + ".png");
            image.Resize(image.Width/10, image.Height/10);
            image.Save(Directory.GetCurrentDirectory() + "//Planer//ConvertRoom//" + idRoom + ".png");
            image.RemoveMetadata();
        }
    }

}
