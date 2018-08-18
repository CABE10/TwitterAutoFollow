using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAutoFollow
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Hello!\n\n");
                //So, we have a browser open with some "follow" image links.
                //If you click these image links, then you follow the person.

                //1) Console will ask you to draw a rectange on the screen where the Follow buttons may be.

                //2) Internally takes a screenshot.

                //3) Screenshot translates to List of coordinates.

                //4) Iterate through coordinates and click those coordinates.

                //5) Page Down. Rinse and Repeat until no matches are found.






            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
