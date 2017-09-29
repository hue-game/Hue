using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewTutorial : MonoBehaviour {

    public Sprite[] slides;
    public Image tutorialCanvasSlide;
    private bool touching = false;
    private int _currentSlide = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            touching = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
            touching = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            touching = false;
    }

    public void PlayerInteract()
    {
        if (touching)
        {
            ShowTutorial();
        }
    }

    public void ShowTutorial()
    {
        if (slides.Length > 0)
        {
            tutorialCanvasSlide.transform.parent.gameObject.SetActive(true);
            tutorialCanvasSlide.sprite = slides[_currentSlide];
        }
    }

    public void NextSlide()
    {
        if (_currentSlide == slides.Length - 1)
        {
            _currentSlide = 0;
            tutorialCanvasSlide.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _currentSlide++;
            tutorialCanvasSlide.sprite = slides[_currentSlide];
        }
    }

}
