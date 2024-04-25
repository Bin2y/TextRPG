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
        
        static void Main(string[] args)
        {
            GameManager.Instance.Initialize();
            GameManager.Instance.Login();
            GameManager.Instance.SelectActivity();
        }
    }
}
