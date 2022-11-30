using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public Slider bossHealth;
    Animator animator;
    public TMP_Text currentHealthText;

    void Start()
    {
        this.currentHealth = maxHealth;
        this.bossHealth.value = this.getPercentage();
        animator = GetComponent<Animator>();
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.damage(10);
        }
    }

}
