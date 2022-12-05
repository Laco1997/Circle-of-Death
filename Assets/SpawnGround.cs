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
                pos.y = y;
                var instance = Instantiate(groundPart, pos, Quaternion.identity);
                instance.tag = "GroundPart";
                instance.layer = LayerMask.NameToLayer("GroundPart");
            }
        }
        /*
       for (int x = 0; x < 5000; x++)
        {
            for (int z = 0; z < 5000; z++)
            {
                Vector3 pos = new Vector3(x, 0, z);
            }
        }*/
        /*for (int i = 0; i < 5000; i++) {
            Vector2 v2 = Random.insideUnitCircle * 100;
            Vector3 pos = new Vector3(v2.x, 0, v2.y);
            Instantiate(groundPart, pos, Quaternion.identity);
        }*/
    }
}
