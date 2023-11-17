using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScanCode.Share;
using ScanCode.WPF.HubServer.Services;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace ScanCode.WPF.ViewModels
{
    public partial class ScanCodeReturnViewModel : ObservableObject
    {
        [ObservableProperty] private string? _errorReturnInfo;

        [ObservableProperty] private int _actualReturnCount;

        [ObservableProperty] private string? _qrcodeKey;
        public ObservableCollection<ReturnsStorageResult> StorageResults { set; get; }

        private readonly HubClientService? _hubService;

        //private readonly OutOrderService? outOrderService;
        public ScanCodeReturnViewModel(HubClientService service)
        {
            _hubService = service;
            //outOrderService = orderService;
            StorageResults = new ObservableCollection<ReturnsStorageResult>();
            StorageResults.CollectionChanged += StorageResults_CollectionChanged;
        }

        private void StorageResults_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ActualReturnCount += 1;
            }
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 点击按钮扫码发货
        /// </summary>
        /// <param name="text"></param>
        [RelayCommand]
        private async Task ExecuteReturnTextBox(string text)
        {
            ErrorReturnInfo = "";

            var code = App.ReplaceScanCodeDdnoLenth(text);

            await ExecuteScanCode(code);
            //bool isNumber = Regex.IsMatch(code, @"^[1-9]\d*$");
            //if (isNumber)
            //{
            //    await ExecuteScanCode(code);
            //}
            //else
            //{
            //    ErrorReturnInfo = $"请输入正确的二维码序号";
            //    return;
            //}

            QrcodeKey = "";
            return;
            // await ExecuteScanCode(text);
        }

        private async Task ExecuteScanCode(string text)
        {
            try
            {
                var stringType = StringAnalyzer.AnalyzeString(text);
                if (stringType == StringType.Numeric)
                {
                    if (string.IsNullOrWhiteSpace(text) || text.Length < 12)
                    {
                        ErrorReturnInfo = $"请输入正确的二维码序号";
                        return;
                    }
                }

                if (stringType == StringType.Alphanumeric)
                {
                    if (string.IsNullOrWhiteSpace(text) || text.Length < 6)
                    {
                        ErrorReturnInfo = $"请输入正确的二维码序号";
                        return;
                    }
                }

                var result = await _hubService.ScanCodeReturnAsync(text, stringType);

                if (result.ResulCode == 200)
                {
                    if (result.IsDdno)
                    {
                        //订单批量退货
                        for (int i = 0; i < result.QrCodeList.Count; i++)
                        {
                            StorageResults.Insert(0, ReturnsStorageResult.Success(result.QrCodeList[i]));
                        }
                        ErrorReturnInfo = "退货成功";
                    }
                    else
                    {
                        //单码退货
                        StorageResults.Insert(0, result);
                        ErrorReturnInfo = "退货成功";
                    }
                }
                else
                {
                    ErrorReturnInfo = $"{text}退货失败：{result.ResultStatus}";
                }
            }
            catch (Exception e)
            {
                ErrorReturnInfo = $"{e.Message}";

                //  throw;
            }

            //throw new NotImplementedException();
        }
    }
}