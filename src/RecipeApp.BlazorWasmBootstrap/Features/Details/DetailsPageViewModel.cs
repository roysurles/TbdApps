
using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;

namespace RecipeApp.BlazorWasmBootstrap.Features.Details
{
    public class DetailsPageViewModel : BaseViewModel, IDetailsPageViewModel
    {
        protected readonly ILogger<DetailsPageViewModel> _logger;

        public DetailsPageViewModel(ILogger<DetailsPageViewModel> logger)
        {
            _logger = logger;
        }

        public Guid IntoductionId { get; protected set; }

        public async Task InitializeAsync(Guid intoductionId)
        {
            _logger.LogInformation($"{nameof(DetailsPageViewModel)}({intoductionId})");

            IntoductionId = intoductionId;
        }
    }

    public interface IDetailsPageViewModel : IBaseViewModel
    {
        Guid IntoductionId { get; }

        Task InitializeAsync(Guid intoductionId);
    }
}
