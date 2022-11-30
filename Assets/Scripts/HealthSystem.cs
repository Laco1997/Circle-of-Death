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
    public Slider bossHealth;
    Animator animator;
    public TMP_Text currentHealthText;

    //the HP Particle
    public GameObject HPParticle;

    //Default Forces
    public Vector3 DefaultForce = new Vector3(0f, 1f, 0f);
    public float DefaultForceScatter = 0.5f;

    void Start()
    {
        this.currentHealth = maxHealth;
        this.bossHealth.value = this.getPercentage();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.damage(10);
        }
    }

    public void damage(int amount)
    {
        this.currentHealth -= amount;
        if (this.currentHealth <= 0)
        {
            this.currentHealth = 0;
            animator.SetBool("isDead", true);
        }
        this.bossHealth.value = this.getPercentage();

        currentHealthText.text = this.currentHealth.ToString();

        CreateHPEffect(amount, this.gameObject.transform.position);

        Debug.Log(this.currentHealth);
    }

    public float getPercentage()
    {
        return (float)this.currentHealth / (float)this.maxHealth * 500;

    }

    public bool isDead()
    {
        return this.currentHealth == 0;
    }

    //Change the HP and Instantiates an HP Particle with a Custom Force and Color
    public void CreateHPEffect(float Delta, Vector3 Position)
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
