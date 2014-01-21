using System;
using System.Collections.Generic;

namespace CoinGameClassesLibrary
{
    public class Coin
    {
        int _NumberOfInitialEvents;
        int _NumberOfThrowings;

        public List<int[]> Outcomes; //  исходы (события), где каждый исход - массив индексов исходных событий

        int[] currentIndexes;   // temp var for recursion
        void GenerateOutcomes(int throwing = 1)
        {//рекурсивная функция, параметр - номер текущего броска, по умолчанию = 1
            if (throwing == 1)
            {
                currentIndexes = new int[_NumberOfThrowings];   //создается временный массив для индексов символов исходных событий
                Outcomes = new List<int[]>();   //создается список исходов
            }
            for (int i = 0; i < _NumberOfInitialEvents; i++)
            {//цикл для обхода исходных событий
                currentIndexes[throwing - 1] = i;   //временный массив сохраняет индекс исхода на позиции, равной текущему броску, т.к. индексация массива с нуля, то throwing - 1
                if (throwing < _NumberOfThrowings)
                {   //  переход к следующему броску если текущий бросок не последний, например, 1-ый или 2-ой из 3-ех
                    GenerateOutcomes(throwing + 1); //рекурсивный вызов, куда передается номер следующего броска
                }
                else
                {   //  иначе, если бросок последний, например 3-ий из 3-ех, добавление исхода
                    int[] item = new int[_NumberOfThrowings];   //временный массив для передачи индексов символов названия исхода
                    Array.Copy(currentIndexes, item, _NumberOfThrowings);   //копирование временного массива с именем в новый массив
                    Outcomes.Add(item); //добавление исхода в список
                }
            }
        }

        public List<int> CountSequences(int NumberOfEventForSequence, int NumberOfInitialEventsInSequence = 1)
        {   //  NumberOfEventForSequence - учитываемый для последовательности исходный выпад
            //  NumberOfInitialEventsInSequence - учитываемое количество событий в последовательности (по умолчанию = 1)
            //  при значении по умолчанию, получаем просто количетсво, иначе считаем подряд выпавшие события
            List<int> resultList = new List<int>(); //список результатов поиска последовательности по исходам
            for (int i = 0; i < Outcomes.Count; i++)
            {//цикл обходит все исходы
                int result = 0; //обнуляем счетчик для подсчета количества искомой последовательности при текущем исходе
                bool prevMatches = false;   //обнуляем флаг для начала последовательности (не играет роли при последовательности в длину 1)
                int countMatches = 0;   //  обнуляем количество выпадений нужного исхода подряд
                for (int j = 0; j < Outcomes[i].Length; j++)
                {//цикл обходит индексы текущего исхода
                    if (Outcomes[i][j] == NumberOfEventForSequence)
                    {
                        //  если искомый выпад найден
                        countMatches++; //счетчик инкременирует
                        if (NumberOfInitialEventsInSequence == 1 || prevMatches)
                        {   //  если предыдущий выпад был искомым
                            if (countMatches >= NumberOfInitialEventsInSequence)
                            {
                                result++;
                            }
                        }
                        else
                        {   // иначе, если выпад был искомый, но не достаточной длины, то
                            prevMatches = true; //указываем, что последовательность началась
                        }
                    }
                    else
                    {
                        //  если искомый выпад не найден
                        countMatches = 0;   //обнуляем последовательность
                    }
                }
                resultList.Add(result); //добавляем результат поиска по данному исходу
            }
            return resultList;  //результат работы функции
        }

        public Coin(int NumberOfInitialEvents, int NumberOfThrowings)
        {   //конструктор
            _NumberOfInitialEvents = NumberOfInitialEvents;
            _NumberOfThrowings = NumberOfThrowings;
            GenerateOutcomes();
        }
    }//основной класс
}
