using System;
using System.Collections.Generic;

namespace DiceGameClassesLibrary
{
    public class Dice
    {
        int _NumberOfInitialEvents;  // количество исходных события
        int _NumberOfThrowings; //  кол-во бросков
        int _NumberOfOutcomesPerStake;    //  кол-во исходов за комбинацию

        int[] tempIndexes;    //  временный массив, содержит индексы при рекурсии, какие исходные события входят в исход
        //например: если currentIndexes = [0, 1], то Исход = _InitialEvents[0] + _InitialEvents[1] = "Ч" + "Н"
        //или массив содержит индексы того, какие исходы сейчас рассматриваются
        //например: если currentIndexes = [1, 2], то Ставка = _Outcomes[1] + _Outcomes[2] = "ЧЧ" + "ЧН"

        public List<int[]> Outcomes; //  исходы (события)
        void GenerateOutcomes(int throwing = 1)
        {
            #region алгоритм
            /*  генерируем исходы (события)
             *  * количество = 8
             *  * Outcomes = [ЧЧЧ, ЧЧН, ЧНЧ, ЧНН, НЧЧ, НЧН, ННЧ, ННН]
             * 
             * Пример алгоритма при 3 бросках
             *  for (int i = 0; i < InitialEvents.Length; i++)
             *  {
             *      for (int j = 0; j < InitialEvents.Length; j++)
             *      {
             *          for (int k = 0; k < InitialEvents.Length; k++)
             *          {
             *              body
             *          }
             *      }
             *  }
             *  
             * ниже - то же самое, но с использованием рекурсии для динамичности
             */
            #endregion
            if (throwing == 1)
            {
                tempIndexes = new int[_NumberOfThrowings];
                Outcomes = new List<int[]>();
                //size = Convert.ToInt32(Math.Pow(InitialEvents.Length, NumberOfThrowings))
            }
            for (int i = 0; i < _NumberOfInitialEvents; i++)
            {
                tempIndexes[throwing - 1] = i;
                if (throwing < _NumberOfThrowings)
                {   //  переход к следующему броску
                    GenerateOutcomes(throwing + 1);
                }
                else
                {   //  добавление исхода
                    int[] item = new int[_NumberOfThrowings];
                    Array.Copy(tempIndexes, item, _NumberOfThrowings);
                    Outcomes.Add(item);
                }
            }
        }

        public List<int[]> Stakes;    //  ставки (действия)
        void GenerateStakes(int throwing = 1, int previousIndex = -1)
        {
            #region алгоритм
            /*  генерируем ставки (действия)
             *  * количество при 2 бросках = 6: Stakes = [ЧЧ и ЧН, ЧЧ и НЧ, ЧЧ и НН, ЧН и НЧ, ЧН и НН, НЧ и НН]
             *  * количество при 3 бросках = 56
             *  * количество при 4 бросках = 1820
             *  * количество при 5 бросках = 201376
             *  * количество при 6 бросках = 74974368
             *  * количество при 7 бросках = 94525795200
             * 
             * Пример: рассмотрим случай на 4 броска
             *  for (int i = 0; i < Outcomes.Count; i++)
             *  {
             *      currentIndexes[0] = i;
             *      for (int j = i + 1; j < Outcomes.Count; j++)
             *      {
             *          currentIndexes[1] = j;
             *          for (int k = j + 1; k < Outcomes.Count; k++)
             *          {
             *              currentIndexes[2] = k;
             *              for (int l = k + 1; l < Outcomes.Count; l++)
             *              {
             *                  currentIndexes[3] = l;
             *                  addStake();
             *              }
             *          }
             *      }
             *  }
             *
             *  index - текущее значение счетчика (i,j,k,l...) предыдущего уровня
             *  level - какой по счету уровень рекурсии (вложенности цикла из примера) рассматривается
             *  
             * ниже - то же самое, но с использованием рекурсии для динамичности
             */
            #endregion
            if (throwing == 1)
            {
                tempIndexes = new int[_NumberOfOutcomesPerStake];
                Stakes = new List<int[]>();
                //  size = ...
            }

            for (int i = previousIndex + 1; i < Outcomes.Count; i++)
            {
                tempIndexes[throwing - 1] = i;  //  добавляем исход текущего уровня в итоговую комбинацию
                if (throwing < _NumberOfOutcomesPerStake)
                {   //  переход в следующий уровень
                    GenerateStakes(throwing + 1, i);
                }
                else
                {   //  добавляем новую ставку
                    int[] item = new int[_NumberOfOutcomesPerStake];
                    for (int j = 0; j < _NumberOfOutcomesPerStake; j++)
                    {
                        item[j] = tempIndexes[j];
                    }
                    Stakes.Add(item);
                }
            }
        }

        public List<StakeOutcomeCombination> StakeOutcomeCombinations; //  комбинации ставка-исход (действие-событие)
        void GenerateStakeOutcomeCombinations()
        {
            StakeOutcomeCombinations = new List<StakeOutcomeCombination>();
            //  size = Outcomes.Count * Stakes.Count
            for (int i = 0; i < Stakes.Count; i++)
            {
                for (int j = 0; j < Outcomes.Count; j++)
                {
                    StakeOutcomeCombinations.Add(new StakeOutcomeCombination() { StakeOutcomes = Stakes[i], _ChoosenStake = i, AcutalOutcome = j, SoEG = SetSoEG(Stakes[i], j) });
                }
            }
        }

        bool SetSoEG(int[] stake, int outcome)
        {
            for (int i = 0; i < stake.LongLength; i++)
            {
                if (stake[i] == outcome)
                {
                    return true;
                }
            }
            return false;
        }

        public Dice(int NumberOfInitialEvents, int NumberOfThrowings, int NumberOfOutcomesPerStake = 2)
        {   //присваиваем значения свойствам игры/класса
            _NumberOfThrowings = NumberOfThrowings;
            _NumberOfOutcomesPerStake = NumberOfOutcomesPerStake;
            _NumberOfInitialEvents = NumberOfInitialEvents;

            GenerateOutcomes();
            GenerateStakes();
            GenerateStakeOutcomeCombinations();
        }
    }

    public struct StakeOutcomeCombination
    {
        public int[] StakeOutcomes; //  ставки в виде массива (для вычисления имени действия в задаче, чтобы опять не лезть в список Stakes) - иначе хватило бы и _ChoosenStake
        public int _ChoosenStake;   //  выбранная ставка
        public int AcutalOutcome;   //  выпаший исход
        public bool SoEG;   //  выигрыш/проигрыш
    }
}
