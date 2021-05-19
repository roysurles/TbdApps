﻿using System.Collections.Generic;

using Tbd.Shared.ApiResult;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Models
{
    public class BaseViewModel : IBaseViewModel
    {
        public bool IsLoading { get; set; }

        public bool IsBusy { get; set; }

        public List<IApiResultMessageModel> ApiResultMessages { get; set; } = new List<IApiResultMessageModel>();

        public IBaseViewModel ClearApiResultMessages()
        {
            ApiResultMessages = new List<IApiResultMessageModel>();
            return this;
        }

        protected IBaseViewModel AddApiResultMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, int? code = null)
        {
            ApiResultMessages.Add(new ApiResultMessageModel
            {
                MessageType = apiResultMessageType,
                Message = message,
                Source = source,
                Code = code
            });
            return this;
        }

        protected IBaseViewModel AddInformationMessage(string message, string source = null, int? code = null) =>
            AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Information, message, source, code);

        protected IBaseViewModel AddWarningMessage(string message, string source = null, int? code = null) =>
            AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Warning, message, source, code);

        protected IBaseViewModel AddErrorMessage(string message, string source = null, int? code = null) =>
            AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Error, message, source, code);

        protected IBaseViewModel AddMessages(IEnumerable<ApiResultMessageModel> messages)
        {
            ApiResultMessages.AddRange(messages);
            return this;
        }
    }

    public interface IBaseViewModel
    {
        bool IsLoading { get; set; }

        bool IsBusy { get; set; }

        List<IApiResultMessageModel> ApiResultMessages { get; set; }

        IBaseViewModel ClearApiResultMessages();
    }
}