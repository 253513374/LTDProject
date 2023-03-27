using Wtdl.Model.ResponseModel;

namespace Wtdl.Wasm
{
    public class DataStateContainer
    {
        public string Code { get; set; }

        public string OpenId { get; set; }

        public ActivityResult Activity { get; set; }

        public event Action OnChanged;

        public Task SetCode(string code, string openid)
        {
            Code = code;
            OpenId = openid;
            NotifyCodeChanged();
            return Task.CompletedTask;
        }

        public Task SetActivity(ActivityResult activity)
        {
            Activity = activity;
            NotifyCodeChanged();
            return Task.CompletedTask;
        }

        private void NotifyCodeChanged() => OnChanged?.Invoke();
    }
}