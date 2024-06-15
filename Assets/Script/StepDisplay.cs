using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    /// <summary>
    /// 不同步驟提示文字
    /// </summary>
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