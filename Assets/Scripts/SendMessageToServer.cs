using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SendMessageToServer : MonoBehaviour
{

    private GameManager _manager;
    private Coroutine _coroutine;
    private RealtyObject _currentRoom;
    
    public void Init(GameManager manager)
    {
        _manager = manager;
        //Test();
        //StartCoroutine(TestSend());
    }

    IEnumerator TestSend()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 5; i++)
        {
            
            for (int j = 0; j < 10; j++)
            {
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
                yield return new WaitForSeconds(Random.Range(1, 3));
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
                yield return new WaitForSeconds(Random.Range(1, 2));
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
                _manager.UdpClient.AddMessage("0102040300000000", "Test Send");
            }
            yield return new WaitForSeconds(Random.Range(1, 2));
        }
        
    }

    private void Test()
    {
        Debug.Log(GetHexXXHH(300123));
    }

    public void StartRoomOnFloor(RealtyObject realtyObject)
    {
        _currentRoom = realtyObject;
        
        OnRoom(realtyObject);
    }
    
    public void OnFloor(RealtyObject realtyObject) //HH03SSXX01RRGGBB\r\n Включить бледным свечением этаж
    {
        //HH03SSXX01080000
        string message = GetHexXX(realtyObject.sectionNumber);
        message += "03";
        message += GetHexXX(realtyObject.floor);
        message += "0103000000";
        _manager.UdpClient.AddMessage(message, "OnFloor " + realtyObject.floor);
    }

    public void OffFloor(RealtyObject realtyObject) //HH03SSXX00000000\r\n HH - номер дома, SS - Этаж, XX - секция
    {
        string message = GetHexXX(realtyObject.sectionNumber);
        message += "03";
        message += GetHexXX(realtyObject.floor);
        message += "0100000000";
        _manager.UdpClient.AddMessage(message, "OffFloor " + realtyObject.floor);
    }

    public void OnRoom(RealtyObject realtyObject) //HH01FFFF03000000\r\n HH - номер дома, FFFF - номер квартиры (для выключения 03 меняем на 00)
    {
        //HH01FFFF03000000
        string message = GetHexXX(realtyObject.sectionNumber);
        message += "01";
        message += GetHexXXHH(realtyObject.number);
        message += "03000000";
        _manager.UdpClient.AddMessage(message, "OnRoom " + realtyObject.number);
    }

    public void OffRoom(RealtyObject realtyObject)
    {
        //HH01FFFF00000000
        string message = GetHexXX(realtyObject.sectionNumber);
        message += "01";
        message += GetHexXXHH(realtyObject.number);
        message += "00000000";
        _manager.UdpClient.AddMessage(message, "OffRoom " + realtyObject.number);
    }
    
    public void OnSection(string number) //HH02010300000000\r\n HH - номер дома, FFFF - номер квартиры (для выключения 03 меняем на 00)
    {
        string message = "0"+ number;
        message += "02010300000000";
        _manager.UdpClient.AddMessage(message, "OnSection " + number);
    }
    
    public void OffSection(string number) //HH02010000000000\r\n HH - номер дома, FFFF - номер квартиры (для выключения 03 меняем на 00)
    {
        string message = "0"+ number;
        message += "02010000000000";
        _manager.UdpClient.AddMessage(message, "OffSection " + number);
    }
    
    public void OffAll() //007F060100000000\r\n HH - номер дома, FFFF - номер квартиры (для выключения 03 меняем на 00)
    {
        _manager.UdpClient.ClearQueue();
        _manager.UdpClient.AddMessage("007F060100000000", "OffAll");
    }

    public void OnAll()
    {
        for (int i = 1; i < 7; i++)
        {
            string message = "0"+ i;
            message += "02010300000000";
            _manager.UdpClient.AddMessage(message, "OnSection " + i);
        }
        _manager.UdpClient.AddMessage("0102040300000000", "OnSection 7"); //Включить 7 корпус
    }

    private string GetHexXX(int value)
    {
        string first = "";
        string second = "";

        first = GetNumber16(value / 16);
        second = GetNumber16(value % 16);

        return first + second;
    }

    private string GetHexXXHH(int value)
    {
        string number = value.ToString();
        if (number.Length > 5)
        {
            number = number.Substring(number.Length - 3);
            int v = Int32.Parse(number);
            string v1 = "0";
            string v2 = "";
            string v3 = "";
            string v4 = "";

            v2 = GetNumber16(v / 256);
            int n = v % 256;
            v3 = GetNumber16(n / 16);
            v4 = GetNumber16(n % 16);
            return v1 + v2 + v3 + v4;
        }
        else
        {
            number = number.Substring(number.Length - 2);
            int v = Int32.Parse(number);
            string v1 = "0";
            string v2 = "0";
            string v3 = "";
            string v4 = "";
            
            v3 = GetNumber16(v / 16);
            v4 = GetNumber16(v % 16);
            return v1 + v2 + v3 + v4;
        }
    }

    private string GetNumber16(int value)
    {
        string str = "";
        switch (value)
        {
            case 0:
            {
                return str = "0";
            }
            case 1:
            {
                return str = "1";
            }
            case 2:
            {
                return str = "2";
            }
            case 3:
            {
                return str = "3";
            }
            case 4:
            {
                return str = "4";
            }
            case 5:
            {
                return str = "5";
            }
            case 6:
            {
                return str = "6";
            }
            case 7:
            {
                return str = "7";
            }
            case 8:
            {
                return str = "8";
            }
            case 9:
            {
                return str = "9";
            }
            case 10:
            {
                return str = "A";
            }
            case 11:
            {
                return str = "B";
            }
            case 12:
            {
                return str = "C";
            }
            case 13:
            {
                return str = "D";
            }
            case 14:
            {
                return str = "E";
            }
            case 15:
            {
                return str = "F";
            }
        }
        return "0";
    }

}
