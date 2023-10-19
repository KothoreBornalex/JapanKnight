using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class projectileLight : MonoBehaviour
{
    // Script a mettre sur votre projectile que vous allez tirer depuis le script attackDIST
    // IL doit y avoir un TRIGGER sur l'objet et un rigidbody

    public int degats = 1;          // Les dégats du projectile
    private Transform player;
    private float timer;
    private Rigidbody2D rb;

    private void Start() {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // La fonction OnTriggerEnter s'enclenche quand votre Trigger touche un autre collider/trigger
    void OnTriggerEnter2D(Collider2D truc) {
        if (truc.tag == "Enemy") {                 // Si le truc qu'on touche a le tag "Enemy"
            truc.SendMessage("takeDamage", degats); // On cherche sur lui une fonction qui s'appel "takeDamage", et on la lance en lui donnant le nombre de dégat qu'on fait
            transform.parent = truc.transform;
            Destroy(gameObject);
        } 
        else if(truc.tag == "Lantern") {
            truc.SendMessage("ON");
            Destroy(gameObject);
        }
        
        else if (!truc.isTrigger && truc.tag != "Player") {     // Sinon si on touche un mur (un collider qui n'est PAS un trigger) et que ce n'est pas le joueur
            transform.parent = truc.transform;
            jeMeure();
        }
    }

    void jeMeure() {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        timer = Time.time;
    }

    private void Update() {
        if(Vector2.Distance(transform.position, player.position) > 20f) {
            Destroy(gameObject);
        }

    }
}
