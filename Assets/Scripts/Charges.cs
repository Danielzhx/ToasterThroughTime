using UnityEngine;
using UnityEngine.UI;

public class Charges : MonoBehaviour
{
    public int currentCharges;
    public int totalCharges;

    public Image[] charges;
    public Sprite fullCharge;
    public Sprite emptyCharge;

    void Update()
    {
        currentCharges = Mathf.Clamp(currentCharges, 0, totalCharges);

        for (int i = 0; i < charges.Length; i++)
        {
            if (i < totalCharges)
            {
                charges[i].enabled = true;
                charges[i].sprite = i < currentCharges ? fullCharge : emptyCharge;
            }
            else
            {
                charges[i].enabled = false;
            }
        }
    }
}