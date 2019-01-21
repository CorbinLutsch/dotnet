using System;
using System.IO; //for stream reader
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mandc_Assign1
{
  
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the World of ConflictCraft: Testing Environment!\n");
            Menu myMenu = new Menu();
            myMenu.PrintMenu();
            myMenu.GetOptions();

            
        }
    }

    public class Menu 
    {
        string line;
        string[] tokens;

        Dictionary<uint, Item> itemDictionary = new Dictionary<uint, Item>();
        Dictionary<uint, Player> playerDictionary = new Dictionary<uint, Player>();
        Dictionary<uint, string> guildDictionary = new Dictionary<uint, string>();

        //default constructor for menu class
        public Menu()
        {
            //read the input files
            using (StreamReader inFile = new StreamReader("..\\..\\items.txt"))
            {
                line = inFile.ReadLine();

                while (line != null)
                {
                    tokens = line.Split('\t');

                    Item myItem = new Item(Convert.ToUInt32(tokens[0]), tokens[1], Convert.ToUInt32(tokens[2]),
                       Convert.ToUInt32(tokens[3]), Convert.ToUInt32(tokens[4]), Convert.ToUInt32(tokens[5]),
                       Convert.ToUInt32(tokens[6]), tokens[7]);

                    itemDictionary.Add(Convert.ToUInt32(tokens[0]), myItem);

                    line = inFile.ReadLine();
                }
            }

            using (StreamReader inFile = new StreamReader("..\\..\\players.txt"))
            {
                line = inFile.ReadLine();

                while (line != null)
                {
                    tokens = line.Split();

                    Player myPlayer = new Player(Convert.ToUInt32(tokens[0]), tokens[1], Convert.ToUInt32(tokens[2]), Convert.ToUInt32(tokens[3]), Convert.ToUInt32(tokens[4]),
                       Convert.ToUInt32(tokens[5]), Convert.ToUInt32(tokens[6]), Convert.ToUInt32(tokens[7]), Convert.ToUInt32(tokens[8]), Convert.ToUInt32(tokens[9]),
                       Convert.ToUInt32(tokens[10]), Convert.ToUInt32(tokens[11]), Convert.ToUInt32(tokens[12]), Convert.ToUInt32(tokens[13]),
                       Convert.ToUInt32(tokens[14]), Convert.ToUInt32(tokens[15]), Convert.ToUInt32(tokens[16]), Convert.ToUInt32(tokens[17]),
                       Convert.ToUInt32(tokens[18]), Convert.ToUInt32(tokens[19]));

                    playerDictionary.Add(Convert.ToUInt32(tokens[0]), myPlayer);

                    line = inFile.ReadLine();
                }
            }

            using (StreamReader inFile = new StreamReader("..\\..\\guilds.txt"))
            {
                line = inFile.ReadLine();

                while (line != null)
                {
                    string rebuild = null;
                    tokens = line.Split();

                    for (int i = 1; i < tokens.Length; i++)
                    {
                        if (i != tokens.Length - 1)
                            rebuild += tokens[i] + " ";
                        else
                            rebuild += tokens[i];
                    }

                    guildDictionary.Add(Convert.ToUInt32(tokens[0]), rebuild);
                    line = inFile.ReadLine();
                }
            }
        }

        public void PrintMenu()
        {
            Console.WriteLine("\nWelcome to World of ConflictCraft: Testing Environment. Please select an option from the list below: ");
            Console.WriteLine("\t1.) Print All Players");
            Console.WriteLine("\t2.) Print All Guilds");
            Console.WriteLine("\t3.) Print All Gear");
            Console.WriteLine("\t4.) Print Gear List for Player");
            Console.WriteLine("\t5.) Leave Guild");
            Console.WriteLine("\t6.) Join Guild");
            Console.WriteLine("\t7.) Equip Gear");
            Console.WriteLine("\t8.) Unequip Gear");
            Console.WriteLine("\t9.) Award Experience");
            Console.WriteLine("\t10.) Quit");
        }

        public void GetOptions()
        {
            line = null;      

            line = Console.ReadLine();

            while (line != "10" && line != "q" && line != "Q" && line != "quit" && line != "Quit" && line != "exit" && line != "Exit")
            {
                switch (line)
                {
                    case "1": //Print All Players
                        foreach (KeyValuePair<uint, Player> obj in playerDictionary)
                        {
                            //Write the override ToString player object 
                            Console.Write(obj.Value);

                            GetGuild(obj.Value);
                        }
                        break;

                    case "2": //Print All Guilds
                        foreach (KeyValuePair<uint, string> obj in guildDictionary)
                        {
                            Console.WriteLine(obj.Value);
                        }
                        break;

                    case "3": //Print All Gear
                        foreach (KeyValuePair<uint, Item> obj in itemDictionary)
                        {
                            Console.Write(obj.Value);
                        }
                        break;

                    case "4": //print gear for a specific player 
                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();
                        //LINQ query to find the associated key for the player name
                        var findKey = from K in playerDictionary where K.Value.Name == line select K.Key;
                        Player foundPlayer;

                        Item foundItem;

                        foreach (uint key in findKey)
                        {
                            playerDictionary.TryGetValue(key, out foundPlayer);
                            Console.Write(foundPlayer);
                            GetGuild(foundPlayer);

                            for (int i = 0; i < foundPlayer.gear.Length; i++)
                            {
                                itemDictionary.TryGetValue(foundPlayer.gear[i], out foundItem);
                                Console.Write(foundItem);
                            }
                        }
                        break;

                    case "5": //Leave Guild
                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();
                        //LINQ query to find the associated guild key for the player name
                        var findKey2 = from K in playerDictionary where K.Value.Name == line select K.Key;
                        Player foundPlayer2;

                        foreach (uint key in findKey2)
                        {
                            playerDictionary.TryGetValue(key, out foundPlayer2);
                            foundPlayer2.GuildId = 0;
                            Console.WriteLine("{0} has left their Guild.", foundPlayer2.Name);
                        }

                        break;

                    case "6": //Join Guild
                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();
                        //LINQ query to find the associated guild key for the player name
                        var findKey3 = from K in playerDictionary where K.Value.Name == line select K.Key;
                        Player foundPlayer3;

                        foreach (uint key in findKey3)
                        {
                            playerDictionary.TryGetValue(key, out foundPlayer3);
                            Console.Write("Enter the Guild they will join: ");
                            line = Console.ReadLine();

                            var findGuildKey = from K in guildDictionary where K.Value == line select K.Key;

                            foreach (uint key2 in findGuildKey)
                            {
                                foundPlayer3.GuildId = key2;
                                Console.WriteLine("{0} has joined {1}!", foundPlayer3.Name, line);
                            }

                        }
                        break;

                    case "7":
                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();
                        //LINQ query to find the associated guild key for the player name
                        var findKey4 = from K in playerDictionary where K.Value.Name == line select K.Key;

                        foreach (uint key in findKey4)
                        {
                            playerDictionary.TryGetValue(key, out Player foundPlayer4);

                            Console.Write("Enter the item name they will equip: ");
                            line = Console.ReadLine();
                            //run query to find the corresponding Item 
                            var findItem = from K in itemDictionary where K.Value.Name == line select K.Value;

                            foreach (Item item in findItem)
                            {
                                if (item.Requirement > foundPlayer4.Level)
                                {
                                    throw new System.ArgumentException("You do not have the required level to equip this item");
                                }
                                else
                                {
                                    int save = 0;
                                    int itemsFound = 0;
                                    for (int i = 0; i < foundPlayer4.gear.Length; i++)//loop through each gear element
                                    {
                                        //find the Item already equipped                                 
                                        itemDictionary.TryGetValue(foundPlayer4.gear[i], out Item item2);

                                        //if item to equip is equal to the same item spot already equipped
                                        if (item.Type == item2.Type)
                                        {
                                            save = i;
                                            itemsFound++;
                                        }
                                    }
                                    //if two of the same items are already equipped
                                    if (itemsFound == 2 && foundPlayer4.equiped && (int)item.Type == 11) //and we havent already equipped one
                                    {
                                        foundPlayer4.gear[save] = item.Id; //put it in the higher index
                                        foundPlayer4.equiped = false;
                                    }
                                    else if (itemsFound == 2 && !foundPlayer4.equiped && (int)item.Type == 11)//lower index
                                    {
                                        foundPlayer4.gear[save - 1] = item.Id;
                                        foundPlayer4.equiped = true;
                                    }
                                    else if (itemsFound == 2 && foundPlayer4.equiped2 && (int)item.Type == 10) //and we havent already equipped one
                                    {
                                        foundPlayer4.gear[save] = item.Id; //put it in the higher index
                                        foundPlayer4.equiped = false;
                                    }
                                    else if (itemsFound == 2 && !foundPlayer4.equiped2 && (int)item.Type == 10)//lower index
                                    {
                                        foundPlayer4.gear[save - 1] = item.Id;
                                        foundPlayer4.equiped = true;
                                    }
                                    else if (itemsFound == 1 && ((int)item.Type == 10 || (int)item.Type == 11))
                                    {
                                        if (save % 2 == 0) //then the upper index is occupied
                                        {
                                            foundPlayer4.gear[save - 1] = item.Id; //put in lower index
                                        }
                                        else
                                        {
                                            foundPlayer4.gear[save + 1] = item.Id; //put in upper index
                                        }
                                    }
                                    else //the spot is open
                                    {
                                        foundPlayer4.gear[(int)item.Type] = item.Id;
                                    }

                                    Console.Write("{0} successfully equipped {1}!", foundPlayer4.Name, item.Name);
                                }

                            }

                        }
                        break;

                    case "8": //unequip gear
                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();
                        //LINQ query to find the associated guild key for the player name
                        var findKey5 = from K in playerDictionary where K.Value.Name == line select K.Key;

                        foreach (uint key in findKey5)
                        {
                            playerDictionary.TryGetValue(key, out Player foundPlayer5);

                            Console.WriteLine("Enter the item slot number they will unequip: ");
                            Console.WriteLine("0 = Helmet");
                            Console.WriteLine("1 = Neck");
                            Console.WriteLine("2 = Shoulders");
                            Console.WriteLine("3 = Back");
                            Console.WriteLine("4 = Chest");
                            Console.WriteLine("5 = Wrist");
                            Console.WriteLine("6 = Gloves");
                            Console.WriteLine("7 = Belt");
                            Console.WriteLine("8 = Pants");
                            Console.WriteLine("9 = Boots");
                            Console.WriteLine("10 = Ring");
                            Console.WriteLine("11 = Trinket");

                            line = Console.ReadLine();
                            foundPlayer5.UnequipGear(Convert.ToInt32(line));
                        }
                        break;

                    case "9": //Award experience
                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();
                        //LINQ query to find the associated guild key for the player name
                        var findKey6 = from K in playerDictionary where K.Value.Name == line select K.Key;

                        foreach (uint key in findKey6)
                        {
                            playerDictionary.TryGetValue(key, out Player foundPlayer6);
                            Console.Write("Enter the amount of experience to award: ");
                            line = Console.ReadLine();
                            foundPlayer6.Exp += Convert.ToUInt32(line);
                            foundPlayer6.LevelUp();
                        }
                        break;
                            default:
                        break; 
                }

                PrintMenu();
                line = Console.ReadLine();
            }
        }

        public void GetGuild(Player obj)
        {
            //find the corresponding guild name
            guildDictionary.TryGetValue(obj.GuildId, out string gname);

            if (obj.GuildId != 0) //if the player is in a guild
            {
                Console.Write("Guild: ");
                Console.WriteLine(gname);
            }
            else
                Console.Write("\n");
        }


    }

    public class Item : IComparable
    {
        public enum ItemType
        {
            Helmet, Neck, Shoulders, Back, Chest,
            Wrist, Gloves, Belt, Pants, Boots,
            Ring, Trinket
        };

       private static uint MAX_LEVEL = 60;
       static uint MAX_ILLVL = 360;
       private static uint MAX_PRIMARY = 200;
       private static uint MAX_STAMINA = 275;


        private readonly uint id;
        private string name;
        private ItemType type;
        private uint ilvl;
        private uint primary;
        private uint stamina;
        private uint requirement;
        private string flavor;

        public uint Id //this is my public property
        {
            get { return id;  }
        }

        public string Name //this is my public property
        {
            get { return name; }
            set { name = value; }
        }

        public ItemType Type //this is my public property
        {
            get { return type; }

            set
            {
                if ((int)value >= 0 && (int)value <= 11)
                    type = value;
                else
                    type = 0;
            }
        }

        public uint Ilvl //this is my public property
        {
            get { return ilvl; }

            set
            {
                if (value >= 0 && value <= MAX_ILLVL)
                    ilvl = value;
                else
                    ilvl = 0;
            }
        }

        public uint Primary //this is my public property
        {
            get { return primary; }
            set
            {
                if (value >= 0 && value <= MAX_PRIMARY)
                    primary = value;
                else
                    primary = 0;
            }
        }

        public uint Stamina //this is my public property
        {
            get { return stamina; }
            set
            {
                if (value >= 0 && value <= MAX_STAMINA)
                    stamina = value;
                else
                    stamina = 0;
            }
        }

        public uint Requirement //this is my public property
        {
            get { return requirement; }
            set
            {
                if (value >= 0 && value <= MAX_LEVEL)
                    requirement = value;
                else
                    requirement = 0;
            }
        }

        public string Flavor //this is my public property
        {
            get { return flavor; }
            set { flavor = value; }
        }


        //default constructor
        public Item()
        {
            id = 0;
            name = "";
            type = 0;
            ilvl = 0;
            primary = 0;
            stamina = 0;
            requirement = 0;
            flavor = "";
        }

        //default constructor #2
        public Item(uint i, string n, uint ty, uint ilv, uint prim, uint stam,
        uint req, string flav) //ItemType ty?
        {
            id = i;
            name = n;
            type = (ItemType)ty;
            ilvl = ilv;
            primary = prim;
            stamina = stam;
            requirement = req;
            flavor = flav;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Item rightObj = obj as Item;

            if (rightObj != null)
                return name.CompareTo(rightObj.name);
            else
                throw new ArgumentException("[Item]:CompareTo argument is not an Item");
        }

        //provide an override to ToString() method
        public override string ToString()
        {
            return String.Format("({0}) {1} |{2}| --{3}--\n\t\"{4}\"\n", type, name, ilvl, requirement, flavor);

        }
    }

    public class Player : IComparable
    {
        public bool equiped = false;
        public bool equiped2 = false;

        public enum Race { Orc, Troll, Tauren, Forsaken };

        private static uint MAX_LEVEL = 60;
        private static uint GEAR_SLOTS = 14;
        private static uint MAX_INVENTORY_SIZE = 20;

        readonly uint id;
        readonly string name;
        readonly Race race;
        uint level;
        uint exp;
        uint guildID;
        public uint[] gear = new uint[14];
        List<uint> inventory = new List<uint>(); 

        public uint Id //this is my public property
        {
            get { return id; }
        }

        public string Name //this is my public property
        {
            get { return name; }
        }

        public Race Race2 //this is my public property
        {
            get { return race; }
        }

        public uint Level
        {
            get { return level; }
            set
            {
                if (value >= 0 && value <= MAX_LEVEL)
                    level = value;
                else
                    level = 0;
            }
        }

        public uint Exp
        {
            get { return exp; }
            set
            {
                exp += value;
                //if exp exceeds required experience for this player
                //to increase their level but not exceed MAX_LEVEl
                LevelUp();
            }
        }

        public uint GuildId
        {
            get { return guildID;  }
            set { guildID = value; }
        }

        public uint this[int index] //this is a public indexer for gear
        {
            get { return gear[index]; }
            set { gear[index] = value; }
        }

        public void LevelUp()
        {
            while (exp >= (level * 1000) && level != MAX_LEVEL)
            {
                exp = exp - (level * 1000);
                level++;
                Console.WriteLine("Ding!");
            }
        } //function determines how a player levels up

        public Player(uint id, string name, uint race, uint level, uint exp, uint guildID, params uint[] gear)
        {
            this.id = id;
            this.name = name;
            this.race = (Race)race;
            this.level = level;
            this.exp = exp;
            this.guildID = guildID;
            for (int i = 0; i < gear.Length; i++)
                this.gear[i] = gear[i];
        }

        public Player()
        {
            this.id = 0;
            this.name = "";
            this.race = 0;
            this.level = 0;
            this.exp = 0;
            this.guildID = 0;
            
            for (uint i = 0; i < GEAR_SLOTS; i++) 
                this.gear[i] = 0;

            this.inventory = null;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Player rightObj = obj as Player;
            if (rightObj != null)
                return name.CompareTo(rightObj.name);
            else
                throw new ArgumentException("[Player]:CompareTo argument is not a Player");
        }

        public void EquipGear(uint newGearID)
        {
           
      

        }

        public void UnequipGear(int gearSlot)
        {
            
            if (gear[gearSlot] != 0)
            {
                if (inventory.Count >= MAX_INVENTORY_SIZE)
                {
                    throw new System.ArgumentException("Inventory is full");
                }
                else
                {
                    inventory.Add(gear[gearSlot]);
                    gear[gearSlot] = 0;
                }      
            }
            else
            {
                throw new System.ArgumentException("Nothing to remove"); 
            }
            
        }

        public override string ToString()
        {
            return String.Format("Name: {0, -10} \t Race: {1, -8}  Level: {2} \t", name, race, level);

        }
    }

    
}
