using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour
{
    // Se lance quand on quitte un état d’animation
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        // On détruit le gameObject attaché à l’animator qu’on modifie (donc le canvas du loading screen)
        Destroy(animator.gameObject);
    }
}
