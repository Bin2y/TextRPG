using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonRPG
{
    public class Dungeon
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
                Console.Clear();
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
                    Thread.Sleep(500);
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
                        Thread.Sleep(500);
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
            Thread.Sleep(500);
            return;
        }
        public void ClearDungeon(int diff, int scale) //scale에 따라서 랜덤값 조정
        {
            while (true)
            {
                Console.Clear();
                Random random = new Random();
                //클리어전 상태
                int bf_health = Character.Instance.health_ab;
                int bf_money = Character.Instance.moneyAmount;
                string diffTxt = "";
                //scale이 음수면 체력이 더 깎이고, 양수면 덜깎임
                Character.Instance.health_ab -= random.Next(20 - scale, 36 - scale);
                GameManager.Instance.CheckGameOver();
                if (Character.Instance.isDead)
                    return;
                Character.Instance.LevelUp();
                //공격력에 따른 보상 분배
                float characterAttack_ab = Character.Instance.attack_ab;
                double bonus = random.Next((int)characterAttack_ab, (int)characterAttack_ab * 2) * 0.01;
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
                        Thread.Sleep(500);
                    }
                } while (input != null);

            }
        }
    }
}
