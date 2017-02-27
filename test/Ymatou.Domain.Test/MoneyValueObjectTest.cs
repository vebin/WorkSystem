using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.Domain.Shard.NMoneys;
using YmtSystem.Domain.Shard.NMoneys.Exchange;
using YmtSystem.Domain.Shard.NMoneys.Extensions;
namespace YmtSystem.Domain.Test
{
    [TestFixture]
    public class MoneyValueObjectTest
    {
        [Test]
        public void ConvertTo()
        {
            //var customProvider = new CustomArithmeticProvider();
            //ExchangeRateProvider.Factory = () => customProvider;
            var tenEuro = new Money(100m, CurrencyIsoCode.CNY);
            //var tenDollars = tenEuro.Convert().To(CurrencyIsoCode.USD);
            var tenDollars = tenEuro.To(Currency.Usd, () => () => new CustomArithmeticProvider());
            Assert.AreEqual(67.00, tenDollars.Amount);
        }
    }
    //自定义实现货币转换算法实现
    public class CustomArithmeticProvider : IExchangeRateProvider
    {
        public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
        {
            return new CustomRateArithmetic(from, to, .67m);
        }

        public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
        {
            rate = new CustomRateArithmetic(from, to, .67m);
            return true;
        }
    }
    //自定义实现货币汇率（可选，如果没有自定义转换率，可以直接使用基类汇率）
    public class CustomRateArithmetic : ExchangeRate
    {
        public CustomRateArithmetic(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate)
            : base(from, to, rate) { }

        //实现自定义汇率
        //public override Money Apply(Money from)
        //{
        //    // instead of this useless "return 0" policy one can implement rounding policies, for instance
        //    return new Money(0m, To);
        //}
    }
}
