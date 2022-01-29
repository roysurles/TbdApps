using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

using Tbd.Shared.Pagination;

namespace Tbd.Shared.ApiResult
{
    /// <summary>
    /// Represents a generic and consistent result model.
    /// </summary>
    /// <typeparam name="T">Type of data.</typeparam>
    public class ApiResultModel<T> : IApiResultModel<T>
    {
        /// <summary>
        /// HttpStatusCode
        /// </summary>
        [JsonPropertyName("httpStatusCode")]
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;

        /// <summary>
        /// IsSuccessHttpStatusCode
        /// </summary>
        public bool IsSuccessHttpStatusCode =>
            (int)HttpStatusCode >= 200 && (int)HttpStatusCode < 300;

        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public T Data { get; set; }

        /// <summary>
        /// Returns 'success' if there are no errors or warnings.
        /// Returns 'warnings' if there are warnings and no errors.
        /// Returns 'errors; if there are errors.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type => HasErrors ? "errors" : HasWarnings ? "warnings" : "success";

        /// <summary>
        /// Pagination meta data.  This will not be serialized if there is no pagination meta data.
        /// </summary>
        [JsonPropertyName("meta")]
        public PaginationMetaDataModel Meta { get; set; }

        /// <summary>
        /// Flag indicating errors exist.
        /// </summary>
        [JsonPropertyName("hasErrors")]
        public bool HasErrors => Messages.Any(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Error));

        /// <summary>
        /// Flag indicating unhandledExceptions exist.
        /// </summary>
        [JsonPropertyName("hasUnhandledExceptions")]
        public bool HasUnhandledExceptions => Messages.Any(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.UnhandledException));

        /// <summary>
        /// Flag indicating warnings exist.
        /// </summary>
        [JsonPropertyName("hasWarnings")]
        public bool HasWarnings => Messages.Any(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Warning));

        /// <summary>
        /// List of messages.
        /// </summary>ummary>
        [JsonPropertyName("messages")]
        public List<ApiResultMessageModel> Messages { get; set; } = new List<ApiResultMessageModel>();

        [JsonIgnore]
        public IEnumerable<ApiResultMessageModel> Errors =>
            Messages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Error));

        /// <summary>
        /// Fluent method to set the HttpStatusCode property.
        /// </summary>
        /// <param name="httpStatusCode">HttpStatusCode</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> SetHttpStatusCode(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
            return this;
        }

        /// <summary>
        /// Fluent method to set the Data property.
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="defaultData">Default data if data parameter is null.</param>
        /// <param name="createNewDataIfNullDefault">Create instance of data if both data parameter is null and defaultData parameter is null.</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> SetData(T data, T defaultData = default, bool createNewDataIfNullDefault = false)
        {
            Data = EqualityComparer<T>.Default.Equals(data, default)
                ? EqualityComparer<T>.Default.Equals(defaultData, default) && createNewDataIfNullDefault
                    ? Activator.CreateInstance<T>()
                    : defaultData
                : data;
            return this;
        }

        /// <summary>
        /// Fluent method to set the Meta property. Used for displaying TotalItemCount.
        /// </summary>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> SetMeta()
        {
            var count = (Data as IEnumerable)?.Cast<object>().Count() ?? 0;
            var page = count == 0 ? 0 : 1;
            Meta = new PaginationMetaDataModel(page, count, count);
            return this;
        }
        /// <summary>
        /// Fluent method to set the Meta property.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="totalItemCount">Total item count.</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> SetMeta(int pageNumber, int pageSize, int totalItemCount)
        {
            Meta = new PaginationMetaDataModel(pageNumber, pageSize, totalItemCount);
            return this;
        }
        /// <summary>
        /// Fluent method to set the Meta property.
        /// </summary>
        /// <param name="paginationRequestModel">IPaginationRequestModel</param>
        /// <param name="totalItemCount">Total item count.</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> SetMeta<TDbDto>(IPaginationRequestModel<TDbDto> paginationRequestModel, int totalItemCount) where TDbDto : class
        {
            Meta = new PaginationMetaDataModel(paginationRequestModel.PageNumber, paginationRequestModel.PageSize, totalItemCount);
            return this;
        }
        /// <summary>
        /// Fluent method to set the Meta property.
        /// </summary>
        /// <param name="paginationMetaDataModel">IPaginationMetaDataModel</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> SetMeta(PaginationMetaDataModel paginationMetaDataModel)
        {
            Meta = paginationMetaDataModel;
            return this;
        }

        /// <summary>
        /// Fluent method to set the next and previous urls of the Meta property.
        /// </summary>
        /// <param name="actionContext">ActionContext</param>
        /// <param name="routeName">Route name.</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        //TODO:  public IApiResultModel<T> SetMetaUrls(ActionContext actionContext, string routeName)
        //{
        //    if (Meta == null)
        //        return this;

        //    Meta.SetUrls(actionContext, routeName);
        //    return this;
        //}

        /// <summary>
        /// Fluent method to add an error.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="code">HttpStatusCode</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> AddMessage(Exception exception, string message = null, string source = null, HttpStatusCode? code = HttpStatusCode.InternalServerError)
        {
            Messages.Add(new ApiResultMessageModel
            {
                MessageType = ApiResultMessageModelTypeEnumeration.UnhandledException,
                Message = message,
                Source = source,
                Code = (int)code,
                //TODO: UnhandledException = exception
            });

            return this;
        }

        /// <summary>
        /// Fluent method to add a message.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="source">Source</param>
        /// <param name="code">Code</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> AddMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, int? code = null)
        {
            Messages.Add(new ApiResultMessageModel { MessageType = apiResultMessageType, Message = message, Source = source, Code = code });
            return this;
        }

        public IApiResultModel<T> AddMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, HttpStatusCode? code = null)
        {
            Messages.Add(new ApiResultMessageModel { MessageType = apiResultMessageType, Message = message, Source = source, Code = (int)code });
            return this;
        }

        public IApiResultModel<T> AddInformationMessage(string message, string source = null, int? code = null)
        {
            return AddMessage(ApiResultMessageModelTypeEnumeration.Information, message, source, code);
        }

        public IApiResultModel<T> AddInformationMessage(string message, string source = null, HttpStatusCode? code = null)
        {
            return AddMessage(ApiResultMessageModelTypeEnumeration.Information, message, source, code);
        }

        public IApiResultModel<T> AddWarningMessage(string message, string source = null, int? code = null)
        {
            return AddMessage(ApiResultMessageModelTypeEnumeration.Warning, message, source, code);
        }

        public IApiResultModel<T> AddWarningMessage(string message, string source = null, HttpStatusCode? code = null)
        {
            return AddMessage(ApiResultMessageModelTypeEnumeration.Warning, message, source, code);
        }

        public IApiResultModel<T> AddErrorMessage(string message, string source = null, int? code = null)
        {
            return AddMessage(ApiResultMessageModelTypeEnumeration.Error, message, source, code);
        }

        public IApiResultModel<T> AddErrorMessage(string message, string source = null, HttpStatusCode? code = null)
        {
            return AddMessage(ApiResultMessageModelTypeEnumeration.Error, message, source, code);
        }

        // Lets wait and see if this is necessary... leaving commented out for now
        //public IApiResultModel<T> AddDefaultUnhandledExceptionError(Exception ex, string source = null)
        //{
        //    // leaving placeholder for 'ex' right now, in case it is utilized down the road
        //    Errors.Add(new ApiResultErrorModel
        //    {
        //        IsException = true,
        //        Code = (int)HttpStatusCode.InternalServerError,
        //        Message = "We're sorry, but we are unable to complete the request.  Please try again later.",
        //        Source = string.IsNullOrWhiteSpace(source) ? ex.Source : source,
        //        Stack = ex.StackTrace,
        //        Type = ex.GetType().Name
        //    });
        //    return this;
        //}

        /// <summary>
        /// Fluent method to add multiple messages.
        /// </summary>
        /// <param name="errors">IEnumerable&lt;IApiResultErrorModel&gt;</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> AddMessages(IEnumerable<ApiResultMessageModel> messages)
        {
            Messages.AddRange(messages);
            return this;
        }

        public IApiResultModel<T> VerifyDataHasCount(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message = "No data found."
            , string source = "Data Count", HttpStatusCode? httpStatusCode = HttpStatusCode.NotFound, bool setHttpStatusCode = true) =>
            VerifyDataHasCount(Data as IEnumerable, apiResultMessageType, message, source, httpStatusCode, setHttpStatusCode);

        public IApiResultModel<T> VerifyDataHasCount(IEnumerable data, ApiResultMessageModelTypeEnumeration apiResultMessageType, string message = "No data found."
            , string source = "Data Count", HttpStatusCode? httpStatusCode = HttpStatusCode.NotFound, bool setHttpStatusCode = true)
        {
            if (data?.Cast<object>().Any() == false)
            {
                AddMessage(apiResultMessageType, message, source, (int?)httpStatusCode);

                if (setHttpStatusCode && httpStatusCode.HasValue)
                    SetHttpStatusCode(httpStatusCode.Value);
            }

            return this;
        }

        /// <summary>
        /// Fluent method to add a message if the Data property is null.
        /// If desired, will also set the HttpStatusCode property if the Data property is null.
        /// </summary>
        /// <param name="message">Message; default is 'No data found.'.</param>
        /// <param name="source">Source; default is 'Data'.</param>
        /// <param name="httpStatusCode">Message HttpStatusCode; default is NotFound.  Will also set the HttpStatusCode property if desired.</param>
        /// <param name="setHttpStatusCode">Set the HttpStatusCode property if desired.</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        public IApiResultModel<T> VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message = "No data found.",
            string source = "Data", HttpStatusCode httpStatusCode = HttpStatusCode.NotFound, bool setHttpStatusCode = true)
        {
            if (EqualityComparer<T>.Default.Equals(Data, default))
            {
                AddMessage(apiResultMessageType, message, source, httpStatusCode);

                if (setHttpStatusCode)
                    SetHttpStatusCode(httpStatusCode);
            }

            return this;
        }

        /// <summary>
        /// Conditional serialization for Errors.
        /// </summary>
        public bool ShouldSerializeErrors() => HasErrors;

        /// <summary>
        /// Conditional serialization for Warnings.
        /// </summary>
        public bool ShouldSerializeWarnings() => HasWarnings;
    }

    /// <summary>
    /// Represents a generic and consistent result model.
    /// </summary>
    /// <typeparam name="T">Type of data.</typeparam>
    public interface IApiResultModel<T>
    {
        /// <summary>
        /// HttpStatusCode
        /// </summary>
        HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// IsSuccessHttpStatusCode
        /// </summary>
        bool IsSuccessHttpStatusCode { get; }

        /// <summary>
        /// Data
        /// </summary>
        T Data { get; set; }

        /// <summary>
        /// Returns 'success' if there are no errors or warnings.
        /// Returns 'warnings' if there are warnings and no errors.
        /// Returns 'errors; if there are errors.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Pagination meta data.  This will not be serialized if there is no pagination meta data.
        /// </summary>
        PaginationMetaDataModel Meta { get; set; }

        /// <summary>
        /// Flag indicating errors exist.
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Flag indicating unhandledExceptions  exist.
        /// </summary>
        bool HasUnhandledExceptions { get; }

        /// <summary>
        /// Flag indicating warnings exist.
        /// </summary>
        bool HasWarnings { get; }

        /// <summary>
        /// List of messages.
        /// </summary>ummary>
        List<ApiResultMessageModel> Messages { get; set; }

        /// <summary>
        /// Fluent method to set the HttpStatusCode property.
        /// </summary>
        /// <param name="httpStatusCode">HttpStatusCode</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> SetHttpStatusCode(HttpStatusCode httpStatusCode);

        /// <summary>
        /// Fluent method to set the Data property.
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="defaultData">Default data if data parameter is null.</param>
        /// <param name="createNewDataIfNullDefault">Create instance of data if both data parameter is null and defaultData parameter is null.</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> SetData(T data, T defaultData = default, bool createNewDataIfNullDefault = false);

        /// <summary>
        /// Fluent method to set the Meta property. Used for displaying TotalItemCount.
        /// </summary>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> SetMeta();
        /// <summary>
        /// Fluent method to set the Meta property.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="totalItemCount">Total item count.</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> SetMeta(int pageNumber, int pageSize, int totalItemCount);
        /// <summary>
        /// Fluent method to set the Meta property.
        /// </summary>
        /// <param name="paginationRequestModel">IPaginationRequestModel</param>
        /// <param name="totalItemCount">Total item count.</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> SetMeta<TDbDto>(IPaginationRequestModel<TDbDto> paginationRequestModel, int totalItemCount) where TDbDto : class;
        /// <summary>
        /// Fluent method to set the Meta property.
        /// </summary>
        /// <param name="paginationMetaDataModel">IPaginationMetaDataModel</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> SetMeta(PaginationMetaDataModel paginationMetaDataModel);

        /// <summary>
        /// Fluent method to set the next and previous urls of the Meta property.
        /// </summary>
        /// <param name="actionContext">ActionContext</param>
        /// <param name="routeName">Route name.</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        // TODO:  IApiResultModel<T> SetMetaUrls(ActionContext actionContext, string routeName);

        /// <summary>
        /// Fluent method to add an error.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="code">HttpStatusCode</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> AddMessage(Exception exception, string message = null, string source = null, HttpStatusCode? code = HttpStatusCode.InternalServerError);

        /// <summary>
        /// Fluent method to add an error.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="source">Source</param>
        /// <param name="code">Code</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> AddMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, int? code = null);

        IApiResultModel<T> AddInformationMessage(string message, string source = null, int? code = null);

        IApiResultModel<T> AddInformationMessage(string message, string source = null, HttpStatusCode? code = null);

        IApiResultModel<T> AddWarningMessage(string message, string source = null, int? code = null);

        IApiResultModel<T> AddWarningMessage(string message, string source = null, HttpStatusCode? code = null);

        IApiResultModel<T> AddErrorMessage(string message, string source = null, int? code = null);

        IApiResultModel<T> AddErrorMessage(string message, string source = null, HttpStatusCode? code = null);

        /// <summary>
        /// Fluent method to add an error.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="source">Source</param>
        /// <param name="code">HttpStatusCode</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> AddMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, HttpStatusCode? code = null);

        // Lets wait and see if this is necessary... leaving commented out for now
        // IApiResultModel<T> AddDefaultUnhandledExceptionError(Exception ex, string source = null);

        /// <summary>
        /// Fluent method to add multiple errors.
        /// </summary>
        /// <param name="errors">IEnumerable&lt;IApiResultErrorModel&gt;</param>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        IApiResultModel<T> AddMessages(IEnumerable<ApiResultMessageModel> messages);

        IApiResultModel<T> VerifyDataHasCount(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message = "No data found."
            , string source = "Data Count", HttpStatusCode? httpStatusCode = HttpStatusCode.NotFound, bool setHttpStatusCode = true);

        IApiResultModel<T> VerifyDataHasCount(IEnumerable data, ApiResultMessageModelTypeEnumeration apiResultMessageType, string message = "No data found."
            , string source = "Data Count", HttpStatusCode? httpStatusCode = HttpStatusCode.NotFound, bool setHttpStatusCode = true);

        IApiResultModel<T> VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message = "No data found.",
            string source = "Data", HttpStatusCode httpStatusCode = HttpStatusCode.NotFound, bool setHttpStatusCode = true);

        /// <summary>
        /// Conditional serialization for Errors.
        /// </summary>
        bool ShouldSerializeErrors();

        /// <summary>
        /// Conditional serialization for Warnings.
        /// </summary>
        bool ShouldSerializeWarnings();
    }
}
