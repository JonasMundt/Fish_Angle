using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerSkinController : MonoBehaviour
{
    [Header("Animator Controller")]
    [SerializeField]
    private RuntimeAnimatorController defaultController;

    [SerializeField]
    private AnimatorOverrideController purpleController;

    [Header("Test")]
    private Animator animator;
    private bool usingPurpleSkin;
    public bool UsingPurpleSkin => usingPurpleSkin;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UseDefaultSkin()
    {
        if (!animator || !defaultController)
        {
            return;
        }

        animator.runtimeAnimatorController =
            defaultController;

        usingPurpleSkin = false;
    }

    public void UsePurpleSkin()
    {
        if (!animator || !purpleController)
        {
            return;
        }

        animator.runtimeAnimatorController =
            purpleController;

        usingPurpleSkin = true;
    }
}