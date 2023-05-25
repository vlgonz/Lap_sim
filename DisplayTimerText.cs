using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DisplayTimerText : MonoBehaviour
{
    public float displayDuration = 5f;  // Duration in seconds to display the text

    private TextMeshProUGUI textComponent;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        StartCoroutine(DisplayTextForDuration());
    }

    private IEnumerator DisplayTextForDuration()
    {
        textComponent.enabled = true;  // Show the text

        yield return new WaitForSeconds(displayDuration);

        textComponent.enabled = false;  // Hide the text
    }
}

