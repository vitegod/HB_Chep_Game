using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    
    [SerializeField] private Animator anim;
    private string currentAnimName;
    private float hp;
    public bool IsDead => hp <= 0;



    public virtual void OnInit() {

    }

    public virtual void OnDespawn() {

    }

    

    public void OnHit(float damage) {
        if (!IsDead) {
            
            hp -= damage;

            if (IsDead) {
                OnDeath();
            }
        }
    }

    public void OnDeath() {

    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
