using UnityEngine;

public class RunnerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MeshRenderer mRenderer;

    public void MoveAnimation(bool move)
    {
        animator.SetBool("Move", move);
    }

    public void SetColor(Material material)
    {
        mRenderer.material = material;
    }
}
