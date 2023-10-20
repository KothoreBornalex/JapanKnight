using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSystem : MonoBehaviour
{
    public class EnemyInstance
    {
        public bool isDead;
        public GameObject enemyInstance;
    }

    public List<EnemyInstance> enemies = new List<EnemyInstance>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
