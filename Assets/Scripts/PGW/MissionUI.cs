using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private Image missionIcon = null;
    [SerializeField] private Text missionCount = null;

    public Image MissionIcon
    {
        get { return missionIcon; }
        set { missionIcon = value; }
    }
    private Mission theMission = null;

    // Start is called before the first frame update
    void Start()
    {
        theMission = GetComponent<Mission>();

    }

    public void UpdateMissionUI(int currentCount, int requireCount)
    {
        missionCount.text = currentCount.ToString() + " / " + requireCount.ToString();
    }

}
