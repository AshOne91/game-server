using System;
using TestClient.TestClient;
using GameBase.Template.GameBase.Table;
using Service.Core;
using System.Data;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TestApp.Instance.Init();
            TestApp.Instance.Update();
        }
    }
}
