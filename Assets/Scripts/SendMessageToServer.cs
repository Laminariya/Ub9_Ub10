using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SendMessageToServer : MonoBehaviour
{

    private GameManager _manager;
    private Coroutine _coroutine;
    private MyApartment _currentMyApartment;
    
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

    public void StartRoomOnFloor(MyApartment realtyObject)
    {
        _currentMyApartment = realtyObject;
        
        OnRoom(realtyObject);
    }
    
    public void OnFloor(MyApartment realtyObject) //HH03SSXX01RRGGBB\r\n Включить бледным свечением этаж
    {
        //HH03SSXX01080000
        
        string str = realtyObject.SectionNumber.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "03";
        string f = realtyObject.Floor.ToString("X");
        if (f.Length == 1) f = "0" + f;
        str += f;
        string s = 1.ToString("X");
        if (s.Length == 1) s = "0" + s;
        str += s + "03000000";
        Debug.Log("Mess Floor: "+ realtyObject.SectionNumber + " // " + realtyObject.Floor);
        _manager.UdpClient.AddMessage(str, "Floor: " + realtyObject.SectionNumber + " // " + realtyObject.Floor);
        return;
        
        string message = GetHexXX(realtyObject.SectionNumber);
        message += "03";
        message += GetHexXX(realtyObject.Floor);
        message += "0103000000";
        _manager.UdpClient.AddMessage(message, "OnFloor " + realtyObject.Floor);
    }

    public void OnRoom(MyApartment realtyObject) //HH01FFFF03000000\r\n HH - номер дома, FFFF - номер квартиры (для выключения 03 меняем на 00)
    {
        //HH01FFFF03000000
        string str = realtyObject.SectionNumber.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "01";
        string f = realtyObject.Number.ToString("X");
        if (f.Length == 1) f = "000" + f;
        else if (f.Length == 2) f = "00" + f;
        else if (f.Length == 3) f = "0" + f;
        f += "03000000";
        str += f;
        //Debug.Log("Mess Flat: "+ realtyObject.sectionNumber + " // " + realtyObject.number);
        _manager.UdpClient.AddMessage(str, "Flat: " + realtyObject.SectionNumber + " // " + realtyObject.Number);
        return;
        
        string message = GetHexXX(realtyObject.SectionNumber);
        message += "01";
        message += GetHexXXHH(realtyObject.Number);
        message += "03000000";
        _manager.UdpClient.AddMessage(message, "OnRoom " + realtyObject.Number);
    }

  
    
    public void OnSection(string number) //HH02010300000000\r\n HH - номер дома, FFFF - номер квартиры (для выключения 03 меняем на 00)
    {
        string message = "0"+ number;
        message += "02010300000000";
        _manager.UdpClient.AddMessage(message, "OnSection " + number);
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
        Debug.Log("XXX " + value);
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
    
    public void MessageOnHouse(int house, int porch, bool isOn = true)
    {
        //Debug.Log(house+" " + porch);
        //HH02PP0300000000
        string str = house.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "02";
        string por = porch.ToString("X");
        if(por.Length==1) por = "0" + por;
        str += por;
        if (isOn) str += "0300000000";
        else str += "0000000000";
        Debug.Log("Mess House");
        _manager.UdpClient.AddMessage(str, "House " + house + " // " + porch);
    }

    public void MessageOnFlat(int house, int porch, int flat, bool isOn = true)
    {
        //HH01FFFF03000000
        string str = house.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "01";
        string f = flat.ToString("X");
        if (f.Length == 1) f = "000" + f;
        else if (f.Length == 2) f = "00" + f;
        else if (f.Length == 3) f = "0" + f;
        if (isOn) f += "03000000";
        else f += "00000000";
        str += f;
        Debug.Log("Mess Flat");
        _manager.UdpClient.AddMessage(str, "Flat: " + house + " // " + porch + " // " + flat);
    }

    public void MessageOnFloor(int house, int porch, int floor)
    {
        //HH03SSXX03000000
        string str = house.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "03";
        string f = floor.ToString("X");
        if (f.Length == 1) f = "0" + f;
        str += f;
        string s = porch.ToString("X");
        if (s.Length == 1) s = "0" + s;
        str += s + "03000000";
        Debug.Log("Mess Floor");
        _manager.UdpClient.AddMessage(str, "Floor: " + house + " // " + porch + " // " + floor);
    }

    public void MessageOffAllLight()
    {
        Debug.Log("Mess OffAll");
        _manager.UdpClient.AddMessage("007F060100000000", "OffAllLight"); //Погасить всё!!!
    }

    public void MessageOnDemo()
    {
        Debug.Log("Mess Demo");
        _manager.UdpClient.AddMessage("0064010000000000", "OnDemo");  //Включить демо!
    }

}
