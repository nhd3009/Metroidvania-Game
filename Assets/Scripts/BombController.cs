using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{

    public float timeToExplode = 0.5f;
    public GameObject explosion;

    public float blastRange;
    public LayerMask whatIsDestructible;

    public int dmgAmount;
    public LayerMask whatIsDamageable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToExplode -= Time.deltaTime;
        if(timeToExplode <= 0)
        {
            if(explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);

            }
            Destroy(gameObject);


            Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDestructible);

            if(objectsToDamage.Length > 0)
            {
                foreach(Collider2D col in objectsToDamage)
                {
                    Destroy(col.gameObject);
                }
            }

            Collider2D[] objectsToDmg = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDamageable);

            foreach(Collider2D collider in objectsToDmg)
            {
                EnemyHealthController enemyHealth = collider.GetComponent<EnemyHealthController>();
                if(enemyHealth != null)
                {
                    enemyHealth.DamageEnemy(dmgAmount);
                }
            }

            AudioManager.instance.PlaySFXAdjusted(4);
        }
    }
}
