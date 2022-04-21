using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
	private const int SIZE = 19;

	public GameObject btnPrefab;
	public Sprite btnBackground;
	public Sprite[] playerPieces;

	private int[,] board = new int[SIZE, SIZE];
	private int playerCount = 2;
	private int currentPlayer = 1;

	private void Start()
	{
		GetComponent<GridLayoutGroup>().cellSize = Vector2.one * (900.0f / SIZE);

		for (int y = 0; y < SIZE; ++y)
		{
			for (int x = 0; x < SIZE; ++x)
			{
				int tempX = x, tempY = y;
				Instantiate(btnPrefab, transform).GetComponent<Button>().onClick.AddListener(new UnityAction(() => ClickCell(tempX, tempY)));
			}
		}
	}

	public void ResetGame()
	{
		currentPlayer = 1;

		for (int y = 0; y < SIZE; ++y)
		{
			for (int x = 0; x < SIZE; ++x)
			{
				board[x, y] = 0;
				transform.GetChild(y * SIZE + x).GetComponent<Image>().sprite = btnBackground;
			}
		}
	}

	public void ClickCell(int x, int y)
	{
		if (board[x, y] == 0)
		{
			board[x, y] = currentPlayer;
			transform.GetChild(y * SIZE + x).GetComponent<Image>().sprite = playerPieces[currentPlayer - 1];

			//CHECK FOR LOGIC


			if (++currentPlayer > playerCount) { currentPlayer = 1; }
		}
	}
}
