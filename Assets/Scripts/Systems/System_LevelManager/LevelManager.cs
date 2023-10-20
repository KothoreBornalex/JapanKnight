using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private Slider _loadingBarr;
    [SerializeField] private Animator _loadingAnimator;

    private float target;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public async void LoadScene(string sceneName)
    {   
        target = 0;
        _loadingBarr.value = 0;
        _loadingCanvas.SetActive(true);




        AudioManager.instance.SetMusic(MusicsEnum.LoadingScreenMusic);





        //Make the fade In appear and wait before it is done.
        _loadingAnimator.Play("FadeIn");
        await Task.Delay(2500);


        //Desactivating the main camera during the loading screen.
        CameraManager.instance._camera.enabled = false;


        //Switching to the loading screen scene.
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScreen");


        //Activating the loading barr.
        _loadingBarr.gameObject.SetActive(true);

        //Make the fade Out appear and wait before it is done.
        _loadingAnimator.Play("FadeOut");
        await Task.Delay(2500);

        //Loading the targeted scene.
        var scene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        do
        {
            await Task.Delay(100);
            target = scene.progress;
        } while (scene.progress < 0.9f);
        await Task.Delay(2500);


        //Make the fade In appear and wait before it is done.
        _loadingAnimator.Play("FadeIn");
        await Task.Delay(2500);

        //Activating the main camera during the loading screen.
        CameraManager.instance._camera.enabled = true;


        //Activating the scene Switch.
        scene.allowSceneActivation = true;
        _loadingBarr.gameObject.SetActive(false);

        AudioManager.instance.SetMusic(MusicsEnum.BaseMusic);

        //Make the fade Out appear and wait before it is done.
        _loadingAnimator.Play("FadeOut");

        await Task.Delay(2500);
        _loadingCanvas.SetActive(false);

    }

    void Update()
    {
        _loadingBarr.value = Mathf.MoveTowards(_loadingBarr.value, target, Time.deltaTime);
    }



}
