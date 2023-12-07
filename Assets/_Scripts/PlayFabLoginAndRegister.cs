using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLoginAndRegister : MonoBehaviour
{
	[Header("Login and Register")]
	public TMP_InputField loginNameInput;

	[Header("Error Handling")]
	public GameObject errorPopUp;
	public TextMeshProUGUI errorMessage;

	private const string titleID = "CCE98";
	private const string password = "123456JHAIhsdkjshfdiashfla";

	public void PlayFabLogin()
	{
		var request = new LoginWithPlayFabRequest
		{
			Username = loginNameInput.text,
			Password = password,
			TitleId = titleID,
		};
		PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginError);
	}

	private void OnLoginError(PlayFabError error)
	{
		PlayFabRegister();
	}

	private void PlayFabRegister()
	{
		var request = new RegisterPlayFabUserRequest
		{
			Username = loginNameInput.text,
			Password = password,
			TitleId = titleID,
			DisplayName = loginNameInput.text,
			RequireBothUsernameAndEmail = false
		};
		PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucces, OnError);
	}

	void OnError(PlayFabError error)
	{
		Debug.Log("OnError Triggers: " + error.GenerateErrorReport());
		errorPopUp.SetActive(true);
		errorMessage.text = "Error: " + error.GenerateErrorReport();
	}

	void OnRegisterSucces(RegisterPlayFabUserResult result)
	{
		Debug.Log("Succesfull Register to PlayFab");
		PlayFabLogin();
	}

	void OnLoginSuccess(LoginResult result)
	{
		Debug.Log("Successfull Login to PlayFab");

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}