using System;
using System.Threading.Tasks;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared
{
    public class Invoker
    {
        public async Task TryInvokeAsync(Func<Task> initialFunc
            , Func<Task> mainFunc
            , Func<Task> finalFunc)
        {
            try
            {
                if (initialFunc is not null)
                    await initialFunc.Invoke();

                if (mainFunc is not null)
                    await mainFunc.Invoke();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (finalFunc is not null)
                    await finalFunc.Invoke();
            }
        }
    }
}
