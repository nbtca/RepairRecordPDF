using Newtonsoft.Json;
using SaturdayAPI.Core;
using SaturdayAPI.Core.Types;

namespace SaturdayAPI.Wrapper;

/// <summary>
/// 带缓存的 API
/// 以减少对后端的请求
/// 包装了 <see cref="Core.Api"/>
/// </summary>
public class ApiWithCache
{
    public ApiWithCache(string cacheRoot, Api? api = null)
    {
        Api = api ?? new();
        CacheRoot = cacheRoot;
        if (!Directory.Exists(cacheRoot))
            Directory.CreateDirectory(cacheRoot);
    }

    private string CacheRoot { get; }

    internal Api Api { get; }

    public async Task<EventInfo[]> GetEvents()
    {
        var cacheJson = Path.Combine(CacheRoot, "events.json");
        var result = await Api.GetEvents(); //总是获取最新的数据
        await File.WriteAllTextAsync(cacheJson, JsonConvert.SerializeObject(result));
        return result;
    }

    public async Task<EventInfo> GetEventById(int eventId)
    {
        var eventsRoot = Path.Combine(CacheRoot, "events"); //事件缓存位置
        var cacheJson = Path.Combine(eventsRoot, $"{eventId}.json"); //保存的文件位置
        if (File.Exists(cacheJson)) //文件存在=>已经缓存
        {
            var content = await File.ReadAllTextAsync(cacheJson);
            var cacheData = //解析缓存的数据
                JsonConvert.DeserializeObject<EventInfo>(content)
                ?? throw new NullReferenceException(nameof(content));
            if (cacheData.Status == Status.Closed) //如果已经关闭
                return cacheData; //返回缓存
            //否则获取新数据
        }
        var result = await Api.GetEventById(eventId);
        if (!Directory.Exists(eventsRoot))
            Directory.CreateDirectory(eventsRoot);
        await File.WriteAllTextAsync(cacheJson, JsonConvert.SerializeObject(result));
        return result;
    }

    public async Task<MemberInfo[]> GetMembers()
    {
        return await Api.GetMembers();
    }
}
