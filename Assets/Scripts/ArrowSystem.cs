using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowSystem : MonoBehaviour
{
    public int maxArrowCount;
    private int currentArrowCount;
    public TMP_Text currentArrowCountText;

    void Start()
    {
        currentArrowCount = maxArrowCount;
    }

    public float getPercentage()
    {
        return (float)currentArrowCount / (float)maxArrowCount * 20;

    }

    public void arrowsUsed(int amount)
    {
        currentArrowCount -= amount;
        if (currentArrowCount <= 0)
        {
            currentArrowCount = 0;
        }

        currentArrowCountText.text = currentArrowCount.ToString();

        //Debug.Log(currentEnergy);
    }

    public void arrowsCollected(int amount)
    {
        currentArrowCount += amount;
        if (currentArrowCount >= maxArrowCount)
        {
            currentArrowCount = maxArrowCount;
        }

        //Debug.Log(currentEnergy);

        currentArrowCountText.text = currentArrowCount.ToString();
    }

    public int CurrentArrows
    {
        get { return currentArrowCount; }
    }
}
