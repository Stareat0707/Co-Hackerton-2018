using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Animator animator;
    public void Hit()
    {
        animator.SetBool("die", true);
        Destroy(this.gameObject, 0.8f);
    }
}
