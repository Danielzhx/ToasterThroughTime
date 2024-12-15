using UnityEngine;
using UnityEngine.UI;
using TarodevController;

public class ToastUIController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private Image toastIconImage;
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
