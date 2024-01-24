using UnityEngine;

public class SnakeMoveControll : MonoBehaviour
{
  // Начальное положение змейки в виде координат ячейки
  // Точку (5, 5) мы выбрали случайно, у вас она может отличаться
  public Vector2Int StartCellId = new Vector2Int(5, 5);

  // Задержка между движениями змейки
  // Позже вы сможете настроить это значение в инспекторе
  public float MoveDelay = 0.4f;

  //Префабы лучше оставлять публичными для гибкости настройки игры
  public GameFieldObject BodyPrefab; // Префаб тела змейки
  public GameFieldObject HeadPrefab; // Префаб головы змейки


  private GameField _gameField; // Скрипт игрового поля
  private GameFieldObject[] _parts;  // Массив частей змейки

  private Vector2Int _moveDirection = Vector2Int.up;  // Направление движения
  private float _moveTimer; // Таймер движения

  public GameStateChanger _gameStateChanger; // Скрипт изменения состояния игры
  public AppleSpawner _appleSpawner;         // Скрипт появления яблок
  private bool _isActive;                    // Флаг активности змейки
  private bool _isFirstStart = true;

  // Update is called once per frame
  void Update()
  {
    if (!_isActive) // Если игра активна
    {
      return;
    } 
      //Debug.Log("ACTIVE!!");
      GetMoveDirection();   // Получаем направление движения от пользователя
      MoveTimerTick();      // Проверяем, достаточно ли прошло времени, чтобы двигать змейку
  }

  public void StartGame() {  
    if(_isFirstStart)
    {
      _gameField = GameObject.Find("GameField").GetComponent<GameField>();
      _gameStateChanger = FindObjectOfType<GameStateChanger>();
      _appleSpawner = FindObjectOfType<AppleSpawner>();
      _isFirstStart = false;
    }
    CreateSnake();    // Создаём змейку
    _isActive = true; // Устанавливаем флаг активности
  }

  public void StopGame()            { _isActive = false; }    // Снимаем флаг активности
  public int  GetSnakePartsLength() { return _parts.Length; } // Возвращаем длину змейки

  private void CreateSnake()
  {
    _parts = new GameFieldObject[0];               // Создаём массив частей змейки с длиной 0
    AddSnakePart(HeadPrefab, StartCellId);   // Добавляем голову змейки на заданную позицию

    // Добавляем одну часть тела змейки на позицию, которая находится на одну клетку ниже от головы
    AddSnakePart(BodyPrefab, StartCellId + Vector2Int.down);
  }

  private void AddSnakePart(GameFieldObject partPrefab, Vector2Int cellId)
  {
    IncreaseGameFieldObjectsArrayLenght();                        // Увеличиваем длину массива частей змейки
    GameFieldObject newSnakePart = Instantiate(partPrefab);       // Создаём новую часть змейки из префаба
    _parts[_parts.Length - 1] = newSnakePart;                     // Записываем новую часть змейки в конец массива
    //SetGameFieldObjectPositionCell(newSnakePartObject, cellId); // Ставим новую часть змейки на заданную ячейку
    _gameField.SetObjectCell(newSnakePart, cellId);
  }

  private void IncreaseGameFieldObjectsArrayLenght()
  {
    GameFieldObject[] tempGameFieldObjectsArray = _parts;                // Заводим временный массив, который будет хранить текущие части змейки
    _parts = new GameFieldObject[tempGameFieldObjectsArray.Length + 1];  // Создаём новый массив частей змейки с длиной на 1 больше

    for (int i = 0; i < tempGameFieldObjectsArray.Length; i++) {    // Проходим по всей длине временного массива
      _parts[i] = tempGameFieldObjectsArray[i];                     // Копируем уже существующие части змейки из временного массива в новый массив
    }
  }

  /*private void SetGameFieldObjectPositionCell(GameFieldObject part, Vector2Int cellId)
  {
    Vector2 cellPosition = _gameField.GetCellPosition((uint)cellId.x, (uint)cellId.y);  // Получаем позицию ячейки по её координатам
    part.SetCellPosition(cellId, cellPosition);                            // Устанавливаем часть змейки на заданную позицию
  }*/

  private void GetMoveDirection()
  {
         if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))   // Если пользователь нажал на клавишу W или стрелку вверх
            && _moveDirection != Vector2Int.down) {                               // и текущее направление не вниз
      SetSnakeMoveDirection(Vector2Int.up);                                       // Устанавливаем направление движения вверх
    }
    else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) // Если пользователь нажал на клавишу A или стрелку влево
            && _moveDirection != Vector2Int.right) {                              // и текущее направление — не вправо
      SetSnakeMoveDirection(Vector2Int.left);                                     // Устанавливаем направление движения влево
    }
    else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // Если пользователь нажал на клавишу S или стрелку вниз
            && _moveDirection != Vector2Int.up) {                                 // и текущее направление не вверх
      SetSnakeMoveDirection(Vector2Int.down);                                     // Устанавливаем направление движения вниз
    }
    else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))// Если пользователь нажал на клавишу D или стрелку вправо
            && _moveDirection != Vector2Int.left) {                               // и текущее направление — не влево
      SetSnakeMoveDirection(Vector2Int.right);                                    // Устанавливаем направление движения вправо
    }
  }

  private void SetSnakeMoveDirection(Vector2Int moveDirection)
  {
    _moveDirection = moveDirection;  // Устанавливаем новое направление движения
    SetHeadRotation(moveDirection);  // Поворачиваем голову змейки по новому направлению
    MoveSnake();                     // Двигаем змейку по карте
  }

  private void MoveTimerTick()
  {
    _moveTimer += Time.deltaTime;   // Увеличиваем значение таймера на время, которое прошло с последнего кадра
    if (_moveTimer >= MoveDelay) {  // Если значение таймера достигло значения задержки
      MoveSnake();                  // Двигаем змейку по карте
    }
  }

  private void MoveSnake()
  {
    _moveTimer = 0;                                          // Сбрасываем значение таймера
                                                             
    Vector2Int lastPartCellId = _parts[_parts.Length - 1].GetCellId();               //  Получаем ячейку последней части змейки
    Vector2Int headNewCell    = MoveCellId(_parts[0].GetCellId(), _moveDirection);   // Получаем новую ячейку для головы змейки в зависимости от текущего направления
    _gameField.SetCellIsEmpty((uint)lastPartCellId.x, (uint)lastPartCellId.y, true); // Освобождаем ячейку последней части змейки на игровом поле

    for (int i = _parts.Length - 1; i >= 0; i--)  {          // После Проходим по всем частям змейки с конца
      Vector2Int partCellId = _parts[i].GetCellId();         // Каждый раз Получаем позицию ячейки текущей части
      if (i == 0) {                                          // Если текущая часть — голова змейки
        partCellId = headNewCell;                            // То Двигаем голову змейки в направлении движения
      } else {                                               // Если текущая часть не голова змейки
        partCellId = _parts[i - 1].GetCellId();              // То Позиция текущей части змейки становится равной позиции предыдущей части
      }
      
      _gameField.SetObjectCell(_parts[i], partCellId);
      CheckNextCellFail(headNewCell);                   // Проверяем, есть ли в следующей ячейке столкновение
      CheckNextCellApple(headNewCell, lastPartCellId);  // Проверяем, есть ли в следующей ячейке яблоко
      
    }
  }

  private Vector2Int MoveCellId(Vector2Int cellId, Vector2Int direction)
  {
    cellId += direction;                                 // Увеличиваем значение cellId на значения direction, чтобы получить новую позицию клетки змейки
    cellId.x = GetNewCoordPositionIfMovingOutsideTheField(cellId.x); // проверяем не зашла ли змейка за края поля по оси х
    cellId.y = GetNewCoordPositionIfMovingOutsideTheField(cellId.y); // проверяем не зашла ли змейка за края поля по оси у

    return cellId;    // Возвращаем новую позицию
  }

  private int GetNewCoordPositionIfMovingOutsideTheField(int coordXorY)
  {
    int resultCoordXorY = coordXorY;
    if (coordXorY >= _gameField.CellsInRow) {      // Если новая позиция по оси больше или равна количеству клеток в ряду игрового поля
      resultCoordXorY = 0;                        // Обнуляем позицию по оси, чтобы змейка переместилась на начало ряда
    } else if (coordXorY < 0) {                   // Иначе, если новая позиция по заданной оси меньше 0
      resultCoordXorY = _gameField.CellsInRow - 1; // Делаем позицию равным количеству клеток в ряду игрового поля минус 1, чтобы змейка переместилась в конец ряда
    }
    return resultCoordXorY;
  }

  private void SetHeadRotation(Vector2Int direction)
  {
    Vector3 headEuler = Vector3.zero;                // Создаём новый вектор, который будет использоваться для поворота головы змейки
    if (direction.x == 0 && direction.y == 1) {      // Если направление движения по оси x равно 0 и направление движения по оси y равно 1 (ВВЕРХ)
      headEuler = new Vector3(0f, 0f, 0f);           // Делаем вектор равным (0, 0, 0), чтобы голова змейки была направлена вверх
    }
    else if (direction.x == 1 && direction.y == 0) { // Иначе, если направление движения по оси x равно 1 и направление движения по оси y равно 0 (ВПРАВО)
      headEuler = new Vector3(0f, 0f, -90f);         // Делаем вектор равным (0, 0, -90), чтобы голова змейки была направлена вправо
    }
    else if (direction.x == 0 && direction.y == -1) { // Иначе, если направление движения по оси x равно 0 и направление движения по оси y равно -1 (ВНИЗ)
      headEuler = new Vector3(0f, 0f, 180f);          // Делаем вектор равным (0, 0, 180), чтобы голова змейки была направлена вниз
    }
    else if (direction.x == -1 && direction.y == 0) { // Иначе, если направление движения по оси x равно -1 и направление движения по оси y равно 0 (ВЛЕВО)
      headEuler = new Vector3(0f, 0f, 90f);           // Делаем вектор равным (0, 0, 90), чтобы голова змейки была направлена влево
    }
    // Приравниваем углы поворота головы змейки к вектору
    _parts[0].transform.eulerAngles = headEuler;
  }

  private void CheckNextCellFail(Vector2Int nextCellId)
  {
    for (int i = 1; i < _parts.Length; i++) // Проходим по частям змейки
    {
      // Если индекс (положение) проверяемой в цикле части змейки совпадает с проверяемым индексом (положением) ячейки
      // То есть если змейка заползает на саму себя
      if (_parts[i].GetCellId() == nextCellId) {
        _gameStateChanger.EndGame(); // Завершаем игру
      }
    }
  }

  private void CheckNextCellApple(Vector2Int nextCellId, Vector2Int cellIdForAddPart)
  {
    if (_appleSpawner.GetAppleCellId() == nextCellId) { // Если в следующей ячейке после головы змейки есть яблоко
      AddSnakePart(BodyPrefab, cellIdForAddPart);           // Добавляем новую часть змейки
      _appleSpawner.SetNextApple();                     // Удаляем текущее яблоко и создаём новое
    }
  }
}
