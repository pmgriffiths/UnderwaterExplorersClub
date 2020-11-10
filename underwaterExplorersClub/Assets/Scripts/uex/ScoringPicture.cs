using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoringPicture 
{
    public bool showPanel = false;

    public Texture2D picture;

    public List<Fish> fishInPicture;

    public int score;


    // Start is called before the first frame update
    public ScoringPicture()
    {
        score = 0;
        fishInPicture = new List<Fish>();
    }

    public void ScorePhoto(Texture2D photo, Camera cameraToCheck, List<Fish> possibleFish)
    {
        picture = photo;

        int photoHeight = cameraToCheck.scaledPixelHeight;
        int photoWidth = cameraToCheck.scaledPixelWidth;
        foreach (Fish fish in possibleFish)
        {
            Vector3 pointInPicture = cameraToCheck.WorldToScreenPoint(fish.GetComponentInChildren<Renderer>().bounds.center);

            // In front: 
            if (pointInPicture.z > 0)
            {
                // in FOV
                if (pointInPicture.x >= 0 && pointInPicture.x <= photoWidth &&
                    pointInPicture.y >= 0 && pointInPicture.y <= photoHeight)
                {
                    // in the picture
                    fishInPicture.Add(fish);
                    score += fish.score;

                    Debug.Log("Found a fish in picture " + fish.name + " score: " + fish.score);
                }
            }
        }

    }


}
