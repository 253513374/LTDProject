

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ScanCode.Web.Wasm
{
    public partial class WechatJssdkComponent : ComponentBase
    {
        [Inject]
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        private IJSRuntime JsRuntime { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        private string _appId = "";  // TODO: 替换为您的微信公众号AppID
#pragma warning disable CS0414 // 字段“WechatJssdkComponent.timestamp”已被赋值，但从未使用过它的值
        private string _timestamp = "";
#pragma warning restore CS0414 // 字段“WechatJssdkComponent.timestamp”已被赋值，但从未使用过它的值
#pragma warning disable CS0414 // 字段“WechatJssdkComponent.nonceStr”已被赋值，但从未使用过它的值
        private string _nonceStr = "";
#pragma warning restore CS0414 // 字段“WechatJssdkComponent.nonceStr”已被赋值，但从未使用过它的值
#pragma warning disable CS0414 // 字段“WechatJssdkComponent.signature”已被赋值，但从未使用过它的值
        private string _signature = "";
#pragma warning restore CS0414 // 字段“WechatJssdkComponent.signature”已被赋值，但从未使用过它的值
        private string _jsApiList = ""; // 需要使用的JSSDK API列表，以逗号分隔

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            //await InitializeWechatJssdk();
        }

        private async Task InitializeWechatJssdk()
        {



            // 调用JSInterop的方法获取微信JSSDK配置参数
            var config = await JsRuntime.InvokeAsync<WechatJssdkConfig>("getWechatJssdkConfig");

            // 使用获取到的配置参数进行微信JSSDK初始化
            await JsRuntime.InvokeVoidAsync("wechatJssdkInit", _appId, config.Timestamp, config.NonceStr, config.Signature, _jsApiList);
        }
    }
}
