using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class MyUdpClient : MonoBehaviour
{
    //192.168.31.104 Server

    private UdpClient udpClient = new UdpClient();
    private GameManager _manager;
    private string _ip = "127.0.0.1";
    private const string _key = "SaveIp";
    private bool _isComplete;
    
    private Queue<string> _queue = new Queue<string>();
    private int _count = 0;
    
    public void Init(GameManager manager)
    {
        _manager = manager;
        //udpClient.Connect("127.0.0.1", 5555); // отправляем данные только на 127.0.0.1:5555
        StartConnect();
    }

    private void Update()
    {
        if (_queue.Count > 0 && _isComplete)
        {
            //Debug.Log("Send");
            Send(_queue.Dequeue());
        }
    }

    public void ClearQueue()
    {
        _queue.Clear();
    }

    public void AddMessage(string str, string nameMess)
    {
        //Debug.Log(nameMess);
        _queue.Enqueue(str);
    }

    private void StartConnect()
    {
        udpClient?.Dispose();
        
        udpClient = new UdpClient();
        if (!PlayerPrefs.HasKey(_key))
        {
            _ip = "192.168.0.104";
            udpClient.Connect(_ip, 5555);
            _isComplete = true;
            PlayerPrefs.SetString(_key, _ip);
            return;
        }
        
        _ip = PlayerPrefs.GetString(_key);
        //Debug.Log(_ip);
        //if (udpClient.Client!=null && udpClient.Client.Connected)
        //{
        //    udpClient.Dispose();
        //}
        
        udpClient.Connect(_ip, 5555);
        _isComplete = true;
    }

    public void Reload()
    {
        CloseConnect();
        StartConnect();
    }

    private void CloseConnect()
    {
        if(udpClient.Client.Connected)
            udpClient.Close();
    }

    private async void Send(string message)
    {
        _isComplete = false;
        if (udpClient.Client.Connected) //Проверяем есть ли подключение
        {
            // отправляемые данные
            // преобразуем в массив байтов
            //Debug.Log(message);
            byte[] data = Encoding.UTF8.GetBytes(message);
            // отправляем данные
            try
            {
                int bytes = await udpClient.SendAsync(data, data.Length);
                //Debug.Log(bytes);
            }
            catch (Exception e)
            {
                Debug.LogError("XXX "+e);
                StartConnect();
                Send(message);
                throw;
            }

            //Debug.Log($"Отправлено {bytes} байт" + message);
        }
        else //Если нет, то пробуем переподключиться
        {
            Debug.LogError("ZZZ");
            udpClient.Connect(_ip, 5555);
            await Task.Delay(500);
            if (udpClient.Client.Connected)
            {
                // отправляемые данные
                //string message = "Hello METANIT.COM";
                // преобразуем в массив байтов
                byte[] data = Encoding.UTF8.GetBytes(message);
                // отправляем данные
                int bytes = await udpClient.SendAsync(data, data.Length);
                //Debug.Log( $"Отправлено {bytes} байт" + message);
            }
        }

        await Task.Delay(100);
        //Debug.Log("Send Complete");
        _isComplete = true;
    }

}
