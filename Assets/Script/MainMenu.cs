using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Jugar(){
        SceneManager.LoadScene("Level 1");
    }

    public void Salir(){
        Debug.Log("Saliendo del juego----");
        Application.Quit();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}