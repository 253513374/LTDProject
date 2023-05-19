using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScanCode.Share;
using ScanCode.WPF.HubServer.Services;
using ScanCode.WPF.Model;

namespace ScanCode.WPF.ViewModels
{
    public partial class ScanCodeReturnViewModel : ObservableObject
    {
        [ObservableProperty] private string? _errorInfo;

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
            ErrorInfo = "";
            if (string.IsNullOrWhiteSpace(text) || text.Length < 12)
            {
                ErrorInfo = $"请输入正确的二维码序号";
                return;
            }
            var code = App.ReplaceScanCode(text);
            bool isNumber = Regex.IsMatch(code, @"^[1-9]\d*$");
            if (isNumber)
            {
                await ExecuteScanCode(code);
            }
            else
            {
                ErrorInfo = $"请输入正确的二维码序号";
                return;
            }

            QrcodeKey = "";
            return;
            // await ExecuteScanCode(text);
        }

        private async Task ExecuteScanCode(string text)
        {
            var result = await _hubService.ScanCodeReturnAsync(text);

            if (result.ResulCode == 200)
            {
                StorageResults.Insert(0, result);
                ErrorInfo = "退货成功";
            }
            else
            {
                ErrorInfo = $"{text}退货失败：{result.ResultStatus}";
            }
            //throw new NotImplementedException();
        }
    }
}