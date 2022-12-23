using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainIntroDialog : MonoBehaviour 

{
    // Start is called before the first frame update
    public GameObject contentImageHolder;
    public UnityEngine.UI.Image contentImageComponent;
    public Sprite newImage;

    void Start()
    {
        // contentImageComponent = contentImageHolder.GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(LoadNextLevel(2.0f));   
    }


    IEnumerator LoadNextLevel(float delay) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("FirstLevel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
