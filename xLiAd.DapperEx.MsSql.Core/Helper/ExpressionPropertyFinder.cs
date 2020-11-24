using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Helper
{
    /// <summary>
    /// 找到一个表达式里 某一类型的属性或成员的 所有访问
    /// </summary>
    public class ExpressionPropertyFinder : ExpressionVisitor
    {
        private List<System.Reflection.MemberInfo> Lsrmi = new List<System.Reflection.MemberInfo>();
        private List<Type> Types;
        public IEnumerable<System.Reflection.MemberInfo> MemberList => Lsrmi;
        public ExpressionPropertyFinder(LambdaExpression @Lambda, Type type)
        {
            Types = new List<Type>();
            while(type != typeof(object) && type != null)
            {
                Types.Add(type);
                type = type.BaseType;
            }
            Visit(@Lambda);
        }
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Visit(node.Body);
        }
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expression e = node.Left;
            Visit(e);
            e = node.Right;
            Visit(e);
            return node;
        }
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var obj = node.Object;
            Visit(obj);
            foreach (var aa in node.Arguments)
            {
                Visit(aa);
            }
            return node;
        }
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var objs = node.Bindings;
            foreach (var obj in objs)
            {
                if (obj is MemberAssignment)
                    VisitMemberAssignment((MemberAssignment)obj);
            }
            return node;
        }
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            Visit(node.Expression);
            return node;
        }
        protected override Expression VisitNew(NewExpression node)
        {
            foreach(var obj in node.Arguments)
            {
                Visit(obj);
            }
            return node;
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            if (Types.Contains(node.Member.DeclaringType))
                Lsrmi.Add(node.Member);
            return node;
        }
    }
}
