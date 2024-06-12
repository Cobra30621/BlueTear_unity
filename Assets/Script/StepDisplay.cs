using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class StepDisplay : MonoBehaviour
    {
        public Text infoText;

        [SerializeField] private Animator _animator;
        
        
        public void Show(string info)
        {
            infoText.text = info;

            _animator.SetTrigger("Show");
        }

        public void Hide()
        {
            _animator.SetTrigger("Hide");
        }

    }
}