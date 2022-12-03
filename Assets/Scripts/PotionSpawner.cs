using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthPotion;
    [SerializeField] private GameObject energyPotion;
    int spawnTime = 12;
    float time = 0f;
    bool healthPotionPickedUp = true;
    bool energyPotionPickedUp = true;

    public void resetTime()
    {
        time = 0f;
    }

    public void healthPicked()
    {
        healthPotionPickedUp = true;
    }

    public void energyPicked()
    {
        energyPotionPickedUp = true;

    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        Debug.Log(time);

        if (time >= spawnTime)
        {
            if (healthPotionPickedUp)
            {
                SpawnHealthPotion();
            }
            if (energyPotionPickedUp) {
                SpawnEnergyPotion();
            }
        }
    }

    void SpawnHealthPotion()
    {
        healthPotionPickedUp = false;
        Vector3 healthRandPos = new Vector3(Random.Range(160, 240), 0.5f, Random.Range(150, 230));
        Instantiate(healthPotion, healthRandPos, Quaternion.identity);
    }

    void SpawnEnergyPotion()
    {
        Debug.Log("ENERGY");
        energyPotionPickedUp = false;
        Vector3 energyRandPos = new Vector3(Random.Range(160, 240), 0.5f, Random.Range(150, 230));
        Instantiate(energyPotion, energyRandPos, Quaternion.identity);
    }

    public bool HealthPotionStatus
    {
        get { return healthPotionPickedUp; }
    }

    public bool EnergyPotionStatus
    {
        get { return energyPotionPickedUp; }
    }

}
