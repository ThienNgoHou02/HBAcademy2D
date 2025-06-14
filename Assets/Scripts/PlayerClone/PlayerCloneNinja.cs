using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloneNinja : MonoBehaviour, IDamage
{
    private Animator animator;
    private string currentState;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        currentState = "isAppearing";
    }
    private void OnEnable()
    {
        SetState("isAppearing");
    }
    private void SetState(string state)
    {
        if (currentState != state)
        {
            animator.SetBool(currentState, false);
            currentState = state;
            animator.SetBool(currentState, true);
        }
    }
    public void Damage(float damage, float xPos, bool crit)
    {
        HitPool.instance.GetFromPool().transform.position = transform.position;

        GameObject damageText = DamageTextPool.instance.GetFromPool();
        damageText.transform.position = transform.position;
        int directon = xPos > transform.position.x ? -1 : 1;
        damageText.GetComponent<DamageText>().SetText($"-{damage}", Color.yellow, 12, directon);

        SetState("isHitting");
    }
}
