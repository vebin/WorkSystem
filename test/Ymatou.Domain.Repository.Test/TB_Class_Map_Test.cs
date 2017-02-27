using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.CrossCutting;
using Ymt.Trading.Domain.Model.Account;
using Ymt.Trading.Domain.Repository;
using Ymt.Trading.Domain.Model.User;
using YmtSystem.Domain.Shard;
using Ymt.Trading.Domain.Model.Table;
namespace YmtSystem.Domain.Repository.Test
{
    [TestFixture]
    public class TB_Class_Map_Test
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            YmatouFramework.Start();
        }
        [TestFixtureTearDown]
        public void End()
        {
            YmatouFramework.Stop();
        }
        //ok
        [Test]
        [Description("多对多")]
        public void Account_Bank_N_N_Test()
        {
            var bank = new Bank { BankName = "AA" };
            bank.AddAddress(new BankAddress { City = "A", Street = "B", Zip = "C" });
            bank.AddAccount(new UserAccount { AName = "A", UserAccountBankInfo = bank });
            var bankRepository = LocalServiceLocator.GetService<IBankRepository>();
            bankRepository.Add(bank);
            var result = bankRepository.Context.Commit();
            Assert.AreEqual(1, result.ResultData);
        }

        //ok...
        [Test]
        [Description("一个实体映射到多个表")]
        public void One_Calss_Map_N_Table()
        {
            var t2 = new T2(Guid.NewGuid().ToString("N"), 3);
            var vResult = t2.Validator();
            Assert.AreEqual(true, vResult.Success);
            var t2epository = LocalServiceLocator.GetService<IT2Repository>();
            t2epository.Add(t2);
            var result = t2epository.Context.Commit();
            Assert.AreEqual(1, result.ResultData);
        }
        //...
        [Test]
        [Description("多个实体映射到一个表")]
        public void Multiple_Class_Map_1_Table()
        {
            //var t7 = new T7 { Id = Guid.NewGuid().ToString("N"), Name = "T5OK..." };
            //var t5 = new T5(t7.Id);
            //t5.T7Field = t7;
            //t5.A = 1;
            //t5.B = 2;
            //t7.T5Field = t5;
            //var validator = t5.Validator();
            //Assert.AreEqual(true, validator.Success);
            //var t5Repository = LocalServiceLocator.GetService<IT5Repository>();
            //t5Repository.Add(t5);
            //var result = t5Repository.Context.Commit();
            //Assert.AreEqual(1, result.ResultData);
        }
        //ok
        [Test]
        [Description("单表继承：将类的继承层次，映射为一个表（父类，子类映射到一个表 tph")]
        public void Single_Table_inheritance_TPH()
        {
            var buyer = new Buyers(Guid.NewGuid().ToString("N"));
            buyer.Sex = 1;
            buyer.Sex2 = 2;
            buyer.Tel = "123";
            buyer.UName = "zhangsan";

            Assert.AreEqual(true, buyer.Validator().Success);
            var buyerRepository = LocalServiceLocator.GetService<IBuyersRepository>();
            buyerRepository.Add(buyer);
            var result = buyerRepository.Context.Commit();
            Assert.AreEqual(1, result.ResultData);
        }
        //ok
        [Test]
        [Description("类表继承 类表映射：{每个类对应一个表}（子类，父类各在一个表中），表示继承的层次]")]
        public void Class_Table_Inheritance_TPT()
        {
            var t8 = new T8(Guid.NewGuid().ToString("N"));
            t8.T81 = 1;
            t8.T82 = "2";
            Assert.AreEqual(true, t8.Validator().Success);
            var t8Repository = LocalServiceLocator.GetService<IT8Repository>();
            t8Repository.Add(t8);
            var result = t8Repository.Context.Commit();
            Assert.AreEqual(1, result.ResultData);
        }
        //ok
        [Test]
        [Description("具体表继承，基类，子类分别在不同的表，但子类包含了基类的字段")]
        public void Conncrete_Table_Inheritance_TPC()
        {
            var t11 = new T11(Guid.NewGuid().ToString("N"));
            t11.T111 = 12;
            t11.T112 = "ok";
            Assert.AreEqual(true, t11.Validator().Success);
            var t11Repository = LocalServiceLocator.GetService<IT11Repository>();
            t11Repository.Add(t11);
            var result = t11Repository.Context.Commit();
            Assert.AreEqual(1, result.ResultData);
            var t12 = new T12(Guid.NewGuid().ToString("N"));
            t12.T111 = t11.T111;
            t12.T112 = t11.T112;
            t12.T121 = 12;
            Assert.AreEqual(true, t12.Validator().Success);
            var t12Repository = LocalServiceLocator.GetService<IT11Repository>();
            t12Repository.Add(t12);
            result = t12Repository.Context.Commit();
            Assert.AreEqual(1, result.ResultData);
        }
    }
}
