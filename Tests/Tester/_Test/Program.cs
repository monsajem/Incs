using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.TimeingTester;
namespace _Test
{

    class Program
    {

        public class Item
        {
            public Item Next;
        }
        static void Main(string[] args)
        {
            var I = new Item();
            var Item = I;
            var t = Timing.run(() =>
            {
                for (int i = 0; i < 100000000; i++)
                {
                    var NewItem = new Item();
                    Item.Next = NewItem;
                    Item = NewItem;
                }
            });
            Item = I;
            var t2 = Timing.run(() =>
            {
                for (int i = 0; i < 100000000; i++)
                {
                    Item = Item.Next;
                }
                Item.ToString(); ;
            });
        }
    }
}