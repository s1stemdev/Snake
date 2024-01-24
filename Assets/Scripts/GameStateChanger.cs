using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
  private AppleSpawner _appleSpawner;   // Скрипт появления яблок
  private SnakeMoveControll _snake;   // Скрипт движения змейки
  private GameField _gameField;       // Скрипт игрового поля
  
  private void Start() {
    InitValues();     // Инициализируем переменные
    FirstStartGame(); // Вызываем метод FirstStartGame() при запуске игры
  }

  private void InitValues() {
    _snake        = GameObject.Find("Snake").GetComponent<SnakeMoveControll>();
    _gameField    = FindObjectOfType<GameField>();
    _appleSpawner = FindObjectOfType<AppleSpawner>();
  }

  private void FirstStartGame()
  {
    _gameField.FillCellsPositions();  // Вызываем метод FillCellsPositions() из скрипта GameField, чтобы заполнить позиции ячеек
    StartGame();
  }

  public void StartGame() {
    _snake.StartGame();           // Начинаем движение змейки
    _appleSpawner.CreateApple(); // Создаём новое яблоко
  }

  public void EndGame() {
    _snake.StopGame(); // Останавливаем движение змейки
  }
}