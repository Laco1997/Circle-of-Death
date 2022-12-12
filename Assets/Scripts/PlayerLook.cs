using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre pohyb pohladu hraca s mysou. Nastavenie sensitivity a zamedzenie 
* moznej rotacie (aby sa hrac nepreklopil).
*/
public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1000f;
    [SerializeField] private Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
