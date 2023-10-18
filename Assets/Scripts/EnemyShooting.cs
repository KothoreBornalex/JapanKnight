using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    public GameObject projectile;
    public Transform projectileLaucher;

    private float reload;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        

        if (distance < 20)
        {
            reload -= Time.deltaTime;
            if (reload <= 0)
            {
                Instantiate(projectile, projectileLaucher.position, Quaternion.identity);
                reload = 1f;
            }
        }
    }
}
