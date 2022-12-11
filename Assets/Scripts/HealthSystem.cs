using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public Slider health;
    Animator animator;
    public TMP_Text currentHealthText;

    // HP Particle
    public GameObject HPParticle;
    public Vector3 DefaultForce = new Vector3(0f, 1f, 0f);
    public float DefaultForceScatter = 0.5f;

    [SerializeField] private GameObject hudDamage;

    void Start()
    {
        currentHealth = maxHealth;
        health.value = getPercentage();
        animator = GetComponent<Animator>();
        hudDamage.SetActive(false);
    }

    public void damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (gameObject.tag != "Player")
            {
                animator.SetBool("isDead", true);
                SceneManager.LoadScene("Win");
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
        }
        health.value = getPercentage();

        currentHealthText.text = currentHealth.ToString();

        if (gameObject.tag != "Player")
        {
            Vector3 indicatorPosition = gameObject.transform.position;
            DamageIndicator(amount, indicatorPosition);
        }
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
    }

    public float getPercentage()
    {
        return ((float)currentHealth / (float)maxHealth) * maxHealth;

    }

    public bool isDead()
    {
        return currentHealth == 0;
    }

    public void DamageIndicator(float Delta, Vector3 Position)
    {
        Position.y += 12;
        GameObject NewHPP = Instantiate(HPParticle, Position, gameObject.transform.rotation) as GameObject;
        NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;
        NewHPP.transform.localScale = new Vector3(5f, 5f, 5f);

        TextMesh TM = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();

        TM.text = "-" + Delta.ToString();
        TM.color = new Color(1f, 0f, 0f, 1f);

        NewHPP.GetComponent<Rigidbody>().AddForce(new Vector3(DefaultForce.x + Random.Range(-DefaultForceScatter, DefaultForceScatter),
            DefaultForce.y + Random.Range(-DefaultForceScatter, DefaultForceScatter),
            DefaultForce.z + Random.Range(-DefaultForceScatter, DefaultForceScatter)));
    }

    public void TimedHitDamage(float duration)
    {
        StartCoroutine(DamageDuration(duration));
    }

    IEnumerator DamageDuration(float damageDuration)
    {
        ActivateDamageHUD();
        yield return new WaitForSeconds(damageDuration);
        DeactivateDamageHUD();
    }

    public void ActivateDamageHUD()
    {
        hudDamage.SetActive(true);
    }

    public void DeactivateDamageHUD()
    {
        hudDamage.SetActive(false);
    }

}
