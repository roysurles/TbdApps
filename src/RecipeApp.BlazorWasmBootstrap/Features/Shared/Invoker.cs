using System;
using System.Reflection;
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




        //public Invoker AddInitialMethod(Action initialMethod)
        //{
        //    return this;
        //}
        //public Invoker AddInitialMethod<TResult>(Func<TResult> initialMethod)
        //{
        //    return this;
        //}

        //public void TryInvoke(delegate del)
        //{
        //    }
    }

    /// <summary>
    /// https://blog.zhaytam.com/2021/03/21/blazor-internals-eventcallback/
    /// </summary>
    public class Invoker2
    {
        public async Task TryInvokeAsync(MulticastDelegate initialDelegate
            , MulticastDelegate mainDelegate
            , MulticastDelegate finallyDelegate)
        {
            try
            {
                await InvokeAsync(initialDelegate);
                await InvokeAsync(mainDelegate);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await InvokeAsync(finallyDelegate);
            }
        }

        protected Task InvokeAsync(MulticastDelegate @delegate)
        {
            switch (@delegate)
            {
                case null:
                    return Task.CompletedTask;

                case Action action:
                    action.Invoke();
                    return Task.CompletedTask;

                case Func<Task> func:
                    return func.Invoke();

                default:
                    {
                        try
                        {
                            return @delegate.DynamicInvoke() as Task ?? Task.CompletedTask;
                        }
                        catch (TargetInvocationException e)
                        {
                            return Task.FromException(e.InnerException!);
                        }
                    }
            }
        }
    }
}
