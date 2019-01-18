using System;
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
        private static uint MAX_ILLVL = 360;
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
                if ((int)value >= 0 && (int)value <= 12)
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
        public Item(uint i, string n, ItemType ty, uint ilv, uint prim, uint stam,
        uint req, string flav)
        {
            id = i;
            name = n;
            type = ty;
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

    public class Player : Item, IComparable
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
        uint[] gear;
        List<uint> inventory;

        public uint Id2 //this is my public property
        {
            get { return id; }
        }

        public string Name2 //this is my public property
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

        public uint this[uint index] //this is a public indexer for gear
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

        public Player(uint id, string name, Race race, uint level, uint exp, uint guildID, uint[] gear, List<uint> inventory)
        {
            this.id = id;
            this.name = name;
            this.race = race;
            this.level = level;
            this.exp = exp;
            this.guildID = guildID;
            for (uint i = 0; i < gear.Length; i++)
                this.gear[i] = gear[i];
            this.inventory = inventory;
        }

        public Player()
        {
            this.id = 0;
            this.name = "";
            this.race = 0;
            this.level = 0;
            this.exp = 0;
            this.guildID = 0;
            this.gear = null; 
            this.inventory = null;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Player rightObj = obj as Player;
            if (rightObj != null)
                return name.CompareTo(rightObj.name);
            else
                throw new ArgumentException("[Player]:CompareTo argument is not an Item");
        }

        public void EquipGear(uint newGearID)
        {
            //is it a valid piece of gear?
            if (Item.id == newGearId)
            

        }
    }

    
}
