using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterAutoFollow.Services;
namespace TwitterAutoFollow
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Please turn off any screen dimmers.
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Hello!\n\n");
                //So, we have a browser open with some "follow" image links.
                //If you click these image links, then you follow the person.

                //1) Console will capture the window.
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Running...");
                Image image = ImageRenderService.CaptureDesktop();
                //2) Screenshot translates to List of coordinates.
                List<Point> coordinates = ImageRenderService.TranslateImageToCoordinates(image);
                //3) Iterate through coordinates and click those coordinates.
                int pageDownWait = int.Parse(ConfigurationManager.AppSettings["PageDownWait"]);
                int waitBetweenFollows = int.Parse(ConfigurationManager.AppSettings["WaitBetweenFollows"]);
                while (coordinates != null && coordinates.Count() > 0)
                {
                    int count = coordinates.Count();
                    int temp = 0;
                    foreach (var item in coordinates)
                    {
                        temp++;
                        SurfaceAreaService.MouseClick(item);
                        System.Threading.Thread.Sleep(waitBetweenFollows);
                        if (temp == count)
                        {   //4) Page Down. Rinse and Repeat until no matches are found.
                            SurfaceAreaService.PageDown();
                            System.Threading.Thread.Sleep(pageDownWait);
                        }
                    }
                    image = ImageRenderService.CaptureDesktop();
                    coordinates = ImageRenderService.TranslateImageToCoordinates(image);
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\nCompleted.");
                Console.ReadLine();
            }
        }
    }
}
