using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	public GameObject notationList;
	public GameObject notation;
	private void Awake()
	{
		Instance = this;
	}

	public void NewNotation()
	{
		Instantiate(notation, notationList.transform);
	}

	public GameObject panel;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			panel.SetActive(!panel.activeSelf);
		}
	}
}
