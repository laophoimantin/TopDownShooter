using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
	public static SceneController Instance { get; private set; }

	private bool _isLoading;

	[Header("Settings")]
	[SerializeField] private float _fadeDuration = 0.5f;
	[SerializeField] private bool _dontDestroyOnLoad = true;

	[Header("References")]
	[SerializeField] private EventDispatcher _eventDispatcher;

	[Header("Loading Screen")]
	[SerializeField] private GameObject _loadingScreen;
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private Slider _progressBar;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			if (_dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		if (_loadingScreen != null)
			_loadingScreen.SetActive(false);
		if (_canvasGroup != null)
		{
			_canvasGroup.blocksRaycasts = false;
			_canvasGroup.alpha = 0f;
			_canvasGroup.gameObject.SetActive(false);
		}
	}

	#region Public API

	/// Standard CurrentLevel Transition: Fades out, loads a new scene, fades in.
	public void LoadNewScene(string sceneName)
	{
		LoadScene(sceneName);
	}

	public void LoadGameplayScene()
	{
		LoadScene(SceneName.GAMEPLAY);
	}

	public void LoadMainMenu()
	{
		LoadScene(SceneName.MAIN_MENU);
	}

	public void ReloadCurrentScene()
	{
		LoadScene(SceneManager.GetActiveScene().name);
	}


	public void QuitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
	}

	#endregion


	#region Internal Logic

	private void LoadScene(string sceneName)
	{
		if (_isLoading) return;
		_isLoading = true;
		StartCoroutine(LoadSceneRoutine(sceneName));
	}


	private IEnumerator LoadSceneRoutine(string sceneName)
	{
		_canvasGroup.gameObject.SetActive(true);
		_canvasGroup.blocksRaycasts = true;

		// PHASE 1: TRANSITION TO LOADING SCREEN
		// =============================================================================
		yield return ScreenFader.FadeIn(_canvasGroup, _fadeDuration).WaitForCompletion();
		if (_loadingScreen != null) _loadingScreen.SetActive(true);
		yield return ScreenFader.FadeOut(_canvasGroup, _fadeDuration).WaitForCompletion();


		// PHASE 2: LOADING
		// =============================================================================

		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		_eventDispatcher.ClearAll();
		operation.allowSceneActivation = false; // Prevent auto-jumping

		// While loading...
		while (operation.progress < 0.9f)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			if (_progressBar != null)
				_progressBar.value = progress;
			yield return null;
		}

		if (_progressBar != null)
			_progressBar.value = 1f;

		yield return new WaitForSecondsRealtime(0.5f);

		// PHASE 3: TRANSITION TO NEW SCENE
		// =============================================================================

		yield return ScreenFader.FadeIn(_canvasGroup, _fadeDuration).WaitForCompletion();

		operation.allowSceneActivation = true;
		while (!operation.isDone)
			yield return null;

		if (_loadingScreen != null)
			_loadingScreen.SetActive(false);

		yield return ScreenFader.FadeOut(_canvasGroup, _fadeDuration).WaitForCompletion();

		_canvasGroup.blocksRaycasts = false;
		_canvasGroup.gameObject.SetActive(false);
		_isLoading = false;
	}

	#endregion
}

public static class SceneName
{
	public const string MAIN_MENU = "MainMenu";
	public const string GAMEPLAY = "Gameplay";
}