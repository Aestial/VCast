using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class ToggleAnimatorBoolean : MonoBehaviour
{
    [SerializeField] string booleanName = default;
    Animator animator;

    public void SetAnimBool (bool value)
    {
        animator.SetBool(booleanName, value);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
