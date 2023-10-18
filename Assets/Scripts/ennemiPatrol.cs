using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemiPatrol : MonoBehaviour
{
    //METTRE CE SCRIPT SUR LES ENNEMIS

    [Range(0,50)] public float speed;                                            // Vitesse de déplacement ennemi
    [SerializeField, Range(0.1f, 50f)] private float limiteDroite = 1f; // distance entre l'ennemi et la limite de patrouille à droite (limité entre 0.1 et 50)
    [SerializeField, Range(0.1f, 50f)] private float limiteGauche = 1f; // distance entre l'ennemi et la limite de patrouille à gauche (limité entre 0.1 et 50)
    private Vector3 limiteDroitePosition;                               // Sert a transformer la distance avec la limite droite en coordonnées X/Y/Z
    private Vector3 limiteGauchePosition;                               // Sert a transformer la distance avec la limite gauche en coordonnées X/Y/Z
    private Rigidbody2D rb;                                             // Le rigidbody de l'ennemi
    private float direction = 1f;                                       // Direction vers laquelle l'ennemi se dirige (1 = droite, -1 = gauche)
    private SpriteRenderer skin;     
    
    private Animator animator;
    // Le sprite de l'ennemi, pour qu'on puisse le retourner quand il change de direction

    // Au lancement du jeu, on enregistre le rigidbody et le sprite de l'ennemi
    // On transforme aussi les valeurs de limite Droite et Gauche en coordonnées réelles
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        skin = GetComponentInChildren<SpriteRenderer>();

        limiteDroitePosition = transform.position + new Vector3(limiteDroite, 0, 0);
        limiteGauchePosition = transform.position - new Vector3(limiteGauche, 0, 0);
    }


    void Update() {
        // Si l'ennemi se coince contre quelque chose (sa vitesse plus petite que 0.1 m/s) alors il se retourne
        if (Mathf.Abs(rb.velocity.x) < 0.1f) {
            direction = -direction;
        }
        
        //Si il dépasse sa limite Droite, il se retourne
        if (transform.position.x > limiteDroitePosition.x) {
            direction = -1f;
            /*animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);*/
        }

        //Si il dépasse sa limite gauche, il se retourne
        if (transform.position.x < limiteGauchePosition.x) {
            direction = 1f;
            /*animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);*/
        }

        // Enfin on met le sprite dans le bon sens
        if (direction == 1f) {
            //skin.flipX = false;
        }

        if (direction == -1f) {
            //skin.flipX = true;
        }

        // Enfin on fait avancer l'ennemi dans la bonne direction
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

   
    //Cette fonction sert a visualiser le chemin de l'ennemi dans l'éditeur
    void OnDrawGizmos() {
        if (!Application.IsPlaying(gameObject)) {
            limiteDroitePosition = transform.position + new Vector3(limiteDroite, 0, 0);
            limiteGauchePosition = transform.position - new Vector3(limiteGauche, 0, 0);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawCube(limiteDroitePosition, new Vector3(0.2f, 1, 0.2f));
        Gizmos.DrawCube(limiteGauchePosition, new Vector3(0.2f, 1, 0.2f));
        Gizmos.DrawLine(limiteDroitePosition, limiteGauchePosition);
    }
}
