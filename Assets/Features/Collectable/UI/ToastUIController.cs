using UnityEngine;
using UnityEngine.UI;
using TarodevController;

namespace TTT
{
    public class ToastUIController : MonoBehaviour
    {
        private Image toastIconImage;
        [SerializeField] private PlayerController player;
        [SerializeField] private Sprite normalToastSprite; 
        [SerializeField] private Sprite greyToastSprite; 

        void Start()
        {
            toastIconImage = GetComponent<Image>();
        }
        
        private void Update()
        {
            if (player == null || toastIconImage == null) return;

            if (player.hasToast)
                toastIconImage.sprite = normalToastSprite;
            else
                toastIconImage.sprite = greyToastSprite;
        }
    }
}

