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

    private Image photo2Image;
    private SpriteRenderer photo2Renderer;

    public Texture otherTexture;

    // Start is called before the first frame update
    void Start()
    {
        photo1Image = transform.Find("PhotoPanel/Photo1").GetComponent<RawImage>();
        photo1Renderer = transform.Find("PhotoPanel/Photo1").GetComponent<CanvasRenderer>();

//        photo2Image = transform.Find("PhotoPanel/Photo1").GetComponent<Image>();
//        photo2Renderer = transform.Find("PhotoPanel/Photo1").GetComponent<SpriteRenderer>();

        Debug.Log("Photo1: " + photo1Image + " rend: " + photo1Renderer);
        HidePanel();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPanel()
    {
        photoPanel.SetActive(true);
        showPanel = true;
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
        Debug.Log("Adding photo " + photo.picture.width + " x " + photo.picture.height + " score: " + photo.score);
        Debug.Log("Canvas Renderer : " + photo1Renderer);

        photo1Image.color = Color.white;
        photo1Image.texture = photo.picture;


    }

}
