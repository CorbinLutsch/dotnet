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
            string line;
            string[] tokens;

            //read the input files
            using (StreamReader inFile = new StreamReader("..\\..\\items.txt"))
            {
                line = inFile.ReadLine();

                Dictionary<uint, Item> itemDictionary = new Dictionary<uint, Item>();

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

                Dictionary<uint, Player> playerDictionary = new Dictionary<uint, Player>();

                while (line != null)
                {
                    tokens = line.Split();

                   Player myPlayer = new Player(Convert.ToUInt32(tokens[0]), tokens[1], Convert.ToUInt32(tokens[2]), Convert.ToUInt32(tokens[3]), Convert.ToUInt32(tokens[4]), 
                      Convert.ToUInt32(tokens[5]),Convert.ToUInt32(tokens[6]), Convert.ToUInt32(tokens[7]), Convert.ToUInt32(tokens[8]), Convert.ToUInt32(tokens[9]),
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
       
                Dictionary<uint, string> guildDictionary = new Dictionary<uint, string>();

                while (line != null)
                {
                    tokens = line.Split();
                    guildDictionary.Add(Convert.ToUInt32(tokens[0]), tokens[1]);           
                    line = inFile.ReadLine();
                }
            }

            Menu myMenu = new Menu();
            myMenu.PrintMenu();
            myMenu.GetOption();
            
        }
    }

    public class Menu 
    {
        //default constructor for menu class
        public Menu()
        { }

        public void PrintMenu()
        {
            System.Console.WriteLine("Welcome to the World of ConflictCraft: Testing Environment!\n\n");
            System.Console.WriteLine("Welcome to World of ConflictCraft: Testing Environment. Please select an option from the list below: ");
            System.Console.WriteLine("\t1.) Print All Players");
            System.Console.WriteLine("\t2.) Print All Guilds");
            System.Console.WriteLine("\t3.) Print All Gear");
            System.Console.WriteLine("\t4.) Print Gear List for Player");
            System.Console.WriteLine("\t5.) Leave Guild");
            System.Console.WriteLine("\t6.) Join Guild");
            System.Console.WriteLine("\t7.) Equip Gear");
            System.Console.WriteLine("\t8.) Unequip Gear");
            System.Console.WriteLine("\t9.) Award Experience");
            System.Console.WriteLine("\t10.) Quit");
        }

        public void GetOption()
        {
            string line = null;

            while (line != "10" && line != "q" && line != "Q" && line != "quit" && line != "Quit" && line != "exit" && line != "Exit")
            {
                line = Console.ReadLine();
            }
        }
    }

    public class Item : Menu, IComparable
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
    }

    public class Player : Menu, IComparable
    {

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
        uint[] gear = new uint[14];
        List<uint> inventory;

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
            //is it a valid piece of gear?
            //is newGearID an item id?

            //&& does the player's level match the item requirement?       

        }

        public void UnequipGear(int gearSlot)
        {
            if (gear[gearSlot] != 0)
            {

            }
        }

    }

    
}
