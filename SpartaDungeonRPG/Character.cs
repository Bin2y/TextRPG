using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonRPG
{
    public class Character
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
            Console.Clear();
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
            {
                Console.WriteLine("다시 입력해주세요!");
                Thread.Sleep(500);
            }

        }
    }
}
