namespace RecipeApp.Maui.Features.Shared.Converters;

public class CustomCompareConverter : CompareConverter<object>
{
    /// <summary>
    /// Converts an object that implements IComparable to a specified object or a boolean based on a comparaison result.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="culture">The culture to use in the converter.  This is not implemented.</param>
    /// <returns>The object assigned to <see cref="TrueObject"/> if (value <see cref="ComparisonOperator"/> <see cref="ComparingValue"/>) equals True and <see cref="TrueObject"/> is not null, if <see cref="TrueObject"/> is null it returns true, otherwise the value assigned to <see cref="FalseObject"/>, if no value is assigned then it returns false.</returns>
    [MemberNotNull(nameof(ComparingValue))]
    public override object ConvertFrom(IComparable value, CultureInfo? culture = null)
    {
        //ArgumentNullException.ThrowIfNull(value);
        if (value is null)
            return FalseObject;

        ArgumentNullException.ThrowIfNull(ComparingValue);
        ArgumentNullException.ThrowIfNull(ComparisonOperator);

        if (!Enum.IsDefined(typeof(OperatorType), ComparisonOperator))
        {
            throw new ArgumentOutOfRangeException($"is expected to be of type {nameof(OperatorType)}", nameof(ComparisonOperator));
        }

        if (!(TrueObject is null ^ FalseObject is not null))
        {
            throw new InvalidOperationException($"{nameof(TrueObject)} and {nameof(FalseObject)} should either be both defined both or both omitted.");
        }

        var result = value.CompareTo(ComparingValue);
        var shouldReturnObjectResult = TrueObject is not null && FalseObject is not null;

        return ComparisonOperator switch
        {
            OperatorType.Smaller => EvaluateCondition(result < 0, shouldReturnObjectResult),
            OperatorType.SmallerOrEqual => EvaluateCondition(result <= 0, shouldReturnObjectResult),
            OperatorType.Equal => EvaluateCondition(result is 0, shouldReturnObjectResult),
            OperatorType.NotEqual => EvaluateCondition(result is not 0, shouldReturnObjectResult),
            OperatorType.GreaterOrEqual => EvaluateCondition(result >= 0, shouldReturnObjectResult),
            OperatorType.Greater => EvaluateCondition(result > 0, shouldReturnObjectResult),
            _ => throw new NotSupportedException($"\"{ComparisonOperator}\" is not supported."),
        };
    }

    object EvaluateCondition(bool comparisonResult, bool shouldReturnObject) => (comparisonResult, shouldReturnObject) switch
    {
        (true, true) => TrueObject ?? throw new InvalidOperationException($"{nameof(TrueObject)} cannot be null"),
        (false, true) => FalseObject ?? throw new InvalidOperationException($"{nameof(FalseObject)} cannot be null"),
        (true, _) => true,
        _ => false
    };
}
