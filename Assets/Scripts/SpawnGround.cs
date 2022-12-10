using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            float rr = r / 2;
            float length = 2 * MathF.PI * rr/2;
            int prefabCount = (int)(Math.Ceiling(length / prefabRadius));
            //prefabCount = 5;
            for (int i = 0; i < prefabCount; i++)
            {

                var radians = 2 * MathF.PI / prefabCount * i;
                var vertical = MathF.Sin(radians);
                var horizontal = MathF.Cos(radians);

                var pos = new Vector3(horizontal, 0, vertical) * rr/2;
                float randomY = UnityEngine.Random.Range(randMin, randMax);
                pos.x += transform.position.x;
                pos.y = y - randomY; // randomSize
                pos.z += transform.position.z;
                var instance = Instantiate(groundPart, pos, Quaternion.identity);
                instance.tag = "GroundPart";
                instance.layer = LayerMask.NameToLayer("GroundPart");

                BoxCollider collider = instance.GetComponent<BoxCollider>();
                collider.center = new Vector3(collider.center.x, collider.center.y + randomY, collider.center.z);
            }
        }
    }
}
