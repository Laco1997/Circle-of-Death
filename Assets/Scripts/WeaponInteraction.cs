using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre prepinanie sa medzi zbranami.
*/
public class WeaponInteraction : MonoBehaviour
{
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject arrow;

    /*
     * Aktivacia meca.
     */
    public void AttachSword()
    {
        sword.SetActive(true);
    }

    /*
     * Deaktivacia meca.
     */
    public void DeattachSword()
    {
        sword.SetActive(false);
    }

    /*
     * Aktivacia luku.
     */
    public void AttachBow()
    {
        bow.SetActive(true);
        arrow.SetActive(true);
    }

    /*
     * Deaktivacia luku.
     */
    public void DeattachBow()
    {
        bow.SetActive(false);
        arrow.SetActive(false);
    }

    /*
     * Aktivacia sipu.
     */
    public void AttachArrow()
    {
        arrow.SetActive(true);
    }

    /*
     * Deaktivacia sipu.
     */
    public void DeattachArrow()
    {
        arrow.SetActive(false);
    }
}
