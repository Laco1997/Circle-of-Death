using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre spravu dat medzi scenami. Trieda vytvara staticku instanciu 
* objektu obsahujuceho health bossa a hraca, ktora sa nezmaze po nacitani
* novej sceny.
*/
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public int PlayerHealth = 500;
    public int BossHealth = 15000;

    /*
     * Ak nemame instanciu, tak sa dany objekt zmaze. V opacnom pripade
     * ho ulozime a nastavime tak, aby sa po loadnuti sceny nezmazal.
     */
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
