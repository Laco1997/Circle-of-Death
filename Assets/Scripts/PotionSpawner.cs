using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre spawnovanie zivota a energie.
*/
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

    /*
     * Ziskanie skriptu Follower.
     */
    void Start()
    {
        bossFollower = boss.GetComponent<Follower>();
    }

    /*
    * Volanie resetu, ked hrac zdvihne potion.
    */
    public void resetTime()
    {
        time = 0f;
    }

    /*
    * Potvrdenie zdvihnutia healthu hracom.
    */
    public void healthPicked()
    {
        healthPotionPickedUp = true;
    }

    /*
    * Potvrdenie zdvihnutia energie hracom.
    */
    public void energyPicked()
    {
        energyPotionPickedUp = true;

    }

    /*
    * Casovac pre spawnovanie potions.
    */
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

    /*
    * Spawnovanie healthu prostrednictvom vytvorenia instancie objektu health.
    */
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

    /*
    * Spawnovanie energie prostrednictvom vytvorenia instancie objektu energy.
    */
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

    /*
    * Getter, ci bol health picked up.
    */
    public bool HealthPotionStatus
    {
        get { return healthPotionPickedUp; }
    }

    /*
    * Getter, ci bol energy picked up.
    */
    public bool EnergyPotionStatus
    {
        get { return energyPotionPickedUp; }
    }

}
