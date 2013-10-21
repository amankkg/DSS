using System;
using System.Collections.Generic;

namespace CoinGameClassesLibrary
{
    public class Coin
    {
        int _NumberOfInitialEvents;
        double[] InitialEventsBonuses;
        int _NumberOfThrowings;

        public List<int[]> Outcomes; //  исходы (события)

        int[] currentIndexes;   // temp var for recursion
        void GenerateOutcomes(int throwing = 1)
        {
            if (throwing == 1)
            {
                currentIndexes = new int[_NumberOfThrowings];
                Outcomes = new List<int[]>();
                //size = Convert.ToInt32(Math.Pow(InitialEvents.Length, NumberOfThrowings))
            }
            for (int i = 0; i < _NumberOfInitialEvents; i++)
            {
                currentIndexes[throwing - 1] = i;
                if (throwing < _NumberOfThrowings)
                {   //  переход к следующему броску
                    GenerateOutcomes(throwing + 1);
                }
                else
                {   //  добавление исхода
                    int[] item = new int[_NumberOfThrowings];
                    Array.Copy(currentIndexes, item, _NumberOfThrowings);
                    Outcomes.Add(item);
                }
            }
        }

        public List<int> CountSequences(int NumberOfEventForSequence, int NumberOfInitialEventsInSequence = 1)
        {   //  NumberOfEventForSequence - учитываемый для последовательности исходный выпад
            //  NumberOfInitialEventsInSequence - учитываемое количество событий в последовательности (по умолчанию = 1)
            //  при значении по умолчанию, получаем просто количетсво, иначе считаем подряд выпавшие события
            List<int> resultList = new List<int>();
            for (int i = 0; i < Outcomes.Count; i++)
            {
                int result = 0;
                bool prevMatches = false;
                int countMatches = 0;   //  количество выпадений нужного исхода подряд
                for (int j = 0; j < Outcomes[i].Length; j++)
                {
                    if (Outcomes[i][j] == NumberOfEventForSequence)
                    {
                        //  если искомый выпад найден
                        countMatches++;
                        if (prevMatches)
                        {   //  если предыдущий выпад был искомым
                            if (countMatches >= NumberOfInitialEventsInSequence)
                            {
                                result++;
                            }
                        }
                        else
                        {
                            prevMatches = true;
                        }
                    }
                    else
                    {
                        //  если искомый выпад не найден
                        countMatches = 0;
                    }
                }
                resultList.Add(result);
            }
            return resultList;
        }

        public Coin(double[] InitialEventsBonusValues, int NumberOfThrowings)
        {   //конструктор
            _NumberOfInitialEvents = InitialEventsBonusValues.Length;
            InitialEventsBonuses = InitialEventsBonusValues;
            _NumberOfThrowings = NumberOfThrowings;
            GenerateOutcomes();
        }
    }//основной класс
}
