using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Animator anim;

    public bool isOver;
    public bool isClear;
    public bool isStop;

    public GameObject OptionUI;
    public GameObject GenIcon;
    public GameObject GasCanIcon;

    public Text MissionCount;

    bool isGascan;
    bool isGen;

    public List<bool> PoliceAware = new List<bool>();

    MissionCreate Mission;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Mission = FindObjectOfType<MissionCreate>();
        if(Mission.theMission == MissionCreate.MissionList.Gascan)
        {
            isGascan = true;
            GasCanIcon.SetActive(true);
        }
        else if(Mission.theMission == MissionCreate.MissionList.Generator)
        {
            isGen = true;
            GenIcon.SetActive(true);
        }
    }

    void Update()
    {
        CheckGameOver();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isStop)
            {

                Resume();

            }
            else
            {
                Pause();


            }
        }
        CheckMissionCount();
    }

    private void CheckMissionCount()
    {
        if (isGascan)
        {
            MissionCount.text = Mission.CurrentGascan.ToString() + " / " + Mission.RequireGascan.ToString();

        }
        else if (isGen)
        {
            MissionCount.text = Mission.CurrentGenerator.ToString() + " / " + Mission.RequireGenerator.ToString();

        }

    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스를 게임 중앙 좌표에 고정시키고 마우스커서가 안보임
        Cursor.visible = false;
        OptionUI.SetActive(false);
        isStop = false;


    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OptionUI.SetActive(true);
        isStop = true;


    }
    public void GameQuit()
    {
        SceneManager.LoadScene("Main");
    }

    public void CheckGameOver()
    {
        if (isOver)
        {
            StartCoroutine(GameOver());
        }

        if (isClear)
        {
            StartCoroutine(GameClear());
        }
    }
    IEnumerator GameOver()
    {
        anim.SetTrigger("FadeOut");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator GameClear()
    {
        anim.SetTrigger("FadeOut");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameClear");
    }
}
