using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonRPG
{
    public class Inventory
    {
        private static Inventory instance;

        public List<Item> inventoryItemList = new List<Item>();
        //인벤토리내에서 장착하는 슬롯 
        public int weaponItemNum;
        public int armorItemNum;

        public static Inventory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Inventory();
                }
                return instance;
            }
        }
        public Inventory()
        {
            weaponItemNum = 0;
            armorItemNum = 0;
        }
        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Item item in inventoryItemList)
                {
                    writer.WriteLine($"{item.itemName},{item.itemAbility},{item.itemInfo},{item.itemPrice},{item.type},{item.isEquip},{item.isSell}");
                }

                writer.WriteLine(weaponItemNum);
                writer.WriteLine(armorItemNum);


                /*writer.WriteLine(weaponItemNum);
                writer.WriteLine(armorItemNum);*/

            }
            //Console.WriteLine("Inventory 데이터가 저장되었습니다.");
            //Console.WriteLine();
        }

        public void LoadToCharacter(string filePath)
        {
            inventoryItemList.Clear(); //초기화시키고 불러와야함
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length == 1)
                    {
                        weaponItemNum = int.Parse(line);
                        armorItemNum = int.Parse(reader.ReadLine());
                        if (weaponItemNum != 0)
                            Equip(weaponItemNum);
                        if (armorItemNum != 0)
                            Equip(armorItemNum);

                        break;
                    }
                    else
                    {
                        string[] itemScript = line.Split(',');
                        string itemName = itemScript[0];
                        int itemAbility = int.Parse(itemScript[1]);
                        string itemInfo = itemScript[2];
                        string itemPrice = itemScript[3];
                        int type = int.Parse(itemScript[4]);
                        bool isEquip = bool.Parse(itemScript[5]);
                        bool isSell = bool.Parse(itemScript[6]);


                        inventoryItemList.Add(new Item(itemName, itemAbility, itemInfo, itemPrice, type, isEquip, isSell));
                    }
                }
            }
            //Console.WriteLine("Inventory 데이터를 불러왔습니다.");
            //Console.WriteLine();
        }
        public void ShowInventoryInfo()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine();

                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    Console.WriteLine($"- {inventoryItemList[i].itemName}  | {inventoryItemList[i].getItemStat()} +{inventoryItemList[i].itemAbility} | {inventoryItemList[i].itemInfo} | {inventoryItemList[i].itemPrice}");
                }

                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        return;
                    case "1":
                        this.Equipment();
                        return;
                }
            }
        }

        //장착 관리 
        public void Equipment()
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("[인벤토리 - 장착 관리]");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].isEquip)
                    {
                        if (inventoryItemList[i].type == 1)
                        {
                            weaponItemNum = i + 1;
                        }
                        else if (inventoryItemList[i].type == 2)
                        {
                            armorItemNum = i + 1;
                        }
                        Console.WriteLine($"- {i + 1} [E]{inventoryItemList[i].itemName}  | {inventoryItemList[i].getItemStat()} +{inventoryItemList[i].itemAbility} | {inventoryItemList[i].itemInfo} | {inventoryItemList[i].itemPrice}");
                    }
                    else
                    {
                        Console.WriteLine($"- {i + 1} {inventoryItemList[i].itemName}  | {inventoryItemList[i].getItemStat()} +{inventoryItemList[i].itemAbility} | {inventoryItemList[i].itemInfo} | {inventoryItemList[i].itemPrice}");
                    }
                }
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                //고른아이템
                string input = Console.ReadLine();
                Console.WriteLine();

                if (input == "0")
                    return;

                int itemNum = int.Parse(input);

                if (itemNum < 0 || itemNum > inventoryItemList.Count)
                {
                    Console.WriteLine("잘못 입력하셨습니다");
                    Console.WriteLine();
                    continue;
                }
                //선택한 아이템 장착
                if (!inventoryItemList[itemNum - 1].isEquip)
                {
                    if (inventoryItemList[itemNum - 1].type == 1)
                    {
                        if (weaponItemNum != 0) //이미 장착되어있음
                        {
                            DisEquip(weaponItemNum);
                        }
                    }
                    if (inventoryItemList[itemNum - 1].type == 2)
                    {
                        if (armorItemNum != 0) //이미 장착되어있음
                        {
                            DisEquip(armorItemNum);
                        }
                    }
                    this.Equip(itemNum);
                }
                //장착해제
                else if (inventoryItemList[itemNum - 1].isEquip)
                    this.DisEquip(itemNum);
            }
        }

        public void Equip(int itemNum)
        {
            //선택한 아이템 장착
            //Console.WriteLine("장착");
            inventoryItemList[itemNum - 1].isEquip = true;
            int ab_type = inventoryItemList[itemNum - 1].type;
            int ab_amount = inventoryItemList[itemNum - 1].itemAbility;
            switch (ab_type)
            {
                case 1:
                    Character.Instance.add_attack_ab += ab_amount;
                    break;
                case 2:
                    Character.Instance.add_defense_ab += ab_amount;
                    break;
            }
        }

        public void DisEquip(int itemNum)
        {
            inventoryItemList[itemNum - 1].isEquip = false;
            int ab_type = inventoryItemList[itemNum - 1].type;
            int ab_amount = inventoryItemList[itemNum - 1].itemAbility;
            switch (ab_type)
            {
                case 1:
                    Character.Instance.add_attack_ab -= ab_amount;
                    weaponItemNum = 0;
                    break;
                case 2:
                    Character.Instance.add_defense_ab -= ab_amount;
                    armorItemNum = 0;
                    break;
            }
        }
    }
}
