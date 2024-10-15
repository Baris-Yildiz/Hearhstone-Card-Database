using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
namespace Hearthstone_Card_Database_Creator
{
    class Program
    {
        //minions, spells, weapons, heros
        static void Main(string[] args)
        {
            string Spells = "Class,Cost,dbfId,ID,Name,Rarity,Expansion,SpellSchool,Type\n";
            string Weapons = string.Empty;
            string Heros = string.Empty;
            string Minions = string.Empty;
            string SpellContent = string.Empty;
            string WeaponContent = string.Empty;
            string HeroContent = string.Empty;
            string MinionsContent = string.Empty;
            string[] CardInformationList = new string[] { };
            string[] CardProperties = new string[] { };
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (File.Exists(@"\bin\Debug\netcoreapp3.1\CardsNew.txt"))
            {
                string jsonFile = File.ReadAllText(@"\bin\Debug\netcoreapp3.1\CardsNew.txt");
                CardInformationList = jsonFile.Split("},");
            }

            for (int i=0; i<CardInformationList.Count()-1; i++)
            {
                CardInformationList[i] = CardInformationList[i].Remove(0, 2);
                CardProperties = CardInformationList[i].Split(','.ToString() + '"'.ToString());
                foreach(string info in CardProperties)
                {
                    if (!info.Contains(':'))
                    {
                        continue;
                    }
                    int splitIndex = info.IndexOf(':');
                    dict[info.Substring(0, splitIndex-1)] = info.Substring(splitIndex+1, info.Length - splitIndex-1);
                }

                dict.Remove("artist");
                //dict.Remove("flavor");
                dict.Remove("text");
                dict.Remove("collectible");
                dict.Remove("mechanics");
                dict.Remove("referencedTags");
                dict.Remove("overload");
                dict.Remove("spellSchool");
                dict.Remove("elite");
                dict.Remove("howToEarnGolden");
                dict.Remove("howToEarn");
                dict.Remove("collectionText");
                dict.Remove("classes");
                dict.Remove("isMiniSet");
                dict.Remove("targetingArrowText");
                dict.Remove("faction");
                dict.Remove("hideStats");
                dict.Remove("questReward");
                dict.Remove("race");
                dict.Remove("spellDamage");
                dict.Remove("techLevel");
                dict.Remove("battlegroundsPremiumDbfId");
                dict.Remove("hasDiamondSkin");
                dict.Remove("isBattlegroundsPoolMinion");
                dict.Remove("battlegroundsDarkmoonPrizeTurn");
                dict.Remove("armor");
                dict.Remove("isBattlegroundsPoolMinion");
                if (dict["name"].Contains(',')) dict["name"] = dict["name"].Replace(',', ' ');
                if (dict.ContainsKey("multiClassGroup"))
                {
                    dict["cardClass"] = dict["multiClassGroup"];
                    dict.Remove("multiClassGroup");
                }

                if (dict["type"] =='"'.ToString()+ "SPELL"+'"'.ToString())
                {
                    dict.Remove("attack");
                    dict.Remove("health");
                    SpellContent = string.Join(",", dict.Values);
                    SpellContent += "\n";
                    Spells += SpellContent;

                }
                else if (dict["type"] == '"'.ToString() + "MINION" + '"'.ToString())
                {
                    if (!dict.ContainsKey("attack")) dict["attack"] = "0";
                    Console.WriteLine(string.Join(",",dict.Keys));
                    MinionsContent = string.Join(",", dict.Values) + "\n";
                    Minions += MinionsContent;
                }
                else if (dict["set"] != '"'.ToString() + "HERO_SKINS" + '"'.ToString() && dict["type"] == '"'.ToString() + "HERO" + '"'.ToString())
                {
                    HeroContent = string.Join(",", dict.Values) + "\n";
                    Heros += HeroContent;
                }
                else if (dict["type"] == '"'.ToString() + "WEAPON" + '"'.ToString())
                {
                    dict.Remove("health");
                    WeaponContent = string.Join(",", dict.Values);
                    WeaponContent += "\n";
                    Weapons += WeaponContent;
                }
                dict.Clear();
            }
            File.WriteAllText(@"C:\xampp\mysql\data\hearthstone\SpellDataBase.txt", Spells);
            File.WriteAllText(@"C:\xampp\mysql\data\hearthstone\WeaponDataBase.txt", Weapons);
            File.WriteAllText(@"C:\xampp\mysql\data\hearthstone\HeroDataBase.txt", Heros);
            File.WriteAllText(@"C:\xampp\mysql\data\hearthstone\MinionDataBase.txt", Minions);
        }
    }
}
