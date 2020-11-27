using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;
    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;

    private void Start()
    {
        defaultScale = buttonScale.localScale;
    }
    bool isSound;
    public void OnPointerDown(PointerEventData eventData) //OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.Start:
                Debug.Log("게임 시작");
                break;
            case BTNType.Next:
                Debug.Log("다음으로");
                break;
            case BTNType.Back:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                break;
            case BTNType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Sound:

                if (isSound)
                {
                    Debug.Log("사운드 OFF");
                }
                else
                {
                    Debug.Log("사운드 ON");
                }
                isSound = !isSound;
                break;
            case BTNType.Quit:
                Application.Quit();
                Debug.Log("나가기");
                break;

        }
    }
    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
    public void OnPointerEnter(PointerEventData eventData) //버튼 올리면 커지기
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData) //버튼 내리면 다시 작아지기
    {
        buttonScale.localScale = defaultScale;
    }

    public void  GoScene() //씬 이동
    {
        SceneManager.LoadScene(1);
    }
}
