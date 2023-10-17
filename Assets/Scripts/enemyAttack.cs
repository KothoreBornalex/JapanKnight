using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    public int degats = 1;
    public float cooldown = 0.5f;
    public Transform weapon;
    public float rayonAttack = 0.5f;

    private Collider2D[] colls;
    private bool reloading;
    private bool VUE;
    private Animator anim;
    private SpriteRenderer monSprite;
    private Vector3 weaponPos;

    public GameObject effect;
    private GameObject effectSave;
    private Vector2 direction;
    private float angleEffect;

    private Collider2D coll;

    private ennemiPatrol patPatrol;


    void Start() {
        anim = GetComponent<Animator>();
        monSprite = GetComponentInChildren<SpriteRenderer>();
        weaponPos = weapon.localPosition;
        coll = GetComponent<Collider2D>();
        patPatrol = GetComponent<ennemiPatrol>();
    }

    void Update() {
        if (monSprite.flipX) {
            weapon.localPosition = new Vector3(-weaponPos.x, weaponPos.y, 0);
        } else {
            weapon.localPosition = new Vector3(weaponPos.x, weaponPos.y, 0);
        }

        colls = Physics2D.OverlapCircleAll(weapon.position, rayonAttack);

        VUE = false;
        foreach (Collider2D truc in colls) {
            if (truc.tag == "Player") {
                patPatrol.enabled = false;
                if (!reloading) {
                    VUE = true;
                    reloading = true;
                    anim.SetTrigger("attack");
                    StartCoroutine(reload());
                }
                break;
            }
        }
        if(!VUE && patPatrol.enabled == false) {
            patPatrol.enabled = true;
        }
    }

    IEnumerator reload() {
        yield return new WaitForSeconds(cooldown);
        reloading = false;
    }

    public void PIF() {
        colls = Physics2D.OverlapCircleAll(weapon.position, rayonAttack);
        foreach (Collider2D truc in colls) {
            if (truc.tag == "Player") {
                truc.SendMessage("takeDamage", degats);
                if (effect != null) {
                    effectSave = Instantiate(effect, truc.ClosestPoint(coll.bounds.center), Quaternion.identity);
                    direction = truc.ClosestPoint(coll.bounds.center) - (Vector2)coll.bounds.center;
                    direction.Normalize();
                    angleEffect = Vector3.SignedAngle(transform.up, direction, Vector3.forward);
                    effectSave.transform.rotation = Quaternion.Euler(0, 0, -angleEffect);
                }

                Destroy(effectSave, 2f);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(weapon.position, rayonAttack);
    }
}
