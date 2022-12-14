using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre vytvaranie kruhu v Lava World, ktori bude boss nicit.
*/
public class SpawnGround : MonoBehaviour
{
    public GameObject groundPart;
    public int radius = 70;
    public float y = 0.5f;
    public float randMin = 0.1f;
    public float randMax = 0.4f;

    void Start()
    {
        int prefabRadius = 3;
        int prefabDiameter = prefabRadius*2;
        for (int r = 0; r < radius*2; r += prefabDiameter)
        {
            // kazdy 0.5 radiu spawne objekt
            float rr = r / 2;
            float length = 2 * MathF.PI * rr/2;
            // vypocitam kolko v danom radiuse sa zmesti prefabov
            int prefabCount = (int)(Math.Ceiling(length / prefabRadius));
            for (int i = 0; i < prefabCount; i++)
            {
                var radians = 2 * MathF.PI / prefabCount * i;
                var vertical = MathF.Sin(radians);
                var horizontal = MathF.Cos(radians);

                var pos = new Vector3(horizontal, 0, vertical) * rr/2;
                // aby podlaha nebola plocha pridam nahodnu vychylku na y
                float randomY = UnityEngine.Random.Range(randMin, randMax);
                pos.x += transform.position.x;
                pos.y = y - randomY;
                pos.z += transform.position.z;
                var instance = Instantiate(groundPart, pos, Quaternion.identity);
                instance.tag = "GroundPart";
                instance.layer = LayerMask.NameToLayer("GroundPart");
                // upravim collider tak aby bola podlaha stale rovna a len vyzerala nerovno
                BoxCollider collider = instance.GetComponent<BoxCollider>();
                collider.center = new Vector3(collider.center.x, collider.center.y + randomY, collider.center.z);
            }
        }
    }
}
