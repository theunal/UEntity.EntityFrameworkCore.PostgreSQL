using System.Linq.Expressions;
using UEntity.EntityFrameworkCore.PostgreSQL;

namespace Tests.Tests;

[TestFixture]
public class ExpressionCombinerTests
{
    private static Expression<Func<int, bool>> IsPositive => x => x > 0;
    private static Expression<Func<int, bool>> IsEven => x => x % 2 == 0;
    private static Expression<Func<int, bool>> IsLessThanTen => x => x < 10;

    [Test]
    public void And_combines_two_predicates_with_andalso()
    {
        var combined = IsPositive.And(IsEven);

        var result = combined.Compile();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result(4), Is.True);
            Assert.That(result(2), Is.True);
            Assert.That(result(-2), Is.False);
            Assert.That(result(3), Is.False);
        }
    }

    [Test]
    public void And_works_with_linq_to_objects()
    {
        var combined = IsPositive.And(IsEven);

        var numbers = new[] { -3, -2, -1, 0, 1, 2, 3, 4, 5, 6 };
        var filtered = numbers.Where(combined.Compile()).ToList();

        Assert.That(filtered, Is.EqualTo([2, 4, 6]));
    }

    [Test]
    public void And_throws_on_null_left()
    {
        Assert.That(() => ((Expression<Func<int, bool>>)null!).And(IsEven),
            Throws.ArgumentNullException);
    }

    [Test]
    public void And_throws_on_null_right()
    {
        Assert.That(() => IsPositive.And(null!),
            Throws.ArgumentNullException);
    }

    [Test]
    public void Or_combines_two_predicates_with_orelse()
    {
        var combined = IsPositive.Or(IsEven);

        var result = combined.Compile();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result(4), Is.True);
            Assert.That(result(3), Is.True);
            Assert.That(result(-2), Is.True);
            Assert.That(result(-1), Is.False);
        }
    }

    [Test]
    public void Or_works_with_linq_to_objects()
    {
        var combined = IsPositive.Or(IsEven);

        var numbers = new[] { -3, -2, -1, 0, 1, 2, 3, 4, 5, 6 };
        var filtered = numbers.Where(combined.Compile()).ToList();

        Assert.That(filtered, Is.EqualTo([-2, 0, 1, 2, 3, 4, 5, 6]));
    }

    [Test]
    public void Or_throws_on_null_left()
    {
        Assert.That(() => ((Expression<Func<int, bool>>)null!).Or(IsEven),
            Throws.ArgumentNullException);
    }

    [Test]
    public void Or_throws_on_null_right()
    {
        Assert.That(() => IsPositive.Or(null!),
            Throws.ArgumentNullException);
    }

    [Test]
    public void And_chained_multiple_predicates()
    {
        var combined = IsPositive.And(IsEven).And(IsLessThanTen);

        var numbers = new[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        var filtered = numbers.Where(combined.Compile()).ToList();

        Assert.That(filtered, Is.EqualTo([2, 4, 6, 8]));
    }

    [Test]
    public void Or_chained_multiple_predicates()
    {
        var combined = IsPositive.Or(IsEven).Or(IsLessThanTen);

        var numbers = new[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        var filtered = numbers.Where(combined.Compile()).ToList();

        Assert.That(filtered, Is.EqualTo([-5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]));
    }

    [Test]
    public void And_does_not_contain_invoke_expression()
    {
        var combined = IsPositive.And(IsEven);

        var hasInvoke = new InvokeFinder().HasInvoke(combined);
        Assert.That(hasInvoke, Is.False);
    }

    [Test]
    public void Or_does_not_contain_invoke_expression()
    {
        var combined = IsPositive.Or(IsEven);

        var hasInvoke = new InvokeFinder().HasInvoke(combined);
        Assert.That(hasInvoke, Is.False);
    }

    [Test]
    public void And_both_bodies_share_same_parameter_instance()
    {
        var combined = IsPositive.And(IsEven);

        Assert.That(GetParameterInExpression(combined.Body), Is.SameAs(combined.Parameters[0]));
    }

    [Test]
    public void Or_both_bodies_share_same_parameter_instance()
    {
        var combined = IsPositive.Or(IsEven);

        Assert.That(GetParameterInExpression(combined.Body), Is.SameAs(combined.Parameters[0]));
    }

    [Test]
    public void And_integration_with_selectas_cache_isolation()
    {
        var combined = IsPositive.And(IsEven);

        var compiled = combined.Compile();
        var numbers = Enumerable.Range(-5, 16).Where(compiled).ToList();

        Assert.That(numbers, Is.EquivalentTo([2, 4, 6, 8, 10]));
    }

    private sealed class InvokeFinder : ExpressionVisitor
    {
        private bool _found;

        public bool HasInvoke(Expression expression)
        {
            _found = false;
            Visit(expression);
            return _found;
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            _found = true;
            return base.VisitInvocation(node);
        }
    }

    private static ParameterExpression? GetParameterInExpression(Expression expr)
    {
        if (expr is BinaryExpression binary)
            return GetParameterInExpression(binary.Left) ?? GetParameterInExpression(binary.Right);

        if (expr is UnaryExpression unary)
            return GetParameterInExpression(unary.Operand);

        if (expr is ParameterExpression param)
            return param;

        return null;
    }
}
