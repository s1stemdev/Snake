using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
  public GameFieldObject ApplePrefab; // Префаб яблока

  public GameStateChanger _gameStateChanger; // Скрипт изменения состояния игры
  public GameField _gameField;        // Скрипт игрового поля
  public SnakeMoveControll Snake;     // Скрипт движения змейки

  private GameFieldObject _apple; // Текущий объект яблока

  public void CreateApple()
  {
    _apple = Instantiate(ApplePrefab); // Создаём новый экземпляр яблока
    SetNextApple();                    // Устанавливаем следующее яблоко
  }

  public void SetNextApple()
  {
    // Если текущего яблока нет
    if (!_apple) { return; } // Возвращаемся из метода

    if (!CheckHasEmptyCells()) {   // Если нет свободных клеток на поле
      _gameStateChanger.EndGame(); // Завершаем игру
      return;                      // Возвращаемся из метода
    }
     
              int emptyCellsCount = GetEmptyCellsCount();            // Получаем количество свободных клеток
    Vector2Int[] possibleCellsIds = new Vector2Int[emptyCellsCount]; // Создаём массив возможных клеток для появления яблока, размер которого равен количеству свободных клеток

    int counter = 0;                                        // Заводим счётчик для отслеживания индекса текущей свободной клетки
    for (int x = 0; x < _gameField.CellsInRow; x++) {       // Проходим по всем рядам поля
      for (int y = 0; y < _gameField.CellsInRow; y++) {     // Проходим по всем ячейкам в ряде
        if (_gameField.GetCellIsEmpty((uint)x, (uint)y)) {  // Если текущая ячейка пуста
          possibleCellsIds[counter] = new Vector2Int(x, y); // Добавляем индекс текущей ячейки в массив возможных клеток
          counter++;                                        // Увеличиваем счётчик свободных клеток
        }
      }
    }
    Vector2Int appleCellId = possibleCellsIds[Random.Range(0, possibleCellsIds.Length)]; // Выбираем случайную клетку из массива возможных клеток для размещения нового яблока
    _gameField.SetObjectCell(_apple, appleCellId);                                       // Устанавливаем яблоко в выбранной клетке
  }

  public Vector2Int GetAppleCellId() { return _apple.GetCellId(); } // Возвращаем индекс текущей клетки яблока

  private bool CheckHasEmptyCells() { return GetEmptyCellsCount() > 0; } // Возвращаем true, если свободных клеток больше 0

  private int GetEmptyCellsCount()
  {
    int snakePartsLength = Snake.GetSnakePartsLength();                  // Определяем длину змейки
    int fieldCellsCount = _gameField.CellsInRow * _gameField.CellsInRow; // Получаем общее количество клеток на поле

    return fieldCellsCount - snakePartsLength;                         // Возвращаем разницу между общим количеством клеток и длиной змейки
  }
}
