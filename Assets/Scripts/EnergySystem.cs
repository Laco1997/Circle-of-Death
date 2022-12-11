using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
* Ovladac pre ovladanie energie hraca. Ovladac spravuje ziskanie a vyuzitie
* energie hraca. Energia je prepojena s HUD.
*/
public class EnergySystem : MonoBehaviour
{
    public Slider playerEnergy;
    public int maxEnergy;
    int currentEnergy;
    bool energyGaining = false;
    public TMP_Text currentEnergyText;
    int energy = 5;

    void Start()
    {
        currentEnergy = maxEnergy;
        playerEnergy.value = this.getPercentage();
    }

    /*
    * Ovladanie pre priebezne ziskanie energie. Energia sa automaticky 
    * navysuje v malom mnozstve.
    */
    void Update()
    {
        if(currentEnergy != maxEnergy && !energyGaining)
        {
            energyGaining = true;
            StartCoroutine(energyGainer(energy));
        }
    }

    /*
    * Prepocitanie energie pre HUD.
    */
    public float getPercentage()
    {
        return (float)currentEnergy / (float)maxEnergy * maxEnergy;

    }

    /*
    * Funkcia pre vyuzitie energie.
    */
    public void energyUsed(int amount)
    {
        currentEnergy -= amount;
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
        }
        playerEnergy.value = getPercentage();

        currentEnergyText.text = currentEnergy.ToString();
    }

    /*
    * Funkcia pre ziskanie energie.
    */
    public void energyGained(int amount)
    {
        currentEnergy += amount;
        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        playerEnergy.value = getPercentage();

        currentEnergyText.text = currentEnergy.ToString();
    }

    /*
    * Interval pre automaticke ziskanie energie.
    */
    IEnumerator energyGainer(int amount)
    {
        energyGained(amount);

        yield return new WaitForSeconds(1f);

        energyGaining = false;
    }

    /*
    * Getter pre aktualnu hodnotu energie.
    */
    public int CurrentEnergy
    {
        get { return currentEnergy; }
    }
}
