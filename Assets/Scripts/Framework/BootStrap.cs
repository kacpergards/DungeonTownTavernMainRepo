using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrap : MonoBehaviour
{
    public GameObject Player;
    public GameObject Framework;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(Framework);
        SceneManager.LoadScene("New Scene",LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
