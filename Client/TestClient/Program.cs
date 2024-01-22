using System;
using TestClient.TestClient;
using GameBase.Template.GameBase.Table;
using Service.Core;
using System.Data;

namespace TestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(System.IO.Directory.GetCurrentDirectory().ToString());
            //DataTable<int, TestToolTable>.Instance.Init("../../../TestTable/TestTool.csv");
            //DataTable<int, TestToolTable>.Instance.GetData(1);
            TestApp.Instance.Init();
            TestApp.Instance.Update();
        }
    }
}
