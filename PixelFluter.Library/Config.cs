using System.Net;

namespace PixelFluter.Library
{
    public class Config
    {
        
            public IPAddress Ip { get; set; }
            public int Port { get; set; }
            public int ClientCount { get; set; }
            public int OffsetX { get; set; }
            public int OffsetY { get; set; }
            public string ImageFile { get; set; }
    }
}