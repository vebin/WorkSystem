namespace YmtSystem.Domain.Specifications
{
    using System;    
    using System.Linq.Expressions;
   
    public class NotEqualSpecification<T> : CompositeSpecification<T>
    {
        public NotEqualSpecification(ISpecification<T> left, ISpecification<T> right)
            : base(left, right)
        {
 
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return Left.SatisfiedBy().NotEqual(Right.SatisfiedBy());
        }
    }
}
