using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRhapsody_Cheat
{
    internal class Program
    {
        static readonly string key = "qingyoo0316";
        static readonly byte[] keyBytes = Encoding.ASCII.GetBytes(key);
        static List<string[]> cheats = new List<string[]>();

        static void Main(string[] args)
        {
            cheats.Add(new string[] { "Walking / Running Speed", "MoveType.NORMAL?500:1e3", "MoveType.NORMAL?1500:2000" });
            cheats.Add(new string[] { "Max Physical", "this.getPhysicalMaxVal()", "1000" });
            cheats.Add(new string[] { "Ranny Everyday", "this.weatherType=f.WeatherType.SUNNY;", "this.weatherType=f.WeatherType.RAINY;" });
            cheats.Add(new string[] { "Sharp Sickle Range", "param2:170,param1:2", "param2:3000,param1:15" });
            cheats.Add(new string[] { "Fast Growing", "e.growDay=0,e.hp=n.hp", "e.growDay=5,e.hp=n.hp,this.checkPlantGrow(e)" });
            cheats.Add(new string[] { "Fast Fishing", "beginGet()},3e3", "beginGet()},10" });
            cheats.Add(new string[] { "Replace Hoeing Animate", "aniTimes=0,c.default.playEffect", "aniTimes=2,c.default.playEffect" });
            cheats.Add(new string[] { "Instant Logging", "getPlantMdl(),t=10;", "getPlantMdl(),t=100;" });
            cheats.Add(new string[] { "Replace Logging Animate", "RoleAniName.LOGGING", "RoleAniName.MOWING" });
            cheats.Add(new string[] { "Instant Mining", "t.hp-=i,", "t.hp-=i+100," });
            cheats.Add(new string[] { "Replace Mining Animate", "RoleAniName.KNOCKING", "RoleAniName.MOWING" });
            cheats.Add(new string[] { "Increase Mining Drop", "i.push([r[0],r[1]])", "i.push([r[0],r[1]])&&i.push([r[0],r[1]])&&i.push([r[0],r[1]])" });
            cheats.Add(new string[] { "Increase Mining Luck", ".randomInt(1,100)<", ".randomInt(1,5)<" });

            var files = Directory.GetFiles(".", "*.*", SearchOption.AllDirectories);
            Parallel.ForEach(files, (_) => Decrypt(_));

            files = Directory.GetFiles(".", "index.*.js", SearchOption.AllDirectories);
            foreach (var file in files) Cheat(file);

            Console.WriteLine("Patch finish! Press any key to exit...");
            Console.ReadKey();
        }

        static void Cheat(string file)
        {
            Console.WriteLine(file);
            var text = File.ReadAllText(file);

            var idx = 1;
            foreach (var cheat in cheats)
            {
                if (!text.Contains(cheat[1])) continue;

                Console.Write($"Enable cheat: #{idx++} {cheat[0]} ?\t(Y/n)");
                var input = (Console.ReadKey().Key == ConsoleKey.Y);
                Console.WriteLine();

                if (!input) continue;
                text = text.Replace(cheat[1], cheat[2]);
            }

            File.WriteAllText(file, text);
        }

        static void Decrypt(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            if (!ext.EndsWith("js")) return;

            var bytes = File.ReadAllBytes(file);
            var sign = bytes.Take(key.Length).ToArray();

            if (!sign.SequenceEqual(keyBytes)) return;

            bytes = bytes.Skip(key.Length).ToArray();
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= keyBytes[i % key.Length];
            }

            File.WriteAllBytes(file, bytes);
            Console.WriteLine(file);
        }
    }
}
