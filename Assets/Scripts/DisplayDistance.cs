using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDistance : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public Text distanceText;

    // Update is called once per frame
    void Update()
    {
        // Display only 3 decimal places of the distance ran
        distanceText.text = "Level: " + player.level + "\nDistance: " + player.distanceRan.ToString("F3");
    }
}
