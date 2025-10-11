using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class LoadJsonClass : MonoBehaviour
{
    public Image ImageTest;
    public SVGImage svgimg;
    
    private Json _json = new Json();
    private HttpClient Client = new HttpClient();

    private string _url =
        "http://81.163.26.132:7060/handlers/view-70-kutuzovski.json";
    private string _urlSVG =
        "https://sreda.ru/storage/flat/2024/07/16/6-1el-r1-6-1x7-4-a-v0-k_31479ce04c1d3b0350c8656842a1fa26.svg";

    private string _token;
    private GameManager _manager;

    public void Init(GameManager manager)
    {
        _manager = manager;
    }

    private void Start()
    {
        //LoadJSON(_url);
        //StartCoroutine(DownloadSVG());
        //StartCoroutine(LoadSVG(_urlSVG));
        
        //PostZapros(); //Загружаем json с сервера
    }

    private bool CheckInternetConnection1()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("https://www.google.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
    
    public bool CheckInternetConnection2(string strServer)
    {
        try
        {
            HttpWebRequest reqFP = (HttpWebRequest)HttpWebRequest.Create(strServer);
            HttpWebResponse rspFP = (HttpWebResponse)reqFP.GetResponse();
            if (HttpStatusCode.OK == rspFP.StatusCode)
            {
                // HTTP = 200 - Интернет безусловно есть! 
                rspFP.Close();
                return true;
            }
            else
            {
                // сервер вернул отрицательный ответ, инета нет
                rspFP.Close();
                return false;
            }
        }
        catch (WebException)
        {
            // Ошибка, интернета у нас нет.
            return false;
        }
    }

    public async Task PostZapros()
    {
        if(Application.internetReachability==NetworkReachability.NotReachable) 
        {
            Debug.Log("Not inet 1");
            return; //Проверяем наличие инета!!!
        }

        if (!CheckInternetConnection1()) //Проверка на наличие инета
        {
            Debug.Log("Not inet 2");
            return;
        }
        
        Account account = new Account();
        account.password = "4z8p1etsh3wU0KK~r7Qa";
        account.username = "maketbm";
        
        var jsonAccount = JsonUtility.ToJson(account);

        // Устанавливаем заголовок Content-Type
        var content = new StringContent(jsonAccount, Encoding.UTF8, "application/json");

        var url = "https://crm-api.mr-group.ru/site/token";
        //Debug.Log("1");
        // Отправляем POST запрос асинхронно
        Client.Timeout = TimeSpan.FromSeconds(10); // Установить тайм-аута на 30 секунд;
        HttpResponseMessage response = await Client.PostAsync(url, content);
       
       // Debug.Log("2");
        // Проверяем результат
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            _token = responseContent;
        }
        else
        {
            // Обработка ошибок
            Debug.Log(response.StatusCode);
            return;
        }

        var client2 = new HttpClient();
//        Debug.Log(_token);
        Token token = new Token();
        token.token = _token;
        var jsonToken = JsonUtility.ToJson(token);

        // Устанавливаем заголовок Content-Type
        var contentToken = new StringContent(jsonToken, Encoding.UTF8, "application/json"); //jsonToken, Encoding.UTF8 ,  "application/json"); //, Encoding.UTF8 "application/json"
        
        var url2 = "https://crm-api.mr-group.ru/site/api/v1/Catalogue/fabdd00e-c970-4357-a1fa-4e1db3b5407f";
        client2.BaseAddress = new Uri(url2);
        client2.DefaultRequestHeaders.Accept.Clear();
        client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        // Отправляем POST запрос асинхронно
        var response2 = await client2.GetStringAsync(url2);
        Debug.Log("JSON :: " + response2.Length);

        _manager.JsonText = response2;
        //WriteJson(response2);
    }

    private async Task LoadJSON(string url)
    {
        var uri = new Uri(url);

        var result = await Client.GetAsync(uri);
        string str = await result.Content.ReadAsStringAsync();
        _json = JsonUtility.FromJson<Json>(str);
        Debug.Log(_json.flat.Count);
        Debug.Log(_json.flat[0].flat_plan_png);
        Debug.Log(_json.lastSuccessAt.date);
    }

    IEnumerator DownloadSVG()
    {
        string url =
            "https://sreda.ru/storage/flat/2024/07/16/6-1el-r1-6-1x7-4-a-v0-k_31479ce04c1d3b0350c8656842a1fa26.svg";
        
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Error while Receiving: " + www.error);
        }

        else
        {
            Debug.Log(www.downloadHandler.text);
            //Convert byte[] data of svg into string
            string bitString = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            var tessOptions = new VectorUtils.TessellationOptions()
            {
                StepDistance = 100.0f,
                MaxCordDeviation = 0.5f,
                MaxTanAngleDeviation = 0.1f,
                SamplingStepSize = 0.01f
            };

            StringReader reader = new StringReader(bitString);
            // Dynamically import the SVG data, and tessellate the resulting vector scene.
            var sceneInfo = SVGParser.ImportSVG(reader, 0, 10f, 356, 496);
            var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);
            // Build a sprite with the tessellated geometry
            Sprite sprite =
                VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
            svgimg.sprite = sprite;
            RectTransform rectTransform = svgimg.rectTransform;
            rectTransform.offsetMax = new Vector2(200, 300);
            rectTransform.offsetMin = new Vector2(-100, -200);
        }
    }
    

    private async Task WriteJson(string text)
    {
        string path = Directory.GetCurrentDirectory() + "\\note1.txt"; //"C://Projects//PlanerRooms//Assets

        // полная перезапись файла 
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            await writer.WriteAsync(text);
        }
        Debug.Log("Save note1.txt complete");
    }
    
    public async Task<string> ReadJson(string text)
    {
        string path = "C://Projects//PlanerRooms//Assets//note1.txt";
        string jsonText;
        
        // полная перезапись файла 
        using (StreamReader reader = new StreamReader(path, false))
        {
            jsonText = await reader.ReadToEndAsync();
        }

        return jsonText;
    }

}

[Serializable]
public class Json
{
    public List<Room> flat = new List<Room>();
    public Success lastSuccessAt;
}

[Serializable]
public class Room
{
    public int id;
    public string bulk_name;
    public int floor;
    public int number;
    public int number_on_floor;
    public int rooms;
    public string flat_plan_png;
    public int quantity;
    public int price;
    public float area;
    public string status;
}

[Serializable]
public class Success
{
    public string date;
}

[Serializable]
public class Account
{
    public string username;
    public string password;
}

[Serializable]
public class Token
{
    public string token;
}