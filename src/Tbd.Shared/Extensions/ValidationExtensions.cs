namespace Tbd.Shared.Extensions;

public static class ValidationExtensions
{
    public static ApiResultMessageModel ToApiResultMessageModel(this ValidationResult validationResult)
    {
        return new ApiResultMessageModel
        {
            Code = (int)HttpStatusCode.BadRequest,
            Message = validationResult.ErrorMessage,
            MessageType = ApiResultMessageModelTypeEnumeration.Error,
            Source = string.Join(", ", validationResult.MemberNames)
        };
    }

    public static bool TryValidateObject(this object obj, ICollection<IApiResultMessageModel> apiResultMessages, bool nullObjReturnsFalse = true, bool validateAllProperties = true)
    {
        var messages = new List<ApiResultMessageModel>();
        var result = obj.TryValidateObject(messages, nullObjReturnsFalse, validateAllProperties);
        apiResultMessages.AddRange(messages.Cast<IApiResultMessageModel>());

        return result;
    }

    public static bool TryValidateObject(this object obj, ICollection<ApiResultMessageModel> apiResultMessages, bool nullObjReturnsFalse = true, bool validateAllProperties = true)
    {
        var returnResult = true;
        var validationResultList = new List<ValidationResult>();

        if (obj == null)
            return !nullObjReturnsFalse;

        // Single object
        if (obj is not IEnumerable enumerable)
        {
            returnResult = Validator.TryValidateObject(obj, new ValidationContext(obj), validationResultList, validateAllProperties);
            foreach (var validationResult in validationResultList)
                apiResultMessages.Add(validationResult.ToApiResultMessageModel());

            return returnResult;
        }

        // List of objects is empty
        if (!enumerable.Cast<object>().Any())
            return true;

        // List of objects
        foreach (var item in (System.Collections.IEnumerable)obj)
        {
            validationResultList.Clear();
            returnResult = returnResult && Validator.TryValidateObject(item, new ValidationContext(item), validationResultList, validateAllProperties);

            foreach (var validationResult in validationResultList)
                apiResultMessages.Add(validationResult.ToApiResultMessageModel());
        }

        return returnResult;
    }
}
