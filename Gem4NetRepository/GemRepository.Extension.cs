using Gem4NetRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository;
public partial class GemRepository
{

}
/// <summary>
/// 想一想還是不要這樣做...
/// </summary>
public static class UseGemDbcontextExtensions
{
    private static SemaphoreSlim semaphore = new SemaphoreSlim(1);
    public static void GetSingleton(this GemDbContext resource, Action action)
    {
        //using (resource = new(_config))
        //{
        //    semaphore.Wait(); // 嘗試進入，如果有其他執行緒已經進入，則會等待

        //    try
        //    {
        //        action.Invoke();
        //        // 這裡的程式碼只能由一個執行緒同時執行
        //        // ...
        //    }
        //    finally
        //    {
        //        semaphore.Release(); // 釋放 SemaphoreSlim，允許其他等待的執行緒進入
        //    }
           
        //}
    }
}
/// <summary>
/// 不太確定我做了啥
/// </summary>
public class ThreadSafeClassProxy : DispatchProxy
{
    private object target;
    private SemaphoreSlim semaphore = new SemaphoreSlim(1);

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        semaphore.Wait();
        try
        {
            return targetMethod.Invoke(target, args);
        }
        finally
        {
            semaphore.Release();
        }
    }

    public static T Create<T>(T target)
    {
        object proxy = Create<T, ThreadSafeClassProxy>();
        ((ThreadSafeClassProxy)proxy).target = target;
        return (T)proxy;
    }
}
