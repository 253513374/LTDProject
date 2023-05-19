using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using ScanCode.Share.SignalR;
using ScanCode.WPF.HubServer.Model;
using ScanCode.WPF.HubServer.ReQuest;
using ScanCode.WPF.HubServer.Services;
using ScanCode.WPF.Model;
using System.Windows.Input;
using ScanCode.WPF.View;
using System.Windows;

namespace ScanCode.WPF.ViewModels
{
    public partial class ScanCodeOutViewModel : ObservableObject
    {
        /// <summary>
        /// 出库单的详细单品信息  一个出库单有多个单品出库
        /// </summary>
        public ObservableCollection<OrderDetail> OrderListDetail { set; get; } = new();

        /// <summary>
        /// 出库单的详细单品信息  一个出库单有多个单品出库
        /// </summary>
        public ObservableCollection<W_LabelStorage> ScanOrderOutDetails { set; get; }

        public ObservableCollection<W_LabelStorage> ScanOrderOutDetailsError { set; get; } = new();

        private readonly HubClientService? _hubService;
        private readonly OutOrderService? _outOrderService;
        private readonly ObjectFileStorage? _fileStorage;

        public ScanCodeOutViewModel(HubClientService service, OutOrderService orderService, ObjectFileStorage fileStorage)
        {
            _hubService = service;
            _outOrderService = orderService;
            _fileStorage = fileStorage;

            ScanOrderOutDetails = new();
            ScanOrderOutDetails.CollectionChanged += ScanOrderOutDetails_CollectionChanged;
            if (!string.IsNullOrWhiteSpace(_outOrderService?.OrdersDto?.Ddno))
            {
                _ = GetOrderDetail(_outOrderService?.OrdersDto?.Ddno);
            }

            _ = LoadFileData();
        }

        public string? SelectDdno => _outOrderService?.OrdersDto?.Ddno;

        public string? SelectKh => _outOrderService?.OrdersDto?.Kh;

        public string? SelectDdrq => _outOrderService?.OrdersDto?.Ddrq;

        public int? SelectTotalsl => _outOrderService?.OrdersDto?.Totalsl;

        [ObservableProperty]
        private string? _qrcodeKey;

        [ObservableProperty]
        private int? _actualOutCount;

        [ObservableProperty]
        private int? _actualOutCountError;

        [ObservableProperty]
        private string? _errorInfo;

        [ObservableProperty]
        private string? _foregroundColor;

        /// <summary>
        /// 点击按钮扫码发货
        /// </summary>
        /// <param name="text"></param>
        [RelayCommand]
        private async Task ExecuteTextBox(string text)
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
                if (ActualOutCount >= SelectTotalsl)
                {
                    ErrorInfo = $"当前客户订单已完成扫码工作!";
                    return;
                }

                if (ScanOrderOutDetails.Any(a => a.QRCode.Contains(text)))
                {
                    ErrorInfo = "请不要重复扫码";
                    return;// ScanOrderOutDetailsError.Insert(0, new W_LabelStorageError(text, ));
                }
                await ExecuteScanCode(code);
            }
            else
            {
                ErrorInfo = $"请输入正确的二维码序号";
                return;
            }

            QrcodeKey = "";
            return;
        }

        /// <summary>
        ///Enter按键自动扫码发货
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ExecuteScanCode(string text)
        {
            var outstorage = new W_LabelStorage();
            outstorage.OutTime = DateTime.Now;
            outstorage.OrderNumbels = SelectDdno;
            outstorage.QRCode = text;
            outstorage.OrderTime = DateTime.Parse(SelectDdrq);
            outstorage.Dealers = SelectDdno;
            outstorage.OutType = "THFX";
            outstorage.ExtensionName = "";

            var result = await _hubService.AddScanCodeAsync(outstorage);

            if (result.Successed)
            {
                outstorage.ExtensionName = "出库成功";
                ErrorInfo = "出库成功";

                if (!ScanOrderOutDetails.Any(a => a.QRCode.Contains(text)))
                {
                    ScanOrderOutDetails.Insert(0, outstorage);
                }
            }
            else
            {
                ErrorInfo = $"{text}：{result.Message}";
                if (ScanOrderOutDetailsError.Any(a => a.QRCode.Contains(text)))
                {
                    return false;
                }

                outstorage.ExtensionName = result.Message;
                ScanOrderOutDetailsError.Insert(0, outstorage);

                return false;
            }

            if (_fileStorage != null)
            {
                _fileStorage.Save(ScanOrderOutDetails, SelectDdno);
                _fileStorage.Save(ScanOrderOutDetailsError, $"{SelectDdno}-Error");
            }

            return true;
        }

        /// <summary>
        /// 返回订单号的详细信息
        /// </summary>
        /// <param name="ddno"></param>
        /// <returns></returns>
        public async Task GetOrderDetail(string? ddno)
        {
            var result = await _hubService.GetOrderDetailAsync(ddno);
            OrderListDetail.Clear();
            foreach (var item in result)
            {
                // OrderListDetail.Add(item);
                OrderListDetail.Add(new OrderDetail
                {
                    DDNO = item.DDNO,
                    DDRQ = item.DDRQ,
                    KH = item.KH,
                    XH = item.XH,
                    GGXH = item.GGXH,
                    SL = item.SL,
                    DW = item.DW,
                    DJ = item.DJ,
                    YS = item.YS
                });
            }
            ActualOutCount = await GetActualOutCount();
        }

        //返回实际出库扫码数量
        public async Task<int> GetActualOutCount()
        {
            return await _hubService.GetBdxOrderTotalCountAsync(SelectDdno);
        }

        /// <summary>
        /// 当发货成功 通知发货数量自增1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanOrderOutDetails_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ActualOutCount += 1;
            }
        }

        //初始化加载本地文件数据数据
        private Task LoadFileData()
        {
            if (_fileStorage != null)
            {
                var data = _fileStorage.Load<ObservableCollection<W_LabelStorage>>(SelectDdno)!;
                if (data != null)
                {
                    //ScanOrderOutDetails = data;

                    for (int i = 0; i < data.Count; i++)
                    {
                        ScanOrderOutDetails.Add(data[i]);
                    }
                }
                var dataError = _fileStorage.Load<ObservableCollection<W_LabelStorage>>($"{SelectDdno}-Error")!;
                if (dataError != null)
                {
                    for (int i = 0; i < dataError.Count; i++)
                    {
                        ScanOrderOutDetailsError.Add(data[i]);
                    }
                    //ScanOrderOutDetailsError = dataError;
                }
            }

            return Task.CompletedTask;
        }

        [RelayCommand]
        private async Task RowButton(W_LabelStorage storage)
        {
            if (_hubService != null)
            {
                var t = await _hubService.GetTraceabilityResultAsync(storage.QRCode.Trim());
                var dialogWindow = App.GetService<DialogWindow>();
                dialogWindow.Owner = Application.Current.Windows.OfType<ScanCodeOutWindow>().FirstOrDefault();

                var datacontext = dialogWindow.DataContext as DialogViewModel;

                if (datacontext != null)
                {
                    datacontext.LabelStorage = storage;
                    datacontext.TraceabilityResultDto = t;
                }
                //scanCodeOutWindow.Owner = App.GetService<ScanCodeOutWindow>();

                bool? result = dialogWindow.ShowDialog();

                if (result == true)
                {
                    var returnResult = await _hubService.ScanCodeReturnAsync(storage.QRCode);

                    if (returnResult.ResulCode == 200)
                    {
                        var execute = await ExecuteScanCode(storage.QRCode);
                        if (execute)
                        {
                            ScanOrderOutDetailsError.Remove(storage);
                        }
                    }
                    else
                    {
                        ErrorInfo = $"强制覆盖出库失败：{returnResult.ResultStatus}";
                    }
                    // 用户点击了OK按钮或者其他标识成功的操作
                }
                else
                {
                    // 用户点击了取消按钮，关闭窗口，或者其他标识失败或取消的操作
                }
            }

            // return Task.CompletedTask;
        }

        ///该方法清空ScanOrderOutDetails数据列表
        ///
        [RelayCommand]
        private void ClearScanOrderOutDetails()
        {
            ScanOrderOutDetails.Clear();
            if (_fileStorage != null) _fileStorage.Save(ScanOrderOutDetails, SelectDdno);
        }

        [RelayCommand]
        private void ClearScanOrderOutDetailsError()
        {
            ScanOrderOutDetailsError.Clear();
            if (_fileStorage != null) _fileStorage.Save(ScanOrderOutDetailsError, $"{SelectDdno}-Error");
        }

        // private ActionCommand executeTextBoxCommand;
        // public ICommand ExecuteTextBoxCommand => executeTextBoxCommand ??= new ActionCommand(ExecuteTextBox1);
    }
}