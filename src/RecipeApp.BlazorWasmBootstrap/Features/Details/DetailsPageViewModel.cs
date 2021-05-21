
using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Introduction;

namespace RecipeApp.BlazorWasmBootstrap.Features.Details
{
    public class DetailsPageViewModel : BaseViewModel, IDetailsPageViewModel
    {
        protected readonly ILogger<DetailsPageViewModel> _logger;

        public DetailsPageViewModel(ILogger<DetailsPageViewModel> logger)
        {
            _logger = logger;
        }

        public IntroductionDto Introduction { get; protected set; } =
            new IntroductionDto();

        public async Task InitializeAsync(Guid intoductionId)
        {
            _logger.LogInformation($"{nameof(DetailsPageViewModel)}({intoductionId})");

            // TODO:  make api call to get Introduction
            Introduction = Guid.Empty == intoductionId
                ? new IntroductionDto()
                : new IntroductionDto();
        }
    }

    public interface IDetailsPageViewModel : IBaseViewModel
    {
        public IntroductionDto Introduction { get; }

        Task InitializeAsync(Guid intoductionId);
    }
}
