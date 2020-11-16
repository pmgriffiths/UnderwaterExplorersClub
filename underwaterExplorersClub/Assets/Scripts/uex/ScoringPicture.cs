using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoringPicture 
{
    public bool showPanel = false;

    public Texture2D picture;

    public List<Fish> fishInPicture;

    private string fishNames;

    private int score;


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

        fishNames = "";
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
                    fishNames += fish.name + " ";

                    score += fish.score;

                    Debug.Log("Found a fish in picture " + fish.fishName + " score: " + fish.score);
                }
            }
        }

    }

    public string GetFishNames()
    {
        return fishNames;
    }

    public int GetScore()
    {
        return score;
    }

}
