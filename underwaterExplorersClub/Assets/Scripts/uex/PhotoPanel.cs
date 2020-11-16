using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhotoPanel : MonoBehaviour
{
    public bool showPanel = false;

    public GameObject photoPanel;

    private RawImage photo1Image;
    private CanvasRenderer photo1Renderer;
    private Text photo1Description;

    private RawImage photo2Image;
    private CanvasRenderer photo2Renderer;
    private Text photo2Description;

    private RawImage photo3Image;
    private CanvasRenderer photo3Renderer;
    private Text photo3Description;

    private RawImage newImage;
    private CanvasRenderer newImageRenderer;
    private Text newPhotoDescription;
    private Text newScore;

    ScoringPicture lastPicture;

    ScoringPicture photo1;
    ScoringPicture photo2;
    ScoringPicture photo3;

    public Texture otherTexture;

    // Start is called before the first frame update
    void Start()
    {
        photo1Image = transform.Find("PhotoPanel/Photo1").GetComponent<RawImage>();
        photo1Renderer = transform.Find("PhotoPanel/Photo1").GetComponent<CanvasRenderer>();
        photo1Description = transform.Find("PhotoPanel/Photo1/Desc1").GetComponent<Text>();

        photo2Image = transform.Find("PhotoPanel/Photo2").GetComponent<RawImage>();
        photo2Renderer = transform.Find("PhotoPanel/Photo2").GetComponent<CanvasRenderer>();
        photo2Description = transform.Find("PhotoPanel/Photo2/Desc2").GetComponent<Text>();

        photo3Image = transform.Find("PhotoPanel/Photo3").GetComponent<RawImage>();
        photo3Renderer = transform.Find("PhotoPanel/Photo3").GetComponent<CanvasRenderer>();
        photo3Description = transform.Find("PhotoPanel/Photo3/Desc3").GetComponent<Text>();

        newImage = transform.Find("PhotoPanel/NewPhoto").GetComponent<RawImage>();
        newImageRenderer = transform.Find("PhotoPanel/NewPhoto").GetComponent<CanvasRenderer>();
        newPhotoDescription = transform.Find("PhotoPanel/NewPhoto/NewDesc").GetComponent<Text>();
        newScore = transform.Find("PhotoPanel/NewPhoto/NewScore").GetComponent<Text>();


        HidePanel();

    }

    // Update is called once per frame
    void Update()
    {

        if (showPanel)
        {
            if (Input.GetKeyDown("1"))
            {
                OnClickSave1();
            }
            if (Input.GetKeyDown("2"))
            {
                OnClickSave2();
            }
            if (Input.GetKeyDown("3"))
            {
                OnClickSave3();
            }
        }

    }

    public void ShowPanel()
    {
        photoPanel.SetActive(true);
        ShowPictures();

        showPanel = true;
    }

    private void ShowPictures()
    {
        if (photo1 != null)
        {
            photo1Image.color = Color.white;
            photo1Image.texture = photo1.picture;
            photo1Description.text = "Score: " + photo1.GetScore();
        }
        else
        {
            photo1Image.color = Color.black;
            photo1Description.text = "Score: ";
        }

        if (photo2 != null)
        {
            photo2Image.color = Color.white;
            photo2Image.texture = photo2.picture;
            photo2Description.text = "Score: " + photo2.GetScore();
        }
        else
        {
            photo2Image.color = Color.black;
            photo2Description.text = "Score: ";
        }

        if (photo3 != null)
        {
            photo3Image.color = Color.white;
            photo3Image.texture = photo3.picture;
            photo3Description.text = "Score: " + photo3.GetScore();

        }
        else
        {
            photo3Image.color = Color.black;
            photo3Description.text = "Score: ";
        }

    }

    public void HidePanel()
    {
        photoPanel.SetActive(false);
        showPanel = false;
    }

    public void TogglePanel()
    {
        if (showPanel)
        {
            HidePanel();
        } else
        {
            ShowPanel();
        }
    }

    public void AddPhoto(ScoringPicture photo)
    {
        Debug.Log("Adding photo " + photo.picture.width + " x " + photo.picture.height + " score: " + photo.GetScore());
        Debug.Log("Canvas Renderer : " + photo1Renderer);

        lastPicture = photo;
        newImage.color = Color.white;
        newImage.texture = photo.picture;
        newPhotoDescription.text = photo.GetFishNames();
        newScore.text = "" + photo.GetScore();

    }

    public void OnClickSave1()
    {
        photo1 = lastPicture;
        ShowPictures();
    }

    public void OnClickSave2()
    {
        photo2 = lastPicture;
        ShowPictures();
    }

    public void OnClickSave3()
    {
        photo3 = lastPicture;
        ShowPictures();
    }
}
