using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    private static GameEvents singleton;
    private static GameEvents Singleton => singleton ? singleton : singleton = FindObjectOfType<GameEvents>();

    public UnityEvent pressedCorrect = new UnityEvent();
    public UnityEvent pressedGood = new UnityEvent();
    public UnityEvent pressedGreat = new UnityEvent();
    public UnityEvent pressedPerfect = new UnityEvent();
    public UnityEvent pressedWrong = new UnityEvent();
    public UnityEvent missed = new UnityEvent();
    public UnityEvent failed = new UnityEvent();
    public UnityEvent levelStarted = new UnityEvent();
    public UnityEvent levelFinished = new UnityEvent();
    public UnityEvent<int> landedCombo = new UnityEvent<int>();
    public UnityEvent<int> finishedCombo = new UnityEvent<int>();
    public UnityEvent<int> cancelledCombo = new UnityEvent<int>();
    public UnityEvent gameOver = new UnityEvent();

    public static UnityEvent PressedCorrect => Singleton?.pressedCorrect;
    public static UnityEvent PressedGood => Singleton?.pressedGood;
    public static UnityEvent PressedGreat => Singleton?.pressedGreat;
    public static UnityEvent PressedPerfect => Singleton?.pressedPerfect;
    public static UnityEvent PressedWrong => Singleton?.pressedWrong;
    public static UnityEvent Missed => Singleton?.missed;
    public static UnityEvent Failed => Singleton?.failed;
    public static UnityEvent LevelStarted => Singleton?.levelStarted;
    public static UnityEvent LevelFinished => Singleton?.levelFinished;
    public static UnityEvent<int> LandedCombo => Singleton?.landedCombo;
    public static UnityEvent<int> FinishedCombo => Singleton?.finishedCombo;
    public static UnityEvent<int> CancelledCombo => Singleton?.cancelledCombo;
    public static UnityEvent GameOver => Singleton?.gameOver;
}
