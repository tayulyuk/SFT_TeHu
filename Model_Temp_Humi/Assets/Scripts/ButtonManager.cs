using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public bool pingPong;
    private MqttManager mqttManager;

    void Start()
    {
        mqttManager = GameObject.Find("UI Root (3D)").GetComponent<MqttManager>();
        pingPong = false;
        //SendAllButtonSetting();
    }
    private void OnClick()
    {
        if (transform.name == "reConnectButton")
            mqttManager.isReConnect = false;
        if (transform.name == "errorButton")
            mqttManager.isError = false;
        if (transform.name == "Button - Exit")
            Application.Quit();
      
    }

    /// <summary>
    /// 버튼의 스위칭 명령   0->1   1->0변환 명령.
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    private string SendOrder(string order)
    {
        string v = "";
        if (order == "1")
            v = "0";
        if (order == "0")
            v = "1";
        else
            v = "0";
        return v;
    }

  
}
