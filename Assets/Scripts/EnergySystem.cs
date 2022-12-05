using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    public Slider playerEnergy;
    public int maxEnergy;
    private int currentEnergy;
    private bool energyGaining = false;
    public TMP_Text currentEnergyText;

    void Start()
    {
        currentEnergy = maxEnergy;
        playerEnergy.value = this.getPercentage();
    }

    void Update()
    {
        if(currentEnergy != maxEnergy && !energyGaining)
        {
            energyGaining = true;
            StartCoroutine(energyGainer(5));
        }
    }

    public float getPercentage()
    {
        return (float)currentEnergy / (float)maxEnergy * 500;

    }

    public void energyUsed(int amount)
    {
        currentEnergy -= amount;
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
        }
        playerEnergy.value = getPercentage();

        currentEnergyText.text = currentEnergy.ToString();

        //Debug.Log(currentEnergy);
    }

    public void energyGained(int amount)
    {
        currentEnergy += amount;
        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        playerEnergy.value = getPercentage();

        //Debug.Log(currentEnergy);

        currentEnergyText.text = currentEnergy.ToString();
    }

    IEnumerator energyGainer(int amount)
    {
        energyGained(amount);

        yield return new WaitForSeconds(1f);

        energyGaining = false;
    }

    public int CurrentEnergy
    {
        get { return currentEnergy; }
    }
}
