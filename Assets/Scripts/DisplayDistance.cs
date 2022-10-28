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
        var distance = "0.0";
        if (player.distanceRan > 1000)
        {
            distance = player.distanceRan.ToString("F2");
        }
        else
        {
            distance = player.distanceRan.ToString("F3");
        }
        distanceText.text = "Level:         " + player.level + "\nDistance:   " + distance;
    }
}
