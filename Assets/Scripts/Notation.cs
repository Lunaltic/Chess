using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Notation : MonoBehaviour
{
    public Text moves;
    public Text move;

	private void Start()
	{
		for (int i = 0; i < GameManager.Instance.moves.Length; i++)
		{
			moves.text = GameManager.Instance.moves.Length.ToString() + ".";

			move.text = GameManager.Instance.moves[i];
		}

		
	}

}
