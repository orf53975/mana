//using BattleSystem;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using xUtil;

//namespace BattleServer.Main
//{
//    public static class Test
//    {
//        public static void DoDeleteFile()
//        {
 
//            var fileList = File.ReadAllLines(@"E:\work\GameEditors\GameEditor\data\spells\.list");
//            var dics = new Dictionary<string , string>();
//            foreach(var str in fileList)
//            {
//                var tmp = str.Split(';');
//                dics.Add(tmp[1], tmp[0]);
//            }
//            var dir = new DirectoryInfo(@"E:\work\GameEditors\GameEditor\data\spells");
//            var allFiles = dir.GetFiles();
//            foreach(var f in allFiles)
//            {
//                var str = Path.GetFileNameWithoutExtension(f.Name);
//                if (!dics.ContainsKey(str))
//                {
//                    if(f.Extension == ".xml")
//                    {
//                        f.Delete();
//                    }
//                }
//            }
//        }


//        static int BinarySearch(List<Battle> battles, long battleId)
//        {
//            int start = 0, end = battles.Count - 1, mid;
//            while (start <= end)
//            {
//                mid = (start + end) / 2;
//                if (battles[mid].UUID < battleId)
//                {
//                    start = mid + 1;
//                }
//                else if (battles[mid].UUID > battleId)
//                {
//                    end = mid - 1;
//                }
//                else
//                {
//                    return mid;
//                }
//            }
//            return -1;
//        }

//        public static void TestBinarySearch()
//        {
//            var battles = new List<Battle>();
//            Random r = new Random();
//            for (int i = 0; i < 256; i++)
//            {
//                //long id = r.Next(0, 0xFFFF) << 32;
//                //id += r.Next();
//                long id = r.Next(0, 0x3FF);
//                var battle = new Battle(new CreatBattleRequest() {
//                    uuid = id
//                });
//                var index = xUtil.Util.BinarySearch(battles, (e) => e.UUID - battle.UUID);
//                //int index = battles.BinarySearch(battle);
//                if (index >= 0)
//                {
//                    Trace.TraceError("Battle UUID[{0}] conflict", battle.UUID);
//                }
//                else
//                {
//                    battles.Insert(~index, battle);
//                }
//            }

//            foreach (var b in battles)
//            {
//                Trace.TraceInformation(b.UUID.ToString());
//            }

//            var findId = 34;
//            var find = xUtil.Util.BinarySearch(battles, (e) => e.UUID - findId);
//            if(find < 0)
//            {
//                Trace.TraceInformation("Find Battle:{0} failed! insert index = {1}", findId, ~find);
//            }
//            else
//            {
//                Trace.TraceInformation("Find Battle:{0} , index = {1}", findId, find);
//            }
//        }


//    }
//}