using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectileEnnemi : MonoBehaviour
{

    // Script a mettre sur votre projectile que vous allez tirer depuis le script attackDIST
    // IL doit y avoir un TRIGGER sur l'objet et un rigidbody

    public int degats = 1;// Les d�gats du projectile

    private GameObject player;
    private Rigidbody2D rb;
    public float force;
   
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");


        Vector3 direction = player.transform.position - transform.position;//repr�sente la direction dans laquelle la fl�che doit se d�placer pour atteindre le joueur.
        rb.velocity = new Vector3(direction.x, direction.y).normalized * force;//r�cup�re notre vecteur de direction, pour cr�er vecteur velocit�

        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//calculer l'angle en radians entre les composantes y et x d'une direction donn�e.
                                                                          //multipli� par Mathf.Rad2Deg pour convertir l'angle de radians � degr�s
                                                                          //rotation sur l'axe z
        transform.rotation = Quaternion.Euler(0f, rot, 0);//cr�er une rotation autour de l'axe y (vertical) de l'objet.
                                                          //permet � la fl�che de faire face � la direction vers laquelle elle se d�place.

    }

    private void Update()
    {
        
    }

    // La fonction OnTriggerEnter s'enclenche quand votre Trigger touche un autre collider/trigger
    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.tag == "Player")
        {                 // Si le truc qu'on touche a le tag "Ennemi"
            /*truc.SendMessage("takeDamage", degats); // On cherche sur lui une fonction qui s'appel "takeDamage",(lifePlayer) et on la lance en lui donnant
                                                    // le nombre de d�gat qu'on fait
            */Destroy(gameObject);                   // Enfin on d�truit le projectile
        }


        else if (!truc.isTrigger && truc.tag != "Enemy")
        {     // Sinon si on touche un mur (un collider qui n'est PAS un trigger) et que ce n'est pas le joueur
            Destroy(gameObject);        // On d�truit simplement le projectile
        }

        else
        {
            Destroy(gameObject, 4f);
        }
    }

}
