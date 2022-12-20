using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainIntroDialog : MonoBehaviour 

{
    // Start is called before the first frame update
    public GameObject contentImageHolder;
    public UnityEngine.UI.Image contentImageComponent;
    public Sprite newImage;

    void Start()
    {
        contentImageComponent = contentImageHolder.GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(ChangeImage(3.0f));
    }

    IEnumerator ChangeImage(float delay) {
        yield return new WaitForSeconds(delay);

        contentImageComponent.sprite = newImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
