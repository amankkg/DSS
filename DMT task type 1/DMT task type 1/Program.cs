using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DMT_task_type_1
{
    class Program
    {
        static void Main(string[] args)
        {
            //  defining new container
            var db = new DSSDBEntities();

            //  getting quantities of alternatives and events
            Console.WriteLine("~~~ Task type #1 ~~~");
            Console.WriteLine("Input Alternatives quantity: ");
            int nAlts = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Input Events quantity: ");
            int nEvs = Convert.ToInt32(Console.ReadLine());
            //  type #1 means next combinations
            int nCP = nAlts * nEvs;
            Console.WriteLine("QuantityOfCombinations = {0} * {1} = {2}", nAlts, nEvs, nCP);
            
            //  defining names for actions
            //  defining list of actions
            Console.WriteLine("Give Alternatives some name");
            List<tbl_Action> Alts = new List<tbl_Action>();
            for (int i = 0; i < nAlts; i++)
            {
                //  adding elements to created list
                Console.WriteLine("#{0}: ", i + 1);
                Alts.Add(new tbl_Action { Action = Console.ReadLine() });
            }

            //  defining names for events
            Console.WriteLine("Give Events some name, and probabilities:");
            List<tbl_Event> Evs = new List<tbl_Event>();
            for (int i = 0; i < nAlts; i++)
            {
                //  adding elements to created list
                Console.WriteLine("#{0}: ", i + 1);
                Evs.Add(new tbl_Event { Event = Console.ReadLine(), Probability = Convert.ToDecimal(Console.ReadLine()) });
            }

            //  defining CP combinations
            Console.WriteLine("Give Conditional Profit values:");
            List<CP> CPs = new List<CP>();
            for (int i = 0; i < nAlts; i++)
            {
                for (int j = 0; j < nEvs; j++)
                {
                    Console.WriteLine("{0}", i + j + 1);
                    CPs.Add(new CP { Action = Alts[i], Event = Evs[j], Value = Convert.ToDecimal(Console.ReadLine()) });  //  continue here...
                }
            }

            Console.Read();
        }

        class CP
        {
            public decimal Value;
            public tbl_Action Action;
            public tbl_Event Event;
        }
    }
}
