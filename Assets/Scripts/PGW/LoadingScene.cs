using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    static string nextScene;
    public Image ProgressBar;
    public GameObject[] Tips;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                ProgressBar.fillAmount = op.progress;
            }
            else
            {
                timer += 0.0005f;
                ProgressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (ProgressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
    private void Start()
    {

        StartCoroutine(LoadSceneProcess());
        int random = Random.Range(0, Tips.Length);
        Tips[random].SetActive(true);
    }

}
