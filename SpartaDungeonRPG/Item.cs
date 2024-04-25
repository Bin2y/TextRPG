using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace SpartaDungeonRPG
{
    public class Item
    {

        public string itemName { get; set; }
        public int itemAbility { get; set; }
        public string itemInfo { get; set; }
        public string itemPrice { get; set; }
        public int type; //1이면 공격력 2이면 방어력
        public bool isEquip;
        public bool isSell;

        public Item()
        {
            itemName = "Unkown";
            itemAbility = 0;
            itemInfo = "정보가없습니다.";
            itemPrice = "0";
            type = 0;
            isEquip = false;
            isSell = false;

        }
        public Item(string itemName, int itemAbility, string itemInfo, string itemPrice, int type, bool isEquip, bool isSell)
        {
            this.itemName = itemName;
            this.itemAbility = itemAbility;
            this.itemInfo = itemInfo;
            this.itemPrice = itemPrice;
            this.type = type;
            this.isEquip = isEquip;
            this.isSell = isSell;
        }

        public string getItemStat()
        {
            switch (type)
            {
                case 1:
                    return "공격력";
                case 2:
                    return "방어력";
                default:
                    return "No Stat";
            }

        }
    }
    
}
