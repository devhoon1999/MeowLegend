using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Home UI"; // ���� �ҷ��� �� �̸�

    void Start()
    {
        // ���� ���� ���۵� �� Additive�� �ٸ� �� �ε�
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
    }
}