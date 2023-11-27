namespace Tbd.Shared.OrderBy;

public class OrderByClauseList<TDbDto> : List<OrderByClauseItemModel<TDbDto>> where TDbDto : class
{
    public new OrderByClauseList<TDbDto> Clear()
    {
        base.Clear();

        return this;
    }

    /// <summary>
    /// Add OrderByClauseItemModel (Ascending direction) to list via strong typed expression
    /// </summary>
    /// <typeparam name="TProperty">Desired dto public property</typeparam>
    /// <param name="expression">Desired MethodExpression</param>
    /// <returns>self</returns>
    public OrderByClauseList<TDbDto> AddOrderByAscending<TProperty>(Expression<Func<TDbDto, TProperty>> expression)
    {
        Add(new OrderByClauseItemModel<TDbDto>(GetPropertyNameFromExpression(expression), OrderByDirectionEnumeration.Ascending));

        return this;
    }

    /// <summary>
    /// Add OrderByClauseItemModel (Ascending direction) to list via string (nameof(dto.Property))
    /// </summary>
    /// <param name="propertyName">Desired dto public property</param>
    /// <returns>self</returns>
    public OrderByClauseList<TDbDto> AddOrderByAscending(string propertyName)
    {
        Add(new OrderByClauseItemModel<TDbDto>(propertyName, OrderByDirectionEnumeration.Ascending));

        return this;
    }

    /// <summary>
    /// Add OrderByClauseItemModel (Descending direction) to list via strong typed expression
    /// </summary>
    /// <typeparam name="TProperty">Desired dto public property</typeparam>
    /// <param name="expression">Desired MethodExpression</param>
    /// <returns>self</returns>
    public OrderByClauseList<TDbDto> AddOrderByDescending<TProperty>(Expression<Func<TDbDto, TProperty>> expression)
    {
        Add(new OrderByClauseItemModel<TDbDto>(GetPropertyNameFromExpression(expression), OrderByDirectionEnumeration.Descending));

        return this;
    }

    /// <summary>
    /// Add OrderByClauseItemModel (Ascending direction) to list via string (nameof(dto.Property))
    /// </summary>
    /// <param name="propertyName">Desired dto public property</param>
    /// <param name="position">Concatenation order</param>
    /// <returns>self</returns>
    public OrderByClauseList<TDbDto> AddOrderByDescending(string propertyName)
    {
        Add(new OrderByClauseItemModel<TDbDto>(propertyName, OrderByDirectionEnumeration.Descending));

        return this;
    }

    public OrderByClauseList<TDbDto> Add<TProperty>(Expression<Func<TDbDto, TProperty>> expression, OrderByDirectionEnumeration orderByDirectionEnumeration)
    {
        Add(new OrderByClauseItemModel<TDbDto>(GetPropertyNameFromExpression(expression), orderByDirectionEnumeration));

        return this;
    }

    public OrderByClauseList<TDbDto> Add(string propertyName, OrderByDirectionEnumeration orderByDirectionEnumeration)
    {
        Add(new OrderByClauseItemModel<TDbDto>(propertyName, orderByDirectionEnumeration));

        return this;
    }

    /// <summary>
    /// Validates each item in Item's is a public property of the desired generic class
    /// </summary>
    /// <returns>Tuple of IsValid and IEnumerable of string (ErrorMessages)</returns>
    public (bool IsValid, IEnumerable<string> ErrorMessages) IsValid()
    {
        var errorMessages = new List<string>();

        if (this.Any().Equals(false))
        {
            errorMessages.Add("At least one 'Item' is required!");
            return (false, errorMessages);
        }

        var type = typeof(TDbDto);
        var memberInfos = type.GetMembers();
        foreach (var item in this)
        {
            var member = Array.Find(memberInfos, m => Equals(item.PropertyName, m.Name)
                                                 && Equals(m.MemberType, MemberTypes.Property));
            if (member is null)
                errorMessages.Add($"{item.PropertyName} is not defined as a public property in: {type.FullName}!");
        }

        return (errorMessages.Any().Equals(false), errorMessages);
    }

    /// <summary>
    /// Renders a concatenated csv list of Items ordered by position
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        string.Join(", ", this);

    public string ToSqlString() =>
        $"ORDER BY {ToString()}";

    /// <summary>
    /// Returns the desired property name of the MemberExpression
    /// Only MemberExpression is accepted - NotSupportedException is thrown for all other expressions
    /// </summary>
    /// <typeparam name="TProperty">The desired property</typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    protected virtual string GetPropertyNameFromExpression<TProperty>(Expression<Func<TDbDto, TProperty>> expression)
    {
        return expression.Body switch
        {
            MemberExpression memberExpression => memberExpression.Member.Name,
            _ => throw new NotSupportedException($"Only public properties are allowed.  This expression is not allowed:  {expression.GetType()}")
        };
    }
}
