using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;

public class MqttManager : MonoBehaviour
{
    private MqttClient client;
    public string tempVal;
    public string humiVal;
   
    public UILabel tempLabel;
    public UILabel humiLabel;
   

    public GameObject errorPopUpObject;
    public GameObject reConnectPopUpObject;
    public bool isError; // error message 들어오면 팝업 띠워주자.
    public bool isReConnect; // 아두이노 wifi통신이 다시 접속했다는 메시지 창.
    public bool isOne; // 메세지 한번 받을때 마다 라벨에 값을 입력합니다.   라벨값은 메인쓰레드에서만 입력하라네요. 

    void Start()
    {
        // create client instance 
        client = new MqttClient(IPAddress.Parse("119.205.235.214"), 1883, false, null);

        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

       string clientId = Guid.NewGuid().ToString();
        //string clientId = "siheung_namu_moter";
        client.Connect(clientId);

        // subscribe to the topic "/home/temperature" with QoS 2 
        client.Subscribe(new string[] { "ModelTempHumi/result" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }
   
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {  
        Debug.Log("M: " + System.Text.Encoding.UTF8.GetString(e.Message));

        //moter constroler의 wifi가 불안정하여 다시 접속했다.
        if (System.Text.Encoding.UTF8.GetString(e.Message) == "Reconnected")
            isReConnect = true;

        ///test 끝나고 다시 연결해라.
        AllMessageParsing(System.Text.Encoding.UTF8.GetString(e.Message));
    }

   void Update()
    {
        //팝업 메시지 띠우기.
       errorPopUpObject.SetActive(isError);
       
        //아두이도 접속 창 띠우기.
       reConnectPopUpObject.SetActive(isReConnect);

      SetValue();
    }

    public void SetValue()
    {
        tempLabel.text = tempVal;
        humiLabel.text = humiVal;
    }
    /// <summary>
    /// 서버로 부터 받은 정보를 각 변수에 저장한다.
    /// </summary>
    /// <param name="getMessage">서버로 부터 받은 정보.</param>
    private void AllMessageParsing(string getMessage)
    {
        tempVal = GetParserString(getMessage, "|Temp=", "0|") + " ℃";
        humiVal = GetParserString(getMessage, "Humi=", "0|") +" %";
    }

    /// <summary>
    /// 서버로 부터 받은 정보를 나눈다.
    /// </summary>
    /// <param name="message">서버 data</param>
    /// <param name="startSearch">시작문구</param>
    /// <param name="endSearch">끝 문구</param>
    /// <returns></returns>
    public string GetParserString(string message ,string startSearch,string endSearch)
    {
        string getValue = "";
        string search = "";

        search = startSearch;        
    
        int p = message.IndexOf(search);
        if (p >= 0)
        {
            // move forward to the value
            int start = p + search.Length;
            // now find the end by searching for the next closing tag starting at the start position, 
            // limiting the forward search to the max value length
            int end = 0;
            end = message.IndexOf(endSearch, start);           

            if (end >= 0)
            {
                // pull out the substring
                string v = message.Substring(start, end - start);
                // finally parse into a float
                // float value = float.Parse(v);
                // Debug.Log("1classTemp Value = " + value);
              
               getValue = v;                
            }
            else
            {
                Debug.Log("Bad html - closing tag not found");
                getValue = "text error";
            }
        }
        return getValue;
    }

}
