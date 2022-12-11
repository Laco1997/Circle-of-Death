using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre strielanie z luku.
*/
public class Bow : MonoBehaviour
{
    public float damage = 5f;
    bool equippedBow = true;
    bool canShoot = true;
    [SerializeField] private float shootCooldownTime = 0.45f;
    public float shootForce, upwardForce;
    public Camera cam;
    public Transform attackPoint;
    [SerializeField] private GameObject arrow;
    float middleX = 0.5f;
    float middleY = 0.5f;
    int middleZ = 0;

    /*
    * Ak ma hrac equipnuty luk, moze strielat a stlaci lave tlacidlo mysi, tak sa 
    * vytvori instancia sipu, a vystreli sa sip. Sip sa striela pomocou raycastingu,
    * ktory je miereny na stred obrazovky. Sip ma pridelenu fyziklanu silu, ktorou
    * leti.
    */
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && equippedBow && canShoot)
        {
            Vector3 targetPoint;
            Ray ray = cam.ViewportPointToRay(new Vector3(middleX, middleY, middleZ));
            canShoot = false;
            RaycastHit hit;

            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(75);
            }

            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            GameObject currentArrow = Instantiate(arrow, attackPoint.position, Quaternion.identity);
            currentArrow.transform.forward = directionWithoutSpread.normalized;
            currentArrow.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
            currentArrow.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);

            Invoke(nameof(ResetShoot), shootCooldownTime);
        }
    }

    /*
    * Resetovanie luku, aby hrac nemohol spawn shootovat. Po strelbe sa moznost 
    * strelby resetne.
    */
    void ResetShoot()
    {
        canShoot = true;
    }
}
