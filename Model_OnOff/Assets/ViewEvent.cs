using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewEvent : MonoBehaviour {

    void OnEnable()
    {
      GameObject.Find("UI Root (3D)").GetComponent<MqttManager>().SetValue();
    }
}
