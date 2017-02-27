namespace YmtSystem.Domain.Shard.NMoneys.Exchange
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ExchangeExtensions
    {
        /// <summary>
        /// 货币转换        
        /// </summary>
        /// <param name="forme"></param>
        /// <param name="toCode"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static Money To(this Money forme, CurrencyIsoCode toCode, Func<Func<IExchangeRateProvider>> rate)
        {
            ExchangeRateProvider.Factory = rate();
            return forme.Convert().To(toCode);
        }

        /// <summary>
        /// 货币转换        
        /// </summary>
        /// <param name="forme"></param>
        /// <param name="toCode"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static Money To(this Money forme, Currency toCode, Func<Func<IExchangeRateProvider>> rate)
        {
            ExchangeRateProvider.Factory = rate();
            return forme.Convert().To(toCode);
        }

        /// <summary>
        /// 货币转换        
        /// </summary>
        /// <param name="forme"></param>
        /// <param name="toCode"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static Money TryTo(this Money forme, CurrencyIsoCode toCode, Func<Func<IExchangeRateProvider>> rate, Money defVal = default(Money))
        {
            ExchangeRateProvider.Factory = rate();
            var cVal = forme.TryConvert().To(toCode);
            return cVal.HasValue ? cVal.Value : defVal;
        }

        /// <summary>
        /// 货币转换        
        /// </summary>
        /// <param name="forme"></param>
        /// <param name="toCode"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static Money TryTo(this Money forme, Currency toCode, Func<Func<IExchangeRateProvider>> rate, Money defVal = default(Money))
        {
            ExchangeRateProvider.Factory = rate();
            var cVal = forme.TryConvert().To(toCode);
            return cVal.HasValue ? cVal.Value : defVal;
        }
    }
}
