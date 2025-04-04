﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameOverType
{
    Victory,
    Defeat
}
public class GameController : MonoBehaviour
{
    [SerializeField] private Animator anim = null;

    public delegate void OptionUI_EventHandler(bool flag);
    public OptionUI_EventHandler OptionUIEvent;

    public delegate void AwareIcon_EventHandler();
    public AwareIcon_EventHandler WarningIconEvent;
    public AwareIcon_EventHandler IdleIconEvent;

    public static GameController instance;

    public List<BaseEnemy> awarePoliceList = new List<BaseEnemy>();

    private bool isGameOver;
    private bool isStop;
    public bool IsStop
    {
        get => isStop;
        private set => isStop = value;
    }


    private void Awake()
    {
        if (instance == null)
        {

            instance = this;
        }
    }


    void Update()
    {

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
    }

    public void AddAwaredPolice(BaseEnemy awaredPolice)
    {
        if (awarePoliceList.Count == 0)
        {
            WarningIconEvent?.Invoke();
        }
        if (awarePoliceList.Contains(awaredPolice)) return;
        awarePoliceList.Add(awaredPolice);
    }

    public void RemoveAwaredPolice(BaseEnemy awaredPolice)
    {
        if (awarePoliceList.Contains(awaredPolice))
        {
            awarePoliceList.Remove(awaredPolice);

            if (awarePoliceList.Count == 0)
            {
                IdleIconEvent?.Invoke();
            }
        }
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isStop = false;
        OptionUIEvent?.Invoke(isStop);


    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isStop = true;
        OptionUIEvent?.Invoke(isStop);


    }
    public void GameQuit()
    {
        SceneManager.LoadScene("Main");
    }

    public IEnumerator GameOver(GameOverType type)
    {
        if (isGameOver) yield break;

        isGameOver = true;
        anim.SetTrigger("FadeOut");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(1f);
        switch (type)
        {
            case GameOverType.Victory:
                SceneManager.LoadScene("GameClear");
                break;

            case GameOverType.Defeat:
                SceneManager.LoadScene("GameOver");
                break;

        }

    }
}
