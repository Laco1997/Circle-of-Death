using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteraction : MonoBehaviour
{
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttachSword()
    {
        sword.SetActive(true);
    }

    public void DeattachSword()
    {
        sword.SetActive(false);
    }

    public void AttachBow()
    {
        bow.SetActive(true);
        arrow.SetActive(true);
    }

    public void DeattachBow()
    {
        bow.SetActive(false);
        arrow.SetActive(false);
    }
}
