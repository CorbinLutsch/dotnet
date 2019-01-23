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

    public static class Global
    {
        public static Dictionary<uint, Item> itemDictionary = new Dictionary<uint, Item>();
        public static Dictionary<uint, Player> playerDictionary = new Dictionary<uint, Player>();
        public static Dictionary<uint, string> guildDictionary = new Dictionary<uint, string>();
    }

    public class Menu 
    {
        string line;
        string[] tokens;

        //default constructor for menu class
        public Menu()
        {
            //read the item input file
            using (StreamReader inFile = new StreamReader("..\\..\\items.txt"))
            {
                line = inFile.ReadLine();

                while (line != null)
                {
                    //split the string read in
                    tokens = line.Split('\t');

                    //create an item object
                    Item myItem = new Item(Convert.ToUInt32(tokens[0]), tokens[1], Convert.ToUInt32(tokens[2]),
                       Convert.ToUInt32(tokens[3]), Convert.ToUInt32(tokens[4]), Convert.ToUInt32(tokens[5]),
                       Convert.ToUInt32(tokens[6]), tokens[7]);

                    Global.itemDictionary.Add(Convert.ToUInt32(tokens[0]), myItem);

                    line = inFile.ReadLine();
                }
            }

            //read the players input file
            using (StreamReader inFile = new StreamReader("..\\..\\players.txt"))
            {
                line = inFile.ReadLine();

                while (line != null)
                {
                    tokens = line.Split(); //again, splitting the string up 

                    //tokens must be converted to the proper value for each attribute in the Player class
                    Player myPlayer = new Player(Convert.ToUInt32(tokens[0]), tokens[1], Convert.ToUInt32(tokens[2]), Convert.ToUInt32(tokens[3]), Convert.ToUInt32(tokens[4]),
                       Convert.ToUInt32(tokens[5]), Convert.ToUInt32(tokens[6]), Convert.ToUInt32(tokens[7]), Convert.ToUInt32(tokens[8]), Convert.ToUInt32(tokens[9]),
                       Convert.ToUInt32(tokens[10]), Convert.ToUInt32(tokens[11]), Convert.ToUInt32(tokens[12]), Convert.ToUInt32(tokens[13]),
                       Convert.ToUInt32(tokens[14]), Convert.ToUInt32(tokens[15]), Convert.ToUInt32(tokens[16]), Convert.ToUInt32(tokens[17]),
                       Convert.ToUInt32(tokens[18]), Convert.ToUInt32(tokens[19]));

                    Global.playerDictionary.Add(Convert.ToUInt32(tokens[0]), myPlayer);

                    line = inFile.ReadLine();
                }
            }

            //read in the guilds text file
            using (StreamReader inFile = new StreamReader("..\\..\\guilds.txt"))
            {
                line = inFile.ReadLine();

                while (line != null)
                {
                    string rebuild = null; //to hold the rebuilded string
                    tokens = line.Split();//seperate the guild ID from the rest of the string

                    //now piece back the guild name since guilds can be more than one word 
                    //seperated by spaces 
                    for (int i = 1; i < tokens.Length; i++)
                    {
                        if (i != tokens.Length - 1)
                            rebuild += tokens[i] + " ";
                        else
                            rebuild += tokens[i];
                    }

                    Global.guildDictionary.Add(Convert.ToUInt32(tokens[0]), rebuild);
                    line = inFile.ReadLine();
                }
            }
        }

        //method to print the options
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
            line = Console.ReadLine();

            while (line != "10" && line != "q" && line != "Q" && line != "quit" && line != "Quit" && line != "exit" && line != "Exit")
            {
                switch (line)
                {
                    case "1": //Print All Players

                        foreach (KeyValuePair<uint, Player> obj in Global.playerDictionary)
                        {                        
                            Console.Write(obj.Value); //call to the Players override ToString() method

                            GetGuild(obj.Value);//prints the guild associated with the player
                        }
                        break;

                    case "2": //Print All Guilds

                        foreach (KeyValuePair<uint, string> obj in Global.guildDictionary)
                        {
                            Console.WriteLine(obj.Value); 
                        }
                        break;

                    case "3": //Print All Gear

                        foreach (KeyValuePair<uint, Item> obj in Global.itemDictionary)
                        {
                            Console.Write(obj.Value); //call to Items override ToString() method
                        }
                        break;

                    case "4": //print gear for a specific player 

                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();

                        //LINQ query to find the associated player object
                        var findKey = from K in Global.playerDictionary where K.Value.Name == line select K.Key;

                        foreach (uint key in findKey)
                        {
                            Global.playerDictionary.TryGetValue(key, out Player foundPlayer); //get the player object associated with the name
                            Console.Write(foundPlayer); //print the player's information                           
                            GetGuild(foundPlayer); //including the guild their in

                            for (int i = 0; i < foundPlayer.gear.Length; i++)
                            {
                                Global.itemDictionary.TryGetValue(foundPlayer.gear[i], out Item foundItem); //find each Item object that the player is wearing 
                                Console.Write(foundItem); //print the item's information
                            }
                        }
                        break;

                    case "5": //Leave Guild

                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();

                        //LINQ query to find the associated player object
                        var findKey2 = from K in Global.playerDictionary where K.Value.Name == line select K.Key;    

                        foreach (uint key in findKey2)
                        {
                            Global.playerDictionary.TryGetValue(key, out Player foundPlayer2);

                            foundPlayer2.GuildId = 0; //set the guildId to 0 which means "no guild" in this context
                            Console.WriteLine("{0} has left their Guild.", foundPlayer2.Name);
                        }

                        break;

                    case "6": //Join Guild

                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();

                        //LINQ query to find the associated Player object
                        var findKey3 = from K in Global.playerDictionary where K.Value.Name == line select K.Key;

                        foreach (uint key in findKey3)
                        {
                            Global.playerDictionary.TryGetValue(key, out Player foundPlayer3);

                            Console.Write("Enter the Guild they will join: ");
                            line = Console.ReadLine();

                            //LINQ query to find the guild key associated with the name of the guild to join
                            var findGuildKey = from K in Global.guildDictionary where K.Value == line select K.Key;

                            foreach (uint key2 in findGuildKey)
                            {
                                foundPlayer3.GuildId = key2; //set the new guildId for that player
                                Console.WriteLine("{0} has joined {1}!", foundPlayer3.Name, line);
                            }

                        }
                        break;

                    case "7": //Equip Item

                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();

                        //LINQ query to find the associated Player object
                        var findKey4 = from K in Global.playerDictionary where K.Value.Name == line select K.Key;

                        foreach (uint key in findKey4)
                        { //gets the Player object for the person entered
                            Global.playerDictionary.TryGetValue(key, out Player foundPlayer4);

                            Console.Write("Enter the item name they will equip: ");
                            line = Console.ReadLine();

                            //run query to find the corresponding Item object
                            var findItem = from K in Global.itemDictionary where K.Value.Name == line select K.Key;

                            foreach (uint key2 in findItem)
                            {
                                foundPlayer4.EquipGear(key2);
                            }
                        }
                        break;

                    case "8": //Unequip gear

                        Console.Write("Enter the player name: ");
                        line = Console.ReadLine();

                        //LINQ query to find the associated guild key for the player name
                        var findKey5 = from K in Global.playerDictionary where K.Value.Name == line select K.Key;

                        foreach (uint key in findKey5)
                        {
                            Global.playerDictionary.TryGetValue(key, out Player foundPlayer5);

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
                        var findKey6 = from K in Global.playerDictionary where K.Value.Name == line select K.Key;

                        foreach (uint key in findKey6)
                        {
                            Global.playerDictionary.TryGetValue(key, out Player foundPlayer6);

                            Console.Write("Enter the amount of experience to award: ");
                            line = Console.ReadLine();
                            foundPlayer6.Exp += Convert.ToUInt32(line);
                        }
                        break;

                    case "T": //Sort the Players and Items "hidden option"

                        SortedSet<Player> SortedPlayers = new SortedSet<Player>();
                        SortedSet<Item> SortedItems = new SortedSet<Item>();
                        foreach(KeyValuePair<uint, Player> p in Global.playerDictionary)
                        {
                            SortedPlayers.Add(p.Value);
                        }

                        foreach (KeyValuePair<uint, Item> i in Global.itemDictionary)
                        {
                            SortedItems.Add(i.Value);
                        }
                        foreach(Item i in SortedItems)
                        {
                            Console.Write(i);
                        }
                        foreach(Player p in SortedPlayers)
                        {
                            Console.Write(p);
                            GetGuild(p);
                        }
                        break;

                 default:
                        break; 
                } //end of switch statement

                PrintMenu(); //reprint the menu
                line = Console.ReadLine(); //get user input
            }//end of while loop
        }

        public void GetGuild(Player obj)
        {
            //find the corresponding guild name
            Global.guildDictionary.TryGetValue(obj.GuildId, out string gname);

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
        uint req, string flav) 
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
                while (exp >= (level * 1000) && level != MAX_LEVEL)
                {
                    LevelUp();
                }
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
                exp = exp - (level * 1000);
                level++;
                Console.WriteLine("Ding!");
            
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
            //LINQ to find the item object we will equip
            var findItem = from K in Global.itemDictionary where K.Key == newGearID select K.Value;

            foreach (Item item in findItem)
            {
                if (item.Requirement > Level)
                {
                    throw new System.ArgumentException("You do not have the required level to equip this item");
                }
                else
                {
                    int save = 0;
                    int itemsFound = 0;

                    for (int i = 0; i < gear.Length; i++)//loop through each gear element
                    {
                        //Find the corresponding Item object                              
                        Global.itemDictionary.TryGetValue(gear[i], out Item item2);

                        //if item to equip is equal to the same item spot already equipped
                        //e.g. Helmet == Helmet, or Ring == Ring, then we know this is the correct spot to put it
                        if (item.Type == item2.Type)
                        {
                            save = i;
                            itemsFound++; //keep track of how many times we encounter the item
                        }
                    }

                    //if two of the same items are already equipped and it's a trinket
                    if (itemsFound == 2 && equiped && (int)item.Type == 11) //and we have already equipped once before
                    {
                        gear[save] = item.Id; //put it in the higher index
                        equiped = false;
                    }
                    else if (itemsFound == 2 && !equiped && (int)item.Type == 11)//two of the same items, trinket, havent equipped
                    {
                        gear[save - 1] = item.Id; //put it in the lower index 
                        equiped = true;
                    }
                    else if (itemsFound == 2 && equiped2 && (int)item.Type == 10) //same thing but with the ring location
                    {
                        gear[save] = item.Id; //put it in the higher index
                        equiped = false;
                    }
                    else if (itemsFound == 2 && !equiped2 && (int)item.Type == 10)
                    {
                        gear[save - 1] = item.Id; //put it in the lwoer index
                        equiped = true;
                    }
                    else if (itemsFound == 1 && ((int)item.Type == 10 || (int)item.Type == 11)) //if only 1 ring or 1 trinket currently equipped
                    {
                        if (save % 2 == 0) //then the upper index is occupied
                        {
                            gear[save - 1] = item.Id; //put in lower index
                        }
                        else //the lower index is occupied
                        {
                            gear[save + 1] = item.Id; //put in upper index
                        }
                    }
                    else //the spot is open
                    {
                        gear[(int)item.Type] = item.Id; //SO JUST PUT THE DAMN THING IN THERE SHEESH!
                    }

                    Console.WriteLine("{0} successfully equipped {1}!", Name, item.Name);
                }

            }
        }

        public void UnequipGear(int gearSlot)
        {
            
            if (gear[gearSlot] != 0) //if the gear slot is not empty
            {
                if (inventory.Count >= MAX_INVENTORY_SIZE) //is the inventory full?
                {
                    throw new System.ArgumentException("Inventory is full");
                }
                else
                {
                    inventory.Add(gear[gearSlot]); //add the item to the inventory
                    gear[gearSlot] = 0; //set the gear slot to empty
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
