using UnityEngine;
using UnityEngine.UI;

public class DialogueColourChanger : MonoBehaviour
{
    public Image uiElement; // Use Graphic type for any UI element (Text, Image, etc.)

    void Start()
    {
        // Make sure a Graphic component is attached to the GameObject
        if (uiElement == null)
        {
            uiElement = GetComponent<Image>();
            if (uiElement == null)
            {
                Debug.LogError("Graphic component not found!");
                return;
            }
        }


        // Call the method to change color based on the character
        ChangeColorBasedOnCharacter('A');
    }

    void Update()
    {
        // For testing purposes, you can change the character during runtime
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeColorBasedOnCharacter('B');
        }
    }

    void ChangeColorBasedOnCharacter(char character)
    {
        Color newColor;

        // Switch case to assign different colors based on characters
        switch (character)
        {
            case 'A':
                uiElement.GetComponent<Image>().color = new Color32(236, 146, 160, 255);

                break;
            case 'B':
                uiElement.GetComponent<Image>().color = new Color32(146, 225, 236, 255);
                break;
            case 'C':
                newColor = Color.blue;
                break;
            default:
                newColor = Color.white;
                break;
        }

       
    }
}