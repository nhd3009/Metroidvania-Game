using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //Reload scene thi khong destroy gameobject
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*[HideInInspector]*/ public int currentHealth;
    public int maxHealth;

    public float invincibilityLength;
    private float invinciCounter;

    public float flashLength;
    public float flashCounter;

    public SpriteRenderer[] playerSprites;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(invinciCounter > 0)
        {
            invinciCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if(flashCounter <= 0)
            {
                foreach(SpriteRenderer sprite in playerSprites)
                {
                    sprite.enabled = !sprite.enabled;
                }

                flashCounter = flashLength;
            }

            //Nhan damage thi sprite se enable tro lai khong bi mat
            if(invinciCounter <= 0)
            {
                foreach (SpriteRenderer sprite in playerSprites)
                {
                    sprite.enabled = true;
                }
                flashCounter = 0f;
            }
        }
    }

    public void DamagePlayer(int dmgAmount)
    {
        if(invinciCounter <= 0)
        {
            currentHealth -= dmgAmount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                //gameObject.SetActive(false);

                RespawnController.instance.Respawn();

                AudioManager.instance.PlaySFX(8);
            }
            else
            {
                invinciCounter = invincibilityLength;

                AudioManager.instance.PlaySFXAdjusted(11);
            }

            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }

        
    }

    public void FillHealth()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}
