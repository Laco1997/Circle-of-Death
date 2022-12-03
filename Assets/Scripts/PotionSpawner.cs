using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthPotion;
    [SerializeField] private GameObject energyPotion;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector3 healthRandPos = new Vector3(Random.Range(160, 240), 0.5f, Random.Range(150, 230));
            Vector3 energyRandPos = new Vector3(Random.Range(160, 240), 0.5f, Random.Range(150, 230));
            Instantiate(healthPotion, healthRandPos, Quaternion.identity);
            Instantiate(energyPotion, energyRandPos, Quaternion.identity);
        }
    }
}
