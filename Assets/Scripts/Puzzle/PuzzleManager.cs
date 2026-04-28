using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;
using UnityEngine.SceneManagement;

// tutorial referenced for puzzle: https://www.youtube.com/watch?v=IgBjJ-bexeo&t=45s
// adjusted for 3d game 

public class PuzzleManager : MonoBehaviour
{
    //puzzle board
    [SerializeField] private Transform gameTransform; 
     //puzzle piece
    [SerializeField] private GameObject puzzlePiece;
    // countdown timer text
    [SerializeField] private TextMeshProUGUI timerText; 
    //puzzle canvas 
    [SerializeField] private GameObject puzzleCanvas;

    //events that communicate w other scripts when the puzzle is open
    public static event Action PuzzleOpened;
    public static event Action PuzzleClosed;

    //tracks puzzle pieces
    private List<RectTransform> pieces = new List<RectTransform>();
    private int emptyLocation;
    
    //board size 
    private int size;
    
    //player can't click while shuffling + once puzzle is solved
    private bool shuffling = false;
    private bool completed = false;
    
    //countdown timer
    private float timeLimit = 90f;
    private float timer = 0f;
    private bool isPaused = false;

    void Start()
    {
        size = 3; //board size (3x3)
        CreateGamePieces(4f);
        shuffling = true;
        StartCoroutine(WaitShuffle(1f));
        if (timerText != null)
            timerText.text = "Time: 90";
        else
            Debug.LogError("timer is null");
    }

    private void CreateGamePieces(float gapThickness)
    {   
        //puzzle piece and grid size
        float pieceSize = 200f;
        float gridSize = pieceSize * size;

        //starting positions for puzzle pieces
        float startX = -gridSize / 2 + pieceSize / 2;
        float startY = gridSize / 2 - pieceSize / 2;
        
        //fraction of the image it should show
        float width = 1f / size;

        //creates the puzzle grid
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                GameObject piece = Instantiate(puzzlePiece, gameTransform);
                //get recttransform (for ui) so the piece can be placed
                RectTransform rt = piece.GetComponent<RectTransform>();
                //set width and height 
                rt.sizeDelta = new Vector2(pieceSize - gapThickness, pieceSize - gapThickness);
                //set position 
                rt.anchoredPosition = new Vector2(startX + col * pieceSize, startY - row * pieceSize);
                //name each piece to its correct index (used to see if puzzle is solved)
                piece.name = $"{(row * size) + col}";
                pieces.Add(rt);

                if ((row == size - 1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    RawImage pieceImage = piece.GetComponent<RawImage>();
                    //what section of the full image should this specific piece display
                    pieceImage.uvRect = new Rect(width * col, 1 - (width * (row + 1)), width, width);
                }
            }
        }
    }

    //short delay before initial shuffle
    //player briefly sees solved state
    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Shuffle();
        shuffling = false;
    }

    //scramble puzzle before it starts, and when timer runs out
    //only makes legal moves from "solved" state to make sure puzzle is actually solvable
    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (size * size * size))
        {
            int random = UnityEngine.Random.Range(0, size * size);
            if (random == last) continue;
            last = emptyLocation;
            //move random piece up one row
            if (SwapIfValid(random, -size, size)) count++;
            //move random piece down one row
            else if (SwapIfValid(random, +size, size)) count++;
            //move random piece left one column
            else if (SwapIfValid(random, -1, 0)) count++;
            //move random piece right one column
            else if (SwapIfValid(random, +1, size - 1)) count++;
        }
    }
    
    void OnEnable()
    {
        PuzzleOpened?.Invoke();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        //subscribe to pause events
        PauseMenu.PauseMenuActive += OnPauseMenuOpen;
        PauseMenu.PauseMenuInactive += OnPauseMenuClose;
    }

    void OnDisable()
    {
        PuzzleClosed?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        //unsubscribe to pause events
        PauseMenu.PauseMenuActive -= OnPauseMenuOpen;
        PauseMenu.PauseMenuInactive -= OnPauseMenuClose;
    }

    private void OnPauseMenuOpen()
    {
        isPaused = true;
    }

    private void OnPauseMenuClose()
    {
        isPaused = false;
    }


    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //calculate and display remaining time
        if (timerText != null)
        {
            float timeRemaining = timeLimit - timer;
            //rounded up to show whole numbers
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";
        }

        //debug key for final demo 
        if (Input.GetKeyDown(KeyCode.C))
        {
            completed = true;
            StartCoroutine(ShowCompletion());
            Debug.Log("shortcut");
        }

        //check if puzzle is solved when not shuffling
        if (!shuffling && CheckCompletion())
        {
            //complete? mark complete, start completion seq
            completed = true;
            Debug.Log("puzzle done");
            StartCoroutine(ShowCompletion());
        }

        if (!isPaused)
        {
            if (!completed && !shuffling)
            {
                timer += Time.unscaledDeltaTime;
                //shuffle board again if player reached time limit
                if (timer >= timeLimit)
                {
                    timer = 0f;
                    //lock clicking while shuffling
                    shuffling = true;
                    Shuffle();
                    //unlock clicking when done shuffling
                    shuffling = false;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    //mouse click within bounds of the puzzle piece? 
                    if (RectTransformUtility.RectangleContainsScreenPoint(pieces[i], Input.mousePosition, null))
                    {
                        if (SwapIfValid(i, -size, size)) break;
                        if (SwapIfValid(i, +size, size)) break;
                        if (SwapIfValid(i, -1, 0)) break;
                        if (SwapIfValid(i, +1, size - 1)) break;
                    }
                }
            }
        }
    }

    //can a piece legally slide into the empty slot?
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            //swap pieces in the list so tracking order is right
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            //swap pieces on screen
            (pieces[i].anchoredPosition, pieces[i + offset].anchoredPosition) = (pieces[i + offset].anchoredPosition, pieces[i].anchoredPosition);
            //update empty slot
            emptyLocation = i;
            //play slide sound when valid swap is made
            FindObjectOfType<AudioManager>().Play("Puzzle");
            return true;
        }
        return false;
    }

    //checks if puzzle is sovled
    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            //if piece name isn't in it's correct slot the puzzle isn't solved
            if (pieces[i].name != $"{i}") //$"{x}" converts value of x into string
            {
                return false;
            }
        }
        //puzzle completed! 
        return true;
    }

    private IEnumerator ShowCompletion()
    {
        //play completion sfx
        FindObjectOfType<AudioManager>().Play("puzzleSolved");
        yield return new WaitForSecondsRealtime(1.5f);
        //load endgame panel in obsidianscene
        SceneManager.LoadScene("ObsidianScene");
    }

    public void ClosePuzzle()
    {
        if(!shuffling && !completed)
        {
            puzzleCanvas.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}