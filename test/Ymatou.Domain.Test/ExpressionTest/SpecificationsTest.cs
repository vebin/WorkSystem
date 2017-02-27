using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq.Expressions;


namespace YmtSystem.Domain.Test
{
    [TestFixture]
    public class SpecificationsTest
    {
        [Test]
        public void Test()
        {
            Expression<Func<Car, bool>> theCarIsRed = c1 => c1.Color == "Red" && c1.Price < 9.0;
            Expression<Func<Car, bool>> theCarIsCheap = c2 => c2.Price < 10.0;
            ParameterExpression p = theCarIsRed.Parameters.Single();
            Expression<Func<Car, bool>> theCarIsRedOrCheap = Expression.Lambda<Func<Car, bool>>(
                Expression.Or(theCarIsRed.Body, Expression.Invoke(theCarIsCheap, p)), p);
            var carQuery = new List<Car> 
            {
                {new Car{Color="Read",Price=9}},
                 {new Car{Color="Read",Price=8}}
            };

            var query = carQuery.Where(theCarIsRedOrCheap.Compile());

            //query = query.ExpandInvocations();
            //////////////////////////////////////////////////////////
            var arr = new string[] { "a", "b" };
            var r = from a in carQuery
                    where arr.Contains(a.Color)
                    select a;

            // Expression.Lambda<Func<Car,double[]>>(
        }
    }
}
