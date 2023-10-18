using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackDistLight : MonoBehaviour
{
    public int degats = 1;
    public Transform weapon;                    // L'object qui va tirer votre projectile, doit �tre en enfant de votre personnage (exemple : Un pistolet)
    private Vector3 positionWeapon;             // Les coordonn�es de l'arme, nous servira a positionner correctement l'arme quand on regarde a gauche
    public GameObject projectil;                // Le prefab du projectile que l'on tir, on doit glisser dans cette case un prefab avec un trigger ET un rigidbody2D
    private GameObject projectilSave;           // Une sauvegarde temporaire du projectile tir� pour lui apport� quelques modification quand on l'invoque

    public float speedProjectil = 10f;           // La vitesse de d�placement de notre projectile (valeur de base = 1)

    public float reloadTime = 0.5f;             // Le temps de chargement entre 2 tirs (valeur de base = 0.5)
    private bool reloading;                     // Bool�en qui devient vrai le temps qu'on recharge

    private Vector3 mousePos;                   // Vector3 pour stocker la position de la souris
    private Vector3 direction;                  // Vector3 pour calculer la direction du projectile
    private float angleProjectil;               // rotation que devra avoir projectile pour "regarder" dans la direction ou il va

    private SpriteRenderer skin;                // Le sprite du joueur, on va s'en servir pour savoir si il regarde � gauche ou a droite

    private Animator anim;                      // L'animator du joueur, �a nous permettra de lancer l'animation d'attaque quand on va tirer

    public manaPlayer manaScript;
    void Start() {
        skin = GetComponent<SpriteRenderer>();  // On r�cup�re le sprite renderer du personnage (pour savoir dans quelle direction il regarde)
        anim = GetComponent<Animator>();        // On r�cup�re son animator pour lancer l'animation d'attaque
        positionWeapon = weapon.localPosition;  // On enregistre la position local de l'arme, on l'utilisera pour retourner l'arme quand le joueur regarde � gauche
    }

    void Update() {

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // On r�cup�re la position de la souris et on transforme �a en coordonn�e locale dans le jeu
        mousePos.z = transform.position.z;// On dit que la position en Z de la souris est �gale � celle du joueur
                                          // (pour �tre sur que le projectile sera sur le m�me plan que le personnage)

        direction = mousePos - weapon.position;// Cacule basique d'une direction : (position de votre cible) - (votre position) =  direction entre vous et la cible
        direction.Normalize();// On donne � la direction une longueur de 1 m�tre, plus simple pour faire nos futurs calculs

        angleProjectil = Vector3.SignedAngle(transform.right, direction, Vector3.forward); // Maintenant qu'on a la direction de la souris,
                                                                                           // on calcul l'angle entre cette direction et la droite du joueur
                                                                                           // (Vector3.forward est la direction "Z" du jeu, pour sp�cifier l'axe de rotation autour duquel l'angle est mesur�.)
        weapon.rotation = Quaternion.Euler(0, 0, angleProjectil);// On fait pivoter l'arme dans la direction de la souris grace � l'angle qu'on vient de calculer.

        if (!skin.flipX) {
            weapon.localPosition = positionWeapon;      // Si on regarde a droite, l'arme prend sa position de base
        }

        if (skin.flipX) {
            weapon.localPosition = new Vector3(-positionWeapon.x, positionWeapon.y, 0);    // Si on regarde a gauche, on inverse la position X (pas Y) de l'arme
        }


        // Bien maintenant qu'on connait dans quelle direction le joueur vise, on check si le joueur appuis sur son bouton de tir (ici clic-droit) ET qu'il n'est pas entrain de recharger (reloading = false)
        if (Input.GetButton("Fire2") && !reloading && manaScript.currentMana >= manaScript.manaCostPerShot) {

            reloading = true;// On passe directement reloading en vrai histoire de ne pas pouvoir tirer 2 fois de suite
            projectilSave = Instantiate(projectil, weapon.position, Quaternion.Euler(0, 0, angleProjectil));// on fait apparaitre le projectile,
                                                      // sur la position de votre arme (weapon) et pivoter avec l'angle qu'on a calcul� plus haut

            projectilSave.GetComponent<Rigidbody2D>().velocity = direction * speedProjectil;// Et on fait avancer le projectile dans la direction qu'on a calcul�
                                                                                            // plut�t
            projectilSave.GetComponent<projectileLight>().degats = degats;
            StartCoroutine(waitShoot());

            manaScript.currentMana -= manaScript.manaCostPerShot;
            Debug.Log(manaScript.currentMana);
        }
    }

    // Voici la coroutine waitShoot
    IEnumerator waitShoot() {
        yield return new WaitForSeconds(reloadTime); // La on dit au script de patienter pendant un certain temps (reloadTime)
        reloading = false;                           // On a fini d'attendre donc on repasse reloading en faux, donc on va pouvoir tirer � nouveau
    }
}
