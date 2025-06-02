using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Home UI"; // 같이 불러올 씬 이름

    void Start()
    {
        // 현재 씬이 시작될 때 Additive로 다른 씬 로드
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
    }
}