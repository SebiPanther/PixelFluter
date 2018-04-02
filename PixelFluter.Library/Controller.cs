using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PixelFluter.Library
{
    public class Controller
    {
        private static Random Random = new Random();  
        public void Run(string[] parameters)
        {
            //SIZE 1920 1080
            var config = new Config()
            {
                Ip = IPAddress.Parse(parameters[0]), //"94.45.232.48"
                Port = int.Parse(parameters[1]), //1234,
                ClientCount = int.Parse(parameters[2]), //5,
                OffsetX = int.Parse(parameters[3]), //250,
                OffsetY = int.Parse(parameters[4]), //450,
                ImageFile = parameters[5] //"ChaosWest-Logo.png"
            };
            Console.WriteLine("Config:");
            Console.WriteLine("- Ip: {0}", config.Ip);
            Console.WriteLine("- Port: {0}", config.Port);
            Console.WriteLine("- ClientCount: {0}", config.ClientCount);
            Console.WriteLine("- OffsetX: {0}", config.OffsetX);
            Console.WriteLine("- OffsetY: {0}", config.OffsetY);
            Console.WriteLine("- ImageFile: {0}", config.ImageFile);

            Console.WriteLine("Open File");
            var pixels = new ImageRepository().ReadImage(config.ImageFile);

            //Optimize Pixels
            Optimize(pixels);

            Console.WriteLine("Convert to Protocol");
            var commands = new CommandRepository().Convert(config.OffsetX, config.OffsetY, pixels);
            
            Console.WriteLine("Build Packages");
            var cmdParts = BuildPackages(commands, config.ClientCount);

            Console.WriteLine("Fire! ClientCount: {0}", config.ClientCount);
            Parallel.ForEach(cmdParts, new ParallelOptions { MaxDegreeOfParallelism = config.ClientCount }, (cmdPart) => {
                new SendClient().Send(cmdPart, config);
            });
        }

        private void Optimize(List<Pixel> pixels)
        {
            int n = pixels.Count;  
            while (n > 1) {  
                n--;  
                int k = Random.Next(n + 1);  
                Pixel value = pixels[k];  
                pixels[k] = pixels[n];  
                pixels[n] = value;  
            }
        }

        private List<PixelPackage> BuildPackages(List<string> commands, int clientCount)
        {
            return commands.Select((x, i) => new { Index = i, Value = x})
                    .GroupBy(x => x.Index / (commands.Count / clientCount))
                    .Select(x => 
                        new PixelPackage {
                            Index = x.Key,
                            Bytes = Encoding.UTF8.GetBytes(string.Join("\n", x.Select(v => v.Value))) 
                        }
                    )
                    .ToList();
        }
    }
}