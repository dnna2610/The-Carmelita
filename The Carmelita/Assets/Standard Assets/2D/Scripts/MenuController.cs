using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour {

	public Button startButton;

	// Use this for initialization
	void Start () {
		startButton = GetComponent<Button>();
	}
	
	public void StartPress(){
		SceneManager.LoadScene("level1");
	}
}
