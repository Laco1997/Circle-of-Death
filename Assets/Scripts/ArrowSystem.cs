using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowSystem : MonoBehaviour
{
    public int maxArrowCount;
    int currentArrowCount;
    public TMP_Text currentArrowCountText;

    void Start()
    {
        currentArrowCount = maxArrowCount;
    }

    public float getPercentage()
    {
        return (float)currentArrowCount / (float)maxArrowCount * maxArrowCount;

    }

    public void arrowsUsed(int amount)
    {
        currentArrowCount -= amount;
        if (currentArrowCount <= 0)
        {
            currentArrowCount = 0;
        }

        currentArrowCountText.text = currentArrowCount.ToString();
    }

    public void arrowsCollected(int amount)
    {
        currentArrowCount += amount;
        if (currentArrowCount >= maxArrowCount)
        {
            currentArrowCount = maxArrowCount;
        }

        currentArrowCountText.text = currentArrowCount.ToString();
    }

    public int CurrentArrows
    {
        get { return currentArrowCount; }
    }
}
