using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static SceneLoader instance;

    private void Awake() 
	{
		instance = this;
	}
	
	public void LoadScene(Scene scene)
	{
		SceneManager.LoadScene(scene.name);
	}
	
	public void LoadScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}
	
	public void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
