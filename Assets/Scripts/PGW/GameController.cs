using System.Collections;
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
    [SerializeField] private GameObject OptionUI = null;

    public delegate void AwareIcon_EventHandler();
    public AwareIcon_EventHandler WarningIconEvent;
    public AwareIcon_EventHandler IdleIconEvent;

    public static GameController instance;

    public List<EnemyAI> awarePoliceList = new List<EnemyAI>();

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
            if (IsStop)
            {

                Resume();

            }
            else
            {
                Pause();


            }
        }
    }

    public void AddAwaredPolice(EnemyAI awaredPolice)
    {
        if (awarePoliceList.Count == 0)
        {
            WarningIconEvent?.Invoke();
        }
        awarePoliceList.Add(awaredPolice);
    }

    public void RemoveAwaredPolice(EnemyAI awaredPolice)
    {
        awarePoliceList.Remove(awaredPolice);
        if (awarePoliceList.Count == 0)
        {
            IdleIconEvent?.Invoke();
        }
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        OptionUI.SetActive(false);
        IsStop = false;


    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OptionUI.SetActive(true);
        IsStop = true;


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
