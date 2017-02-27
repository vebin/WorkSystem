namespace YmtSystem.Domain.Specifications
{
    using System;    
    using System.Linq.Expressions;    

    public class AndAlsoSpecification<T> : CompositeSpecification<T>
    {
        public AndAlsoSpecification(ISpecification<T> left, ISpecification<T> right)
            : base(left, right)
        {

        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return Left.SatisfiedBy().AndAlso(Right.SatisfiedBy());
        }
    }
}
