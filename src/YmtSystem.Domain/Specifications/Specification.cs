namespace YmtSystem.Domain.Specifications
{

    using System;
    using System.Linq.Expressions;
    /// <summary>
    /// Represents the base class for specifications.
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        #region Public Methods
        /// <summary>
        /// Evaluates a LINQ expression to its corresponding specification.
        /// </summary>
        /// <param name="expression">The LINQ expression to be evaluated.</param>
        /// <returns>The specification which represents the same semantics as the given LINQ expression.</returns>
        public static Specification<T> Parse(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpecification<T>(expression);
        }
        public static Specification<T> CreateAnySpecification()
        {
            return new AnySpecification<T>();
        }
        #endregion

        #region ISpecification<T> Members
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value which indicates whether the specification
        /// is satisfied by the given object.
        /// </summary>
        /// <param name="obj">The object to which the specification is applied.</param>
        /// <returns>True if the specification is satisfied, otherwise false.</returns>
        public virtual bool IsSatisfiedBy(T obj)
        {
            return this.SatisfiedBy().Compile()(obj);
        }
        /// <summary>
        /// Combines the current specification instance with another specification instance
        /// and returns the combined specification which represents that both the current and
        /// the given specification must be satisfied by the given object.
        /// </summary>
        /// <param name="other">The specification instance with which the current specification
        /// is combined.</param>
        /// <returns>The combined specification instance.</returns>
        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }
        /// <summary>
        /// Combines the current specification instance with another specification instance
        /// and returns the combined specification which represents that either the current or
        /// the given specification should be satisfied by the given object.
        /// </summary>
        /// <param name="other">The specification instance with which the current specification
        /// is combined.</param>
        /// <returns>The combined specification instance.</returns>
        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }
        /// <summary>
        /// Combines the current specification instance with another specification instance
        /// and returns the combined specification which represents that the current specification
        /// should be satisfied by the given object but the specified specification should not.
        /// </summary>
        /// <param name="other">The specification instance with which the current specification
        /// is combined.</param>
        /// <returns>The combined specification instance.</returns>
        public ISpecification<T> AndNot(ISpecification<T> other)
        {
            return new AndNotSpecification<T>(this, other);
        }
        /// <summary>
        /// Reverses the current specification instance and returns a specification which represents
        /// the semantics opposite to the current specification.
        /// </summary>
        /// <returns>The reversed specification instance.</returns>
        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
        public ISpecification<T> AndAlso(ISpecification<T> other)
        {
            return new AndAlsoSpecification<T>(this, other);
        }

        public ISpecification<T> OrElse(ISpecification<T> other)
        {
            return new OrElseSpecification<T>(this, other);
        }

        public ISpecification<T> NotEqual(ISpecification<T> other)
        {
            return new NotEqualSpecification<T>(this, other);
        }
        /// <summary>
        /// Gets the LINQ expression which represents the current specification.
        /// </summary>
        /// <returns>The LINQ expression.</returns>
        public abstract Expression<Func<T, bool>> SatisfiedBy();
        #endregion



    }
}
