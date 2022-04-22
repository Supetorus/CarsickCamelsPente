using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
	[SerializeField] IntData playerCount;

	private const int SIZE = 19;

	public GameObject btnPrefab;
	public Sprite btnBackground;
	public Sprite[] playerPieces;

	private int[,] board = new int[SIZE, SIZE];
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

			Capture(x, y);
			CheckWin(x, y);

			if (++currentPlayer > playerCount.value) { currentPlayer = 1; }
		}
	}

	private void CheckWin(int x, int y)
	{
		bool[] flags = { true, true, true, true, true, true, true, true };
		int highest = 0;

		for(int i = 1; i < 5; ++i)
		{
			if (x - i >= 0)
			{
				if(flags[0] && board[x - i, y] == currentPlayer) { highest = i; }
				else { flags[0] = false; }

				if (flags[1] && y - i >= 0 && board[x - i, y - i] == currentPlayer) { highest = i; }
				else { flags[1] = false; }

				if (flags[2] && y + i < SIZE && board[x - i, y + i] == currentPlayer) { highest = i; }
				else { flags[2] = false; }
			}

			if (x + i < SIZE)
			{
				if (flags[3] && board[x + i, y] == currentPlayer) { highest = i; }
				else { flags[3] = false; }

				if (flags[4] && y - i >= 0 && board[x + i, y - i] == currentPlayer) { highest = i; }
				else { flags[4] = false; }

				if (flags[5] && y + i < SIZE && board[x + i, y + i] == currentPlayer) { highest = i; }
				else { flags[5] = false; }
			}

			if(flags[6] && y - i >= 0 && board[x, y - i] == currentPlayer) { highest = i; }
			else { flags[6] = false; }

			if (flags[7] && y + i >= 0 && board[x, y + i] == currentPlayer) { highest = i; }
			else { flags[7] = false; }
		}

		if(highest == 2) { print("Three"); }
		if(highest == 3) { print("Four"); }
		if(highest == 4) { print("YOU WIN!!11!!1"); }
	}

	private void Capture(int x, int y)
	{
		//I will hopefully refactor this garbage code
		if (x > 2)
		{
			if (board[x - 1, y] == board[x - 2, y] && board[x - 1, y] != board[x - 3, y] && board[x - 3, y] > 0)
			{
				board[x - 1, y] = 0;
				transform.GetChild(y * SIZE + x - 1).GetComponent<Image>().sprite = btnBackground;
				board[x - 2, y] = 0;
				transform.GetChild(y * SIZE + x - 2).GetComponent<Image>().sprite = btnBackground;
			}

			if (y > 2 && board[x - 1, y - 1] == board[x - 2, y - 2] && board[x - 1, y - 1] != board[x - 3, y - 3] && board[x - 3, y - 3] > 0)
			{
				board[x - 1, y - 1] = 0;
				transform.GetChild((y - 1) * SIZE + x - 1).GetComponent<Image>().sprite = btnBackground;
				board[x - 2, y - 2] = 0;
				transform.GetChild((y - 2) * SIZE + x - 2).GetComponent<Image>().sprite = btnBackground;
			}

			if (y < SIZE - 3 && board[x - 1, y + 1] == board[x - 2, y + 2] && board[x - 1, y + 1] != board[x - 3, y + 3] && board[x - 3, y + 3] > 0)
			{
				board[x - 1, y + 1] = 0;
				transform.GetChild((y + 1) * SIZE + x - 1).GetComponent<Image>().sprite = btnBackground;
				board[x - 2, y + 2] = 0;
				transform.GetChild((y + 2) * SIZE + x - 2).GetComponent<Image>().sprite = btnBackground;
			}
		}

		if (x < SIZE - 3)
		{
			if (board[x + 1, y] == board[x + 2, y] && board[x + 1, y] != board[x + 3, y] && board[x + 3, y] > 0)
			{
				board[x + 1, y] = 0;
				transform.GetChild(y * SIZE + x + 1).GetComponent<Image>().sprite = btnBackground;
				board[x + 2, y] = 0;
				transform.GetChild(y * SIZE + x + 2).GetComponent<Image>().sprite = btnBackground;
			}

			if (y > 2 && board[x + 1, y - 1] == board[x + 2, y - 2] && board[x + 1, y - 1] != board[x + 3, y - 3] && board[x + 3, y - 3] > 0)
			{
				board[x + 1, y - 1] = 0;
				transform.GetChild((y - 1) * SIZE + x + 1).GetComponent<Image>().sprite = btnBackground;
				board[x + 2, y - 2] = 0;
				transform.GetChild((y - 2) * SIZE + x + 2).GetComponent<Image>().sprite = btnBackground;
			}

			if (y < SIZE - 3 && board[x + 1, y + 1] == board[x + 2, y + 2] && board[x + 1, y + 1] != board[x + 3, y + 3] && board[x + 3, y + 3] > 0)
			{
				board[x + 1, y + 1] = 0;
				transform.GetChild((y + 1) * SIZE + x + 1).GetComponent<Image>().sprite = btnBackground;
				board[x + 2, y + 2] = 0;
				transform.GetChild((y + 2) * SIZE + x + 2).GetComponent<Image>().sprite = btnBackground;
			}
		}

		if (y > 2 && board[x, y - 1] == board[x, y - 2] && board[x, y - 1] != board[x, y - 3] && board[x, y - 3] > 0)
		{
			board[x, y - 1] = 0;
			transform.GetChild((y - 1) * SIZE + x).GetComponent<Image>().sprite = btnBackground;
			board[x, y - 2] = 0;
			transform.GetChild((y - 2) * SIZE + x).GetComponent<Image>().sprite = btnBackground;
		}

		if (y < SIZE - 3 && board[x, y + 1] == board[x, y + 2] && board[x, y + 1] != board[x, y + 3] && board[x, y + 3] > 0)
		{
			board[x, y + 1] = 0;
			transform.GetChild((y + 1) * SIZE + x).GetComponent<Image>().sprite = btnBackground;
			board[x, y + 2] = 0;
			transform.GetChild((y + 2) * SIZE + x).GetComponent<Image>().sprite = btnBackground;
		}
	}
}
