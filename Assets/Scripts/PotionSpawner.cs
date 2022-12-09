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
    [SerializeField] private GameObject boss;
    Follower bossFollower;

    void Start()
    {
        bossFollower = boss.GetComponent<Follower>();
    }

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
        if (bossFollower.stage == 3)
        {
            healthRandPos = new Vector3(Random.Range(50, 160), 0.5f, Random.Range(50, 140));
        }
        Instantiate(healthPotion, healthRandPos, Quaternion.identity);
    }

    void SpawnEnergyPotion()
    {
        energyPotionPickedUp = false;
        Vector3 energyRandPos = new Vector3(Random.Range(160, 240), 0.5f, Random.Range(150, 230));
        if (bossFollower.stage == 3)
        {
            energyRandPos = new Vector3(Random.Range(50, 160), 0.5f, Random.Range(50, 140));
        }
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
