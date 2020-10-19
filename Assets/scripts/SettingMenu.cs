using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    private void Awake()
    {
        GameObject.Find("ReturnButton").GetComponent<Button>().onClick.AddListener(Return);
        GameObject.Find("ReturnButton").GetComponent<Button>().AddButtonSound();
        GameObject.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener(ValueChange);
    }

    private void ValueChange(bool arg0)
    {
        Debug.Log("Value change to : " + arg0);
    }


    private void Return()
    {
        Debug.Log("return");
        Loader.Load(Loader.Scene.MainMenu);
    }
}
