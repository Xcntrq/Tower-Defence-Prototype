using nsResourceGenerator;
using UnityEngine;

namespace nsAnimatorToggler
{
    public class AnimatorToggler : MonoBehaviour
    {
        [SerializeField] private ResourceGenerator _resourceGenerator;

        private Animator _animator;

        private void OnEnable()
        {
            _resourceGenerator.OnGetToWork += ResourceGenerator_OnGetToWork;
        }

        private void OnDisable()
        {
            _resourceGenerator.OnGetToWork -= ResourceGenerator_OnGetToWork;
        }

        private void ResourceGenerator_OnGetToWork(int nearbyResourceNodeCount)
        {
            _animator.speed = 1 + (float)(nearbyResourceNodeCount - 1) / 5;
            _animator.SetBool("working", true);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
    }
}
