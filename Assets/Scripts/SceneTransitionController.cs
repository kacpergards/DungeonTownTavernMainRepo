using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{

    private GameObject player;
    public GameObject loadingScreen;
    public string transitionDoorName;
    private bool firstTimeLoad = true;

    void Awake()
    {
        DontDestroyOnLoad(GameObject.Find("Framework"));
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private IEnumerator LoadingScreen()
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        loadingScreen.SetActive(false);
        

    }
    //public void TransitionScene(String sceneName, Vector3 position) {
    //    if (playerPositions.ContainsKey(SceneManager.GetActiveScene().name)) {
    //        playerPositions[SceneManager.GetActiveScene().name] = position;
    //    } else {
    //        playerPositions.Add(SceneManager.GetActiveScene().name, position);
    //    }
    //    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    //}
    public void transitionDoor(String DoorName, string whichScene)
    {
        transitionDoorName = DoorName;
        SceneManager.LoadScene(whichScene, LoadSceneMode.Single);
    }

    public void setPlayerPosition()
    {
        
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        //need to make it so that the screen is blacked out for like 3 seconds before you load into scene to hide camer jankines
        StartCoroutine(LoadingScreen());
        if (SceneManager.GetActiveScene().name != "LoadingScene" && !firstTimeLoad)
        {
            GameObject DoorToSpawnAt = GameObject.Find(transitionDoorName);
            DoorToSpawnAt.GetComponent<Door>().isEnabled = false;
            player = GameObject.Find("Player");
            player.transform.position = DoorToSpawnAt.transform.position;
        }

        if (firstTimeLoad && SceneManager.GetActiveScene().name != "LoadingScene")
        {
            firstTimeLoad = false;
        }


    }
}

