using mana.Foundation;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace TestClient
{
    class Program
    {
        private static readonly Regex REGEX = new Regex(@"\w+|");

        static void Main(string[] args)
        {
            try
            {
                CustomClient c = testConnect();

                testConnect();
                testConnect();
                testConnect();
                testConnect();
                testConnect();
                testConnect();

                while (true)
                {

                    Console.Write("send>");
                    string msg = Console.ReadLine();
                    if (msg == "exit")
                    {
                        break;
                    }
                    else if (REGEX.IsMatch(msg))
                    {
                        var match = REGEX.Match(msg);
                        var times = int.Parse(match.Value);
                        int index = 0;
                        while (index < times)
                        {
                            c.send(CreatTestPacket(index));
                            index++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("invalid command:{0}" , msg);
                    }
                }
                c.disconnect();
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("Press any key to exit....");
            Console.ReadKey();
        }

        public static CustomClient testConnect()
        {
            CustomClient c = new CustomClient(8088, IPAddress.Parse("192.168.102.90"));
            c.connect();
            Console.WriteLine("服务器连接成功!");
            var bp = CreatBindPlayerPacket(Guid.NewGuid().ToString());
            c.send(bp);
            c.startRecive();
            return c;
        }


        public static Packet CreatBindPlayerPacket(string player_uuid)
        {
            var p = new Packet(0x1001);
            p.putString(player_uuid);
            return p;
        }

        public static Packet CreatTestPacket(int index)
        {
            var p = new Packet((ushort)(0x2000 + index));

            var n = (index % 5) + 1;
            var fileName = "sendTest" + n + ".txt";
            var bytesdat = File.ReadAllText(fileName);
            if (bytesdat.Length > 0xFFF)
            {
                p.putString(bytesdat.Substring(0, 0xFFF));
            }
            else
            {
                p.putString(bytesdat);
            }
            return p;
        }
    }
}
