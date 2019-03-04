using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button_1 : MonoBehaviour
{
    //public Button button1;

    void Start()
    {
        //button1.onClick.AddListener(click);
    }

    public void testing()
    {
        print("Button_1");
        Debug.Log("Button_1");

    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) {
            
            SceneManager.LoadScene("GameUIScene");

        }
    }



}
