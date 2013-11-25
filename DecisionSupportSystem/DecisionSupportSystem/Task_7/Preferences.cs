using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionSupportSystem.Task_7
{
    public class Preferences
    {
        public char[] evenoddNames, numericNames;          //списки имен исходных событий
        public bool evenoddGame;                           //выбор типа игры Ч/Н или 1/2/3/4/5/6
        public int numberofthrowings;                      //количество бросков
        public int numberofoutcomesperstake;               //количество учитываемых исходов/событий в ставке
        public decimal amountofstakevalue;                 //сумма ставки
    }
}
