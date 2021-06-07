using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared
{
    public class Invoker
    {
        public async Task TryInvokeAsync(Func<Task> initialFunc
            , Func<Task> mainFunc
            , Func<Task> finalFunc
            , ILogger logger
            , string callerClass
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData)
        {
            try
            {
                logger.LogInformation($"{callerClass}.{callerMemberName}");

                await initialFunc.Invoke();

                await mainFunc.Invoke();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await finalFunc.Invoke();
            }
        }
    }
}
