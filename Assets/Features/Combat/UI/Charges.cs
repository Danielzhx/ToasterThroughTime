using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Charges : MonoBehaviour
{
    public int currentCharges;
    public int totalCharges;

    public Image[] chargeImages;
    public Sprite fullCharge;
    public Sprite emptyCharge;

    // Duration for the blink effect
    private const float BlinkDuration = 1f;
    private const float BlinkInterval = 0.1f;

    void Update()
    {
        // Clamp currentCharges to valid range
        currentCharges = Mathf.Clamp(currentCharges, 0, totalCharges);

        for (int i = 0; i < chargeImages.Length; i++)
        {
            if (i < totalCharges)
            {
                chargeImages[i].enabled = true;
                chargeImages[i].sprite = i < currentCharges ? fullCharge : emptyCharge;
            }
            else
            {
                chargeImages[i].enabled = false;
            }
        }
    }

    public int ConsumeCharge()
    {
        if (currentCharges <= 0)
            return -1;

        int consumedIndex = currentCharges - 1;
        currentCharges--;
        StartCoroutine(BlinkCharge(consumedIndex));
        return consumedIndex;
    }

    public int AddCharge()
    {
        if (currentCharges >= totalCharges)
            return -1;

        int addedIndex = currentCharges;
        currentCharges++;
        StartCoroutine(BlinkCharge(addedIndex));
        return addedIndex;
    }

    public void AddCharges(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (currentCharges < totalCharges)
            {
                AddCharge();
            }
            else
            {
                Debug.Log("Maximum charges reached. Cannot add more charges.");
                break;
            }
        }
    }
    public IEnumerator BlinkCharge(int index)
    {
        if (index < 0 || index >= chargeImages.Length)
            yield break;

        Color originalColor = chargeImages[index].color;
        Color blinkColor = Color.yellow;
        float elapsedTime = 0f;
        bool toggle = false;

        while (elapsedTime < BlinkDuration)
        {
            chargeImages[index].color = toggle ? blinkColor : originalColor;
            toggle = !toggle;
            yield return new WaitForSeconds(BlinkInterval);
            elapsedTime += BlinkInterval;
        }

        // Ensure the charge ends with the correct color
        chargeImages[index].color = originalColor;
    }
}
