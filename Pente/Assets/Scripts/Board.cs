using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Board : MonoBehaviour
{
	private const int SIZE = 19;
	private Vector2Int[] directions = {
		Vector2Int.up,
		Vector2Int.down,
		Vector2Int.left,
		Vector2Int.right,
		Vector2Int.up + Vector2Int.left,
		Vector2Int.up + Vector2Int.right,
		Vector2Int.down + Vector2Int.left,
		Vector2Int.down + Vector2Int.right,
	};

	public GameObject btnPrefab;
	public Sprite btnBackground;
	public IntData playerCount;
	public StringData[] playerNames;
	public EnumData[] playerColors;
	public Sprite[] availablePieces;

	public StringData timeString;

	private float turnTimer = 30.0f;
	private bool gameStart = false;

	private int[,] board = new int[SIZE, SIZE];
	private int currentPlayer = 0;

	private List<Player> players = new List<Player>();
	private List<Player> currentPlayers;

	private void Start()
	{

	}

	public void SetupGame()
	{
		for (int i = 0; i < playerCount.value; ++i)
		{
			players.Add(new Player(availablePieces[playerColors[i].value], playerNames[i].value, i));
		}

		GetComponent<GridLayoutGroup>().cellSize = Vector2.one * (900.0f / SIZE);

		for (int y = 0; y < SIZE; ++y)
		{
			for (int x = 0; x < SIZE; ++x)
			{
				int tempX = x, tempY = y;
				Instantiate(btnPrefab, transform).GetComponent<Button>().onClick.AddListener(new UnityAction(() => ClickCell(tempX, tempY)));
			}
		}

		ResetGame();
	}

	private void Update()
	{
		if (gameStart)
		{
			turnTimer -= Time.deltaTime;
			timeString.value = turnTimer.ToString("F1"); 
			//print(timeString.value);
		
			


			if (turnTimer < 0)
			{
				if (++currentPlayer > playerCount.value) { currentPlayer = 1; }
				turnTimer = 30.0f;

		

			}
		}
	}

	public void ResetGame()
	{
		Debug.ClearDeveloperConsole();
		currentPlayers = players;
		currentPlayer = 0;
		turnTimer = 30.0f;
		gameStart = true;

		for (int y = 0; y < SIZE; ++y)
		{
			for (int x = 0; x < SIZE; ++x)
			{
				board[x, y] = -1;
				transform.GetChild(y * SIZE + x).GetComponent<Image>().sprite = btnBackground;
			}
		}
	}

	public void ClickCell(int x, int y)
	{
		if (board[x, y] == -1)
		{
			board[x, y] = currentPlayers[currentPlayer].number;
			transform.GetChild(y * SIZE + x).GetComponent<Image>().sprite = currentPlayers[currentPlayer].sprite;

			Capture(x, y);
			CheckWin(x, y);

			++currentPlayer;
			currentPlayer %= currentPlayers.Count;

			turnTimer = 30.0f;
		}
	}

	private void CheckWin(int x, int y)
	{
		bool[] flags = { true, true, true, true, true, true, true, true };
		int highest = 0;

		//TODO: refactor
		for (int i = 1; i < 5; ++i)
		{
			if (x - i >= 0)
			{
				if (flags[0] && board[x - i, y] == currentPlayer) { highest = i; }
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

			if (flags[6] && y - i >= 0 && board[x, y - i] == currentPlayer) { highest = i; }
			else { flags[6] = false; }

			if (flags[7] && y + i < SIZE && board[x, y + i] == currentPlayer) { highest = i; }
			else { flags[7] = false; }
		}

		if (highest == 2) { print("Three"); }
		if (highest == 3) { print("Four"); }
		if (highest == 4) { Win(currentPlayer); }
	}

	private void Capture(int x, int y)
	{
		foreach (Vector2Int v in directions)
		{
			if (Captureable(x + v.x * 3, y + v.y * 3) &&
				board[x + v.x, y + v.y] != -1 &&
				board[x + v.x * 3, y + v.y * 3] != -1 &&
				board[x + v.x, y + v.y] != board[x + v.x * 3, y + v.y * 3] &&
				board[x, y] != board[x + v.x, y + v.y] &&
				board[x + v.x, y + v.y] == board[x + v.x * 2, y + v.y * 2])
			{
				if (++players[board[x + v.x, y + v.y]].captured == 5)
				{
					currentPlayers.Remove(players[board[x + v.x, y + v.y]]);
					++currentPlayer;
					currentPlayer %= currentPlayers.Count;
					if(currentPlayers.Count == 1) { Win(0); }
					print("Player " + (board[x + v.x, y + v.y] + 1) + " is out!");
				}

				board[x + v.x, y + v.y] = -1;
				transform.GetChild((y + v.y) * SIZE + x + v.x).GetComponent<Image>().sprite = btnBackground;
				board[x + v.x * 2, y + v.y * 2] = -1;
				transform.GetChild((y + v.y * 2) * SIZE + x + v.x * 2).GetComponent<Image>().sprite = btnBackground;
			}
		}
	}

	private bool Captureable(int x, int y)
	{
		return x >= 0 && x < SIZE && y >= 0 && y < SIZE;
	}

	private void Win(int player)
	{
		print(currentPlayers[player].name + " Wins!");
	}
}
