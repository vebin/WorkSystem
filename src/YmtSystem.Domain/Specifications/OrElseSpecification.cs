namespace YmtSystem.Domain.Specifications
{
    using System;   
    using System.Linq.Expressions;
   
    public class OrElseSpecification<T> : CompositeSpecification<T>
    {
        public OrElseSpecification(ISpecification<T> left, ISpecification<T> right)
            : base(left, right)
        {
 
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return Left.SatisfiedBy().OrElse(Right.SatisfiedBy());
        }
    }
}
