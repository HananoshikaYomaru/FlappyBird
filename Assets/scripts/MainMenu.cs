using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{

    private void Awake()
    {
        GameObject.Find("PlayButton").GetComponent<Button>().onClick.AddListener(Play);
        GameObject.Find("SettingButton").GetComponent<Button>().onClick.AddListener(Setting);
        GameObject.Find("ExitButton").GetComponent<Button>().onClick.AddListener(Exit);

        GameObject.Find("PlayButton").AddComponent<ButtonSound>();
        GameObject.Find("SettingButton").GetComponent<Button>().AddButtonSound();
        GameObject.Find("ExitButton").GetComponent<Button>().AddButtonSound();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Setting()
    {
        Loader.Load(Loader.Scene.SettingMenu);
    }

    private void Exit()
    {
        Debug.Log("exit");
        Application.Quit();
    }

    private void Play()
    {
        Debug.Log("play");
        Loader.Load(Loader.Scene.GameScene);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
