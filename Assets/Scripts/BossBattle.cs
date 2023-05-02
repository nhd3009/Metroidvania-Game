using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    private CameraController theCam;
    public Transform camPosition;
    public float camSpeed;

    public int threshhold1, threshhold2;

    public float activeTime, fadeoutTime, inactiveTime;
    private float activeCounter, fadeCounter, inactiveCounter;

    public Transform[] spawnPoints;
    private Transform targetPoint;
    public float moveSpeed;

    public Animator anim;

    public Transform theBoss;

    public float timeBetweenShot1, timeBetweenShot2;
    private float shotCounter;
    public GameObject bullet;
    public Transform shotPoint;

    public GameObject winObjects;

    private bool battleEnded;

    // Start is called before the first frame update
    void Start()
    {
        theCam = FindObjectOfType<CameraController>();
        theCam.enabled = false;

        activeCounter = activeTime;

        shotCounter = timeBetweenShot1;

        AudioManager.instance.PlayBossMusic();
    }

    // Update is called once per frame
    void Update()
    {
        theCam.transform.position = Vector3.MoveTowards(theCam.transform.position, camPosition.position, camSpeed * Time.deltaTime);

        if (!battleEnded)
        {
            if (BossHealthController.instance.currentHealth > threshhold1)
            {
                if (activeCounter > 0)
                {
                    activeCounter -= Time.deltaTime;

                    //Bien mat
                    if (activeCounter <= 0)
                    {
                        fadeCounter = fadeoutTime;
                        anim.SetTrigger("Vanish");
                    }

                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShot1;

                        Instantiate(bullet, shotPoint.position, Quaternion.identity);
                    }
                }
                else if (fadeCounter > 0)
                {
                    fadeCounter -= Time.deltaTime;
                    if (fadeCounter <= 0)
                    {
                        theBoss.gameObject.SetActive(false);
                        inactiveCounter = inactiveTime;
                    }
                }
                else if (inactiveCounter > 0)
                {
                    inactiveCounter -= Time.deltaTime;
                    if (inactiveCounter <= 0)
                    {
                        theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                        theBoss.gameObject.SetActive(true);

                        activeCounter = activeTime;

                        shotCounter = timeBetweenShot1;
                    }
                }
            }
            else
            {
                if (targetPoint == null)
                {
                    targetPoint = theBoss;
                    fadeCounter = fadeoutTime;
                    anim.SetTrigger("Vanish");
                }
                else
                {
                    if (Vector3.Distance(theBoss.position, targetPoint.position) > 0.02f)
                    {
                        theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);

                        //Bien mat
                        if (Vector3.Distance(theBoss.position, targetPoint.position) <= 0.02f)
                        {
                            fadeCounter = fadeoutTime;
                            anim.SetTrigger("Vanish");
                        }

                        shotCounter -= Time.deltaTime;
                        if (shotCounter <= 0)
                        {
                            if (PlayerHealthController.instance.currentHealth > threshhold2)
                            {
                                shotCounter = timeBetweenShot1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShot2;
                            }

                            Instantiate(bullet, shotPoint.position, Quaternion.identity);
                        }
                    }
                    else if (fadeCounter > 0)
                    {
                        fadeCounter -= Time.deltaTime;
                        if (fadeCounter <= 0)
                        {
                            theBoss.gameObject.SetActive(false);
                            inactiveCounter = inactiveTime;
                        }
                    }
                    else if (inactiveCounter > 0)
                    {
                        inactiveCounter -= Time.deltaTime;
                        if (inactiveCounter <= 0)
                        {
                            theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                            int whileBreaker = 0;

                            while (targetPoint.position == theBoss.position && whileBreaker < 100)
                            {
                                targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                                whileBreaker++;
                            }

                            theBoss.gameObject.SetActive(true);

                            if (PlayerHealthController.instance.currentHealth > threshhold2)
                            {
                                shotCounter = timeBetweenShot1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShot2;
                            }
                        }
                    }
                }
            }
        }
        //Boss ngỏm
        else
        {
            fadeCounter -= Time.deltaTime;
            if(fadeCounter < 0)
            {
                if(winObjects != null)
                {
                    winObjects.SetActive(true);
                    winObjects.transform.SetParent(null);
                }

                theCam.enabled = true;

                gameObject.SetActive(false);

                AudioManager.instance.PlayeLevelMusic();
            }
        }
    }

    public void EndBattle()
    {
        battleEnded = true;

        fadeCounter = fadeoutTime;
        anim.SetTrigger("Vanish");
        theBoss.GetComponent<Collider2D>().enabled = false;

        BossFireBall[] fireballs = FindObjectsOfType<BossFireBall>();

        if(fireballs.Length > 0)
        {
            foreach(BossFireBall fireball in fireballs)
            {
                Destroy(fireball.gameObject);
            }
        }
    }
}
