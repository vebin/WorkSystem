using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YmtSystem.Domain.Test
{
    public class InvocationExpander : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;
        private readonly Expression _expansion;
        private readonly InvocationExpander _previous;
        public InvocationExpander(ParameterExpression _parameter, Expression _expansion,InvocationExpander _previous)
        {
            this._parameter = _parameter;
            this._expansion = _expansion;
            this._previous = _previous;
        }
        protected override Expression VisitInvocation(InvocationExpression iv)
        {
            if (iv.Expression.NodeType == ExpressionType.Lambda)
            {
                LambdaExpression lambda = (LambdaExpression)iv.Expression;
                return lambda
                    .Parameters
                    // zip together parameters and the corresponding argument values
                    .Zip(iv.Arguments, (p, e) => new { Parameter = p, Expansion = e })
                    // add to the stack of available parameters bindings (this class doubles as an immutable stack)
                    .Aggregate(this, (previous, pair) => new InvocationExpander(pair.Parameter, pair.Expansion, previous))
                    // visit the body of the lambda using an expander including the new parameter bindings
                    .Visit(lambda.Body);
            }
            return base.VisitInvocation(iv);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            InvocationExpander expander = this;
            while (null != expander)
            {
                if (expander._parameter == p)
                {
                    return base.Visit(expander._expansion);
                }
                expander = expander._previous;
            }
            return base.VisitParameter(p);
        }
    }
}
