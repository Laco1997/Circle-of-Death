using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public Slider health;
    Animator animator;
    public TMP_Text currentHealthText;

    //the HP Particle
    public GameObject HPParticle;

    //Default Forces
    public Vector3 DefaultForce = new Vector3(0f, 1f, 0f);
    public float DefaultForceScatter = 0.5f;

    void Start()
    {
        currentHealth = maxHealth;
        health.value = getPercentage();
        animator = GetComponent<Animator>();
    }

    public void damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.SetBool("isDead", true);
        }
        health.value = getPercentage();

        currentHealthText.text = currentHealth.ToString();

        int x = Screen.width / 2;
        int y = Screen.width / 2;

        Vector3 indicatorPosition = gameObject.transform.position;
        DamageIndicator(amount, indicatorPosition);

        //Debug.Log(currentHealth);
    }

    public void healthGained(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        health.value = getPercentage();

        currentHealthText.text = currentHealth.ToString();

        //Debug.Log(currentHealth);
    }

    public float getPercentage()
    {
        return (float)currentHealth / (float)maxHealth * 4000;

    }

    public bool isDead()
    {
        return currentHealth == 0;
    }

    //Change the HP and Instantiates an HP Particle with a Custom Force and Color
    public void DamageIndicator(float Delta, Vector3 Position)
    {
        Position.y += 12;
        GameObject NewHPP = Instantiate(HPParticle, Position, gameObject.transform.rotation) as GameObject;
        NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;
        NewHPP.transform.localScale = new Vector3(5f, 5f, 5f);

        TextMesh TM = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();

        TM.text = "-" + Delta.ToString();
        TM.color = new Color(1f, 0f, 0f, 1f);


        NewHPP.GetComponent<Rigidbody>().AddForce(new Vector3(DefaultForce.x + Random.Range(-DefaultForceScatter, DefaultForceScatter), DefaultForce.y + Random.Range(-DefaultForceScatter, DefaultForceScatter), DefaultForce.z + Random.Range(-DefaultForceScatter, DefaultForceScatter)));
    }

}
