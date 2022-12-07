using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SpawnGround : MonoBehaviour
{
    public GameObject groundPart;
    public int prefabSize = 1;
    public int radius = 70;
    public float y = -0.9f;

    void Start()
    {
        for (int r = 0; r < radius; r++)
        {
            float length = 2 * MathF.PI * r/2;
            int prefabCount = (int)(Math.Ceiling(length / prefabSize)*1.2);
            for (int i = 0; i < prefabCount; i++)
            {

                var radians = 2 * MathF.PI / prefabCount * i;
                var vertical = MathF.Sin(radians);
                var horizontal = MathF.Cos(radians);

                var pos = new Vector3(horizontal, 0, vertical) * r/2;
                float randomSize = UnityEngine.Random.Range(0.1f, 1f);
                pos.x += 104;
                pos.y = y * randomSize;
                pos.z += 91;
                var instance = Instantiate(groundPart, pos, Quaternion.identity);
                instance.tag = "GroundPart";
                instance.layer = LayerMask.NameToLayer("GroundPart");

                BoxCollider collider = instance.AddComponent<BoxCollider>();
                collider.center = new Vector3(0f, (y - randomSize), 0f);
                collider.size = new Vector3(1f, 2.5f, 1f);
            }
        }
    }
}
