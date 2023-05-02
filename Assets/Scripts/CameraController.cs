using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController player;
    public BoxCollider2D boundBox;

    private float halfHeight, halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        AudioManager.instance.PlayeLevelMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, boundBox.bounds.min.x + halfWidth, boundBox.bounds.max.x - halfWidth),
                Mathf.Clamp(player.transform.position.y, boundBox.bounds.min.y + halfHeight, boundBox.bounds.max.y - halfHeight), 
                transform.position.z);
        }
        else
        {
            player = FindObjectOfType<PlayerController>();
        }
    }
}
