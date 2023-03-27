

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Wtdl.Wasm
{
    public partial class WechatJssdkComponent : ComponentBase
    {
        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        private string appId = "";  // TODO: 替换为您的微信公众号AppID
        private string timestamp = "";
        private string nonceStr = "";
        private string signature = "";
        private string jsApiList = ""; // 需要使用的JSSDK API列表，以逗号分隔

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            //await InitializeWechatJssdk();
        }

        private async Task InitializeWechatJssdk()
        {



            // 调用JSInterop的方法获取微信JSSDK配置参数
            var config = await jsRuntime.InvokeAsync<WechatJssdkConfig>("getWechatJssdkConfig");

            // 使用获取到的配置参数进行微信JSSDK初始化
            await jsRuntime.InvokeVoidAsync("wechatJssdkInit", appId, config.Timestamp, config.NonceStr, config.Signature, jsApiList);
        }
    }
}
