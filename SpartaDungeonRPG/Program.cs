using System.Data;
using System.Net.Http.Headers;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using System;

namespace SpartaDungeonRPG
{
    internal class Program
    {
        class Character
        {
            private static Character instance;
            public int lv { get; set; }
            public string chad { get; set; }
            public string name { get; set; }

            public float attack_ab { get; set; }
            public int add_attack_ab;

            public int defense_ab { get; set; }
            public int add_defense_ab;
            public int health_ab { get; set; }
            public int add_health_ab;

            public bool isDead { get; set; }

            //능력치를 업데이트해주는 코드도 작성해야함
            //예를들어 5 +(15)이면 토탈 방어력은 20이어야함

            public int moneyAmount { get; set; }

            public static Character Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Character();
                    }
                    return instance;
                }
            }
            public Character()
            {
                lv = 1;
                chad = "전사";
                name = "yoon";
                attack_ab = 10;
                add_attack_ab = 0;
                defense_ab = 0;
                add_defense_ab = 0;
                health_ab = 100;
                add_health_ab = 0;
                moneyAmount = 10000;
                isDead = false;
            }
            public void SaveToFile(string filePath)
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(lv);
                    writer.WriteLine(chad);
                    writer.WriteLine(name);
                    writer.WriteLine(attack_ab);
                    writer.WriteLine(defense_ab);
                    writer.WriteLine(health_ab);
                    writer.WriteLine(moneyAmount);
                    writer.WriteLine(isDead);
                }
                //Console.WriteLine("Character 데이터가 저장되었습니다.");
                //Console.WriteLine();
            }

            public void LoadToCharacter(string filePath)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    lv = int.Parse(reader.ReadLine());
                    chad = reader.ReadLine();
                    name = reader.ReadLine();
                    attack_ab = float.Parse(reader.ReadLine());
                    defense_ab = int.Parse(reader.ReadLine());
                    health_ab = int.Parse(reader.ReadLine());
                    moneyAmount = int.Parse(reader.ReadLine());
                    isDead = bool.Parse(reader.ReadLine());
                }
                //Console.WriteLine("Character 데이터를 불러왔습니다.");
                //Console.WriteLine();
            }

            public void LevelUp()
            {
                Console.WriteLine("레벨업! 공격력이 0.5, 방어력이 1상승했습니다!.");
                Console.WriteLine();
                lv += 1;
                attack_ab += 0.5f;
                defense_ab += 1;
            }
            public void ShowCharacterInfo()
            {
                Console.WriteLine("[상태 보기]");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine();
                Console.WriteLine($"Lv. {lv}");
                Console.WriteLine($"chad ( {chad} )");
                if (add_attack_ab != 0)
                    Console.WriteLine($"공격력 : {attack_ab} (+{add_attack_ab})");
                else
                    Console.WriteLine($"공격력 : {attack_ab}");
                if (add_defense_ab != 0)
                    Console.WriteLine($"방어력 : {defense_ab} (+{add_defense_ab})");
                else
                    Console.WriteLine($"방어력 : {defense_ab}");
                Console.WriteLine($"체 력 : {health_ab}");
                Console.WriteLine($"Gold : {moneyAmount} G");
                if (isDead == true)
                    Console.WriteLine($"상태 : 사망");
                else
                    Console.WriteLine($"상태 : 정상");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                string input = Console.ReadLine();

                if (input != null)
                {
                    if (input == "0")
                    {
                        return;
                    }
                }
                else
                    Console.WriteLine("다시 입력해주세요!");

            }
        }

        class Inventory
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
                        //string itemName, int itemAbility, string itemInfo, string itemPrice, int type
                    }


                }
                //Console.WriteLine("Inventory 데이터를 불러왔습니다.");
                //Console.WriteLine();
            }
            public void ShowInventoryInfo()
            {
                while (true)
                {
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

        class Store
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
                            continue;
                        }

                        int itemNum = int.Parse(input);
                        int ItemPrice = int.Parse(storeItems[itemNum - 1].itemPrice);
                        if (storeItems[itemNum - 1].isSell) //이미 팔렸다면
                        {
                            Console.WriteLine("이미 팔린 물품입니다.");
                            continue;
                        }
                        if (money >= ItemPrice)
                        {
                            //money -= ItemPrice;
                            Character.Instance.moneyAmount -= ItemPrice;
                            //Console.WriteLine($"{Character.Instance.moneyAmount}, {ItemPrice}");
                            storeItems[itemNum - 1].isSell = true;
                            Console.WriteLine($"[{storeItems[itemNum - 1].itemName}] 구매완료!");
                            Console.WriteLine();
                            Inventory.Instance.inventoryItemList.Add(storeItems[itemNum - 1]);// 고른아이템 인벤토리에 넣기
                        }
                    }
                }
            }
            public void SellItem()
            {
                while (true)
                {
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
                            continue;
                        }
                    }
                }
            }
        }
        class Dungeon
        {
            private static Dungeon instance;

            public static Dungeon Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Dungeon();
                    }
                    return instance;
                }
            }
            public void showDungeonInfo()
            {
                while (true)
                {
                    if (Character.Instance.isDead)
                        return;

                    Console.WriteLine("[던전입장]");
                    Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                    Console.WriteLine();

                    Console.WriteLine("1. 쉬운 던전 | 방어력 5 이상 권장");
                    Console.WriteLine("2. 일반 던전 | 방어력 11 이상 권장");
                    Console.WriteLine("3. 어려운 던전 | 방어력 17 이상 권장");
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    string input = Console.ReadLine();
                    if (input == null)
                    {
                        Console.WriteLine("입력이 안되었습니다. 다시 입력해주세요");
                        continue;
                    }







                    switch (input)
                    {
                        case "0": return;
                        case "1": EnterDungeon(int.Parse(input)); break;
                        case "2": EnterDungeon(int.Parse(input)); break;
                        case "3": EnterDungeon(int.Parse(input)); break;
                        default:
                            Console.WriteLine("던전선택이 잘못되었습니다!");
                            Console.WriteLine();
                            break;
                    }


                }
            }

            public void EnterDungeon(int diff)
            {

                int currentDefense_ab = Character.Instance.defense_ab + Character.Instance.add_defense_ab;
                int recomendedDefense = 0;

                switch (diff)
                {
                    case 1: recomendedDefense = 5; break;
                    case 2: recomendedDefense = 11; break;
                    case 3: recomendedDefense = 17; break;
                }

                if (currentDefense_ab < recomendedDefense)
                {
                    Random random = new Random();
                    if (random.Next(1, 101) <= 40)
                    {
                        FailDungeon();
                    }
                    else
                    {
                        ClearDungeon(diff, currentDefense_ab - recomendedDefense);
                    }

                }
                else
                {
                    ClearDungeon(diff, currentDefense_ab - recomendedDefense);
                }

            }

            public void FailDungeon()
            {
                Character.Instance.health_ab /= 2;
                Console.WriteLine("[던전 공략에 실패해 체력에 절반 깎였습니다.]");
                return;
            }
            public void ClearDungeon(int diff, int scale) //scale에 따라서 랜덤값 조정
            {
                while (true)
                {
                    Random random = new Random();
                    //클리어전 상태
                    int bf_health = Character.Instance.health_ab;
                    int bf_money = Character.Instance.moneyAmount;
                    string diffTxt = "";
                    //scale이 음수면 체력이 더 깎이고, 양수면 덜깎임
                    Character.Instance.health_ab -= random.Next(20 - scale, 36 - scale);
                    CheckGameOver();
                    if (Character.Instance.isDead)
                        return;
                    Character.Instance.LevelUp();
                    //공격력에 따른 보상 분배
                    float characterAttack_ab = Character.Instance.attack_ab;
                    double bonus = random.Next((int)characterAttack_ab, (int)characterAttack_ab * 2) * 0.01;
                    Console.WriteLine(bonus.ToString());
                    switch (diff)
                    {
                        case 1:
                            Character.Instance.moneyAmount += (int)(1000 + bonus * 1000);
                            diffTxt = "쉬운";
                            break;
                        case 2:
                            Character.Instance.moneyAmount += (int)(1700 + bonus * 1700);
                            diffTxt = "일반";
                            break;
                        case 3:
                            Character.Instance.moneyAmount += (int)(2500 + bonus * 2500);
                            diffTxt = "어려운";
                            break;
                    }

                    string input;
                    do
                    {
                        Console.WriteLine("[던전 클리어]");
                        Console.WriteLine("축하합니다!!");
                        Console.WriteLine($"{diffTxt} 던전을 클리어 하였습니다.");
                        Console.WriteLine();
                        Console.WriteLine("[탐혐 결과]");
                        Console.WriteLine($"체력 {bf_health} -> {Character.Instance.health_ab}");
                        Console.WriteLine($"Gold {bf_money} G -> {Character.Instance.moneyAmount} G");
                        Console.WriteLine();
                        Console.WriteLine("0.나가기");
                        Console.WriteLine();
                        Console.WriteLine("원하시는 행동을 입력해주세요");
                        input = Console.ReadLine();
                        Console.WriteLine();
                        if (input == "0")
                            return;
                        else
                        {
                            Console.WriteLine("잘못 입력하셨습니다. 다시 입력해주세요");
                        }
                    } while (input != null);

                }
            }
        }
        class Item
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



        static void GenerateItem()
        {
            string[,] itemsInfo =
            {
                { "수련자 갑옷", "5", "수련에 도움을 주는 갑옷입니다.", "1000", "2","false","false"},
                {"무쇠갑옷", "9", "무쇠로 만들어져 튼튼한 갑옷입니다.", "2000", "2","false","false" },
                { "스파르타의 갑옷", "15", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", "3500", "2","false","false" },
                { "황금 갈기 애마", "20", "바다의 신 포세이돈이 타고다녔던 애마", "10000","2","false","false" },
                { "낡은 검", "2", "쉽게 볼 수 있는 낡은 검 입니다", "600", "1" ,"false","false"},
                { "청동 도끼", "5", "어디선가 사용됐던거 같은 도끼입니다.", "1500", "1" ,"false","false"},
                { "스파르타의 창", "7", "스파르타의 전사들이 사용했다는 전설의 창입니다.", "3000", "1","false","false" },
                { "트리아이나", "15", "바다의 신 포세이돈이 사용했던 삼지창", "10000","1","false","false" }
        };

            for (int i = 0; i < itemsInfo.GetLength(0); i++)
            {
                Item item = new Item(
                    itemsInfo[i, 0],
                    int.Parse(itemsInfo[i, 1]),
                    itemsInfo[i, 2],
                    itemsInfo[i, 3],
                    int.Parse(itemsInfo[i, 4]),
                    bool.Parse(itemsInfo[i, 5]),
                    bool.Parse(itemsInfo[i, 6])

                );
                Store.Instance.storeItems.Add(item);
            }
        }
        static void CheckGameOver()
        {
            if (Character.Instance.health_ab <= 0) //죽는다
            {
                Character.Instance.isDead = true;
                Character.Instance.moneyAmount /= 2;
                Character.Instance.health_ab = 0;
                Console.WriteLine("[캐릭터가 사망하였습니다. 소지금이 절반으로 깎이고 로비로 돌아갑니다.]");
                Console.WriteLine();
                return;
            }
        }
        static void Initialize()
        {
            GenerateItem(); //아이템 리스트생성
        }

        static void Login()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\r\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\r\n");
        }

        static void Rest()
        {
            while (true)
            {
                Console.WriteLine("휴식하기");
                Console.WriteLine($"500 G 를 내면 체력을 10 회복할 수 있습니다. (보유 골드 : {Character.Instance.moneyAmount} G)");
                Console.WriteLine();
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "0": return;
                    case "1":
                        Character.Instance.isDead = false; //회복하면 항상 살아있는상태
                        Character.Instance.moneyAmount -= 500;
                        Character.Instance.health_ab += 10; break;
                    default:
                        Console.WriteLine("잘못 입력하셨습니다 다시 입력해주세요.");
                        break;
                }
            }
        }

        static void SelectActivity()
        {
            while (true)
            {
                if (Character.Instance.isDead)
                {
                    Console.WriteLine("사망 상태입니다. 던전입장을 할 수 없습니다.");
                    Console.WriteLine();
                }
                Console.WriteLine("[메인로비]");
                Console.WriteLine();
                Console.WriteLine("0. 게임종료 (저장하고 종료하세요!)");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전입장");
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine("6. 데이터 저장");
                Console.WriteLine("7. 데이터 불러오기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                string input = Console.ReadLine();
                Console.WriteLine();

                string characterFilePath = "characterData.txt";
                string inventoryFilePath = "inventoryData.txt";
                string storeFilePath = "storeData.txt";
                switch (input)
                {
                    case "0":
                        return;
                    case "1":
                        Character.Instance.ShowCharacterInfo(); break;
                    case "2":
                        Inventory.Instance.ShowInventoryInfo(); break;
                    case "3":
                        Store.Instance.ShowStoreInfo(); break;
                    case "4":
                        Dungeon.Instance.showDungeonInfo(); break;
                    case "5":
                        Rest(); break;
                    case "6":
                        SaveAll();
                        break;
                    case "7":
                        LoadAll();
                        break;
                    default:
                        Console.WriteLine("잘못 입력하셨습니다 다시 입력해주세요.");
                        break;
                }
            }
        }

        static void SaveAll()
        {
            string characterFilePath = "characterData.txt";
            string inventoryFilePath = "inventoryData.txt";
            string storeFilePath = "storeData.txt";

            Character.Instance.SaveToFile(characterFilePath);
            Inventory.Instance.SaveToFile(inventoryFilePath);
            Store.Instance.SaveToFile(storeFilePath);

            Console.WriteLine("[데이터가 저장되었습니다.]");
        }

        static void LoadAll()
        {
            string characterFilePath = "characterData.txt";
            string inventoryFilePath = "inventoryData.txt";
            string storeFilePath = "storeData.txt";

            Character.Instance.LoadToCharacter(characterFilePath);
            Inventory.Instance.LoadToCharacter(inventoryFilePath);
            Store.Instance.LoadToCharacter(storeFilePath);

            Console.WriteLine("[데이터 불러오기를 하였습니다.]");

        }
        static void Main(string[] args)
        {
            int num;

            Inventory inventory = new Inventory();
            //Character character = new Character(inventory);
            Store store = new Store();


            Initialize();
            Login();
            SelectActivity();
        }
    }
}
