using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonRPG
{
    public class GameManager
    {
        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }


        public void CheckGameOver()
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

        public void Initialize()
        {
            GenerateItem(); //아이템 리스트생성
        }

        public void Login()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\r\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\r\n");
        }

        static void Rest()
        {
            while (true)
            {
                Console.Clear();
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
                        if(Character.Instance.moneyAmount<500)
                        {
                            Console.WriteLine("돈이 없습니다!");
                            Thread.Sleep(500);
                            break;
                        }
                        Character.Instance.moneyAmount -= 500;
                        Character.Instance.health_ab += 10; break;
                    default:
                        Console.WriteLine("잘못 입력하셨습니다 다시 입력해주세요.");
                        Thread.Sleep(500);
                        break;
                }
            }
        }

        public void SelectActivity()
        {
            while (true)
            {
                Console.Clear();
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
                        Thread.Sleep(500);
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
            Thread.Sleep(500);
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
            Thread.Sleep(500);

        }
    }
}
