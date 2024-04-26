using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonRPG
{
    public class Store
    {
        private static Store instance;
        public List<Item> storeItems = new List<Item>();
        public static Store Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Store();
                }
                return instance;
            }
        }
        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Item item in storeItems)
                {
                    writer.WriteLine($"{item.itemName},{item.itemAbility},{item.itemInfo},{item.itemPrice},{item.type},{item.isEquip},{item.isSell}");
                }
            }
            //Console.WriteLine("Store 데이터가 저장되었습니다.");

        }

        public void LoadToCharacter(string filePath)
        {
            storeItems.Clear();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {

                    string[] itemScript = line.Split(',');
                    string itemName = itemScript[0];
                    int itemAbility = int.Parse(itemScript[1]);
                    string itemInfo = itemScript[2];
                    string itemPrice = itemScript[3];
                    int type = int.Parse(itemScript[4]);
                    bool isEquip = bool.Parse(itemScript[5]);
                    bool isSell = bool.Parse(itemScript[6]);
                    storeItems.Add(new Item(itemName, itemAbility, itemInfo, itemPrice, type, isEquip, isSell));
                }


            }
            // Console.WriteLine("Store 데이터를 불러왔습니다.");

        }
        public void ShowStoreInfo()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("[상점]");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{Character.Instance.moneyAmount} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < storeItems.Count; i++)
                {
                    Console.WriteLine($"- {storeItems[i].itemName}  | {storeItems[i].getItemStat()} +{storeItems[i].itemAbility} | {storeItems[i].itemInfo} | {storeItems[i].itemPrice}");
                }

                Console.WriteLine();
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "0":
                        return; //메인화면으로 이동
                    case "1":
                        this.PurchaseItem();
                        break;
                    case "2":
                        this.SellItem();
                        break;
                }
            }
        }
        public void PurchaseItem()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("[상점 - 아이템 구매]");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                int money = Character.Instance.moneyAmount;
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{money} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < storeItems.Count; i++)
                {
                    if (storeItems[i].isSell)//이미 팔렸다면
                    {
                        Console.WriteLine($"- {i + 1} {storeItems[i].itemName}  | {storeItems[i].getItemStat()} +{storeItems[i].itemAbility} | {storeItems[i].itemInfo} | 구매완료");
                    }
                    else
                    {
                        Console.WriteLine($"- {i + 1} {storeItems[i].itemName}  | {storeItems[i].getItemStat()} +{storeItems[i].itemAbility} | {storeItems[i].itemInfo} | {storeItems[i].itemPrice}");
                    }
                }
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                string input = Console.ReadLine();

                Console.WriteLine();


                if (input == "0")
                    break; //상점으로 이동
                else
                {
                    if (input == null || int.Parse(input) > storeItems.Count) //상점보유수보다 많은 숫자를 선택시
                    {
                        Console.WriteLine("!!!잘못된 입력입니다 아이템 번호를 입력해주세요.!!!");
                        Console.WriteLine();
                        Thread.Sleep(500);
                        continue;
                    }

                    int itemNum = int.Parse(input);
                    int ItemPrice = int.Parse(storeItems[itemNum - 1].itemPrice);
                    if (storeItems[itemNum - 1].isSell) //이미 팔렸다면
                    {
                        Console.WriteLine("이미 팔린 물품입니다.");
                        Thread.Sleep(500);
                        continue;
                    }
                    if (money >= ItemPrice)
                    {
                        Character.Instance.moneyAmount -= ItemPrice;
                        //Console.WriteLine($"{Character.Instance.moneyAmount}, {ItemPrice}");
                        storeItems[itemNum - 1].isSell = true;
                        Console.WriteLine($"[{storeItems[itemNum - 1].itemName}] 구매완료!");
                        Console.WriteLine();
                        Thread.Sleep(500);
                        Inventory.Instance.inventoryItemList.Add(storeItems[itemNum - 1]);// 고른아이템 인벤토리에 넣기
                    }
                }
            }
        }
        public void SellItem()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("[상점 - 아이템 판매]");
                Console.WriteLine("필요한 아이템을 팔 수 있는 상점입니다.");
                Console.WriteLine();
                int money = Character.Instance.moneyAmount;
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{money} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < Inventory.Instance.inventoryItemList.Count; i++)
                {
                    double Price = double.Parse(Inventory.Instance.inventoryItemList[i].itemPrice) * 0.85;

                    if (Inventory.Instance.inventoryItemList[i].isEquip)
                    {
                        Console.WriteLine($"- {i + 1} [E]{Inventory.Instance.inventoryItemList[i].itemName}  | {Inventory.Instance.inventoryItemList[i].getItemStat()} +{Inventory.Instance.inventoryItemList[i].itemAbility} | {Inventory.Instance.inventoryItemList[i].itemInfo} | {(int)Price}");

                    }
                    else
                    {
                        Console.WriteLine($"- {i + 1} {Inventory.Instance.inventoryItemList[i].itemName}  | {Inventory.Instance.inventoryItemList[i].getItemStat()} +{Inventory.Instance.inventoryItemList[i].itemAbility} | {Inventory.Instance.inventoryItemList[i].itemInfo} | {(int)Price}");
                    }
                }
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                string input = Console.ReadLine();
                Console.WriteLine();


                if (input == "0")
                    break; //상점으로 이동
                else
                {
                    if (input == null || int.Parse(input) > Inventory.Instance.inventoryItemList.Count) //상점보유수보다 많은 숫자를 선택시
                    {
                        Console.WriteLine("!!!잘못된 입력입니다 아이템 번호를 입력해주세요.!!!");
                        Console.WriteLine();
                        Thread.Sleep(500);
                        continue;
                    }

                    int itemNum = int.Parse(input);
                    double ItemPrice = double.Parse(Inventory.Instance.inventoryItemList[itemNum - 1].itemPrice) * 0.85;

                    if (Inventory.Instance.inventoryItemList[itemNum - 1].isSell) //팔려있다면 소유중
                    {
                        Inventory.Instance.inventoryItemList[itemNum - 1].isEquip = false;
                        Character.Instance.moneyAmount += (int)ItemPrice;
                        Inventory.Instance.DisEquip(itemNum);
                        string sellingItemName = Inventory.Instance.inventoryItemList[itemNum - 1].itemName;
                        int storeSellingItemNum = storeItems.FindIndex(item => item.itemName.Equals(sellingItemName));
                        storeItems[storeSellingItemNum].isSell = false;
                        Inventory.Instance.inventoryItemList.RemoveAt(itemNum - 1);

                        Console.WriteLine("아이템을 판매합니다");
                        Thread.Sleep(500);
                        continue;
                    }
                }
            }
        }
    }
}
