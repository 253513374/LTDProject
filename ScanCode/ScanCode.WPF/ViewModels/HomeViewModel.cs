using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScanCode.Share;
using ScanCode.WPF.HubServer.Services;
using ScanCode.WPF.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ScanCode.WPF.View;

namespace ScanCode.WPF.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        public ObservableCollection<GroupOrdersDTO> GroupOrdersDTOs { get; set; }

        [ObservableProperty] private string? querykeywords = "";

        [ObservableProperty] private string? collectionCount;

        private HubClientService hubService;
        private IMapper Mapper;

        public HomeViewModel()
        {
            GroupOrdersDTOs = new ObservableCollection<GroupOrdersDTO>();
            GroupOrdersDTOs.CollectionChanged +=
                (_, _) => OnPropertyChanged(
                    nameof(CollectionCount)); // CollectionCount = $"DDNO  {GroupOrdersDTOs.Count}";)
            hubService = App.GetService<HubClientService>();
            Mapper = App.GetService<IMapper>();
            _ = Search();
        }

        [RelayCommand]
        public async Task Search()
        {
            //

            if (GroupOrdersDTOs.Any())
            {
                GroupOrdersDTOs.Clear();
            }

            if (string.IsNullOrWhiteSpace(Querykeywords))
            {
                //GroupOrdersDTOs = new ObservableCollection<GroupOrdersDTO>();
                var result = await hubService.GetGroupedBdxOrdersAsync();
                result = result.OrderByDescending(O => O.DDRQ).ToList();
                var results = new ObservableCollection<GroupOrdersDTO>(
                    Mapper.Map<ObservableCollection<GroupOrdersDTO>>(result));

                foreach (var VARIABLE in results)
                {
                    GroupOrdersDTOs?.Add(VARIABLE);
                }
            }
            else
            {
                var hubResult = await hubService.GetGroupedBdxOrdersAsync(Querykeywords);

                hubResult = hubResult.OrderByDescending(O => O.DDRQ).ToList();
                var results = new ObservableCollection<GroupOrdersDTO>(
                    Mapper.Map<ObservableCollection<GroupOrdersDTO>>(hubResult));

                //  GroupOrdersDTOs = new ObservableCollection<GroupOrdersDTO>();
                foreach (var VARIABLE in results)
                {
                    GroupOrdersDTOs?.Add(VARIABLE);
                }
            }

            CollectionCount = $"DDNO  {GroupOrdersDTOs.Count}";
            //  Mapper.Map(result, GroupOrdersDTOs);
            // throw new NotImplementedException();
        }

        [RelayCommand]
        public void ItemButtonClick(object? selectedItem)
        {
            if (selectedItem is null)
            {
                return;
            }
            var outOrder = App.GetService<OutOrderService>();
            outOrder.OrdersDto = selectedItem as GroupOrdersDTO;
            // 在这里处理按钮点击事件逻辑，并使用传递的选中项数据

            var scanCodeOutWindow = App.GetService<ScanCodeOutWindow>();

            scanCodeOutWindow.Owner = App.GetService<HomeWindow>();
            scanCodeOutWindow.ShowDialog();
        }

        //[RelayCommand]
        //private void OpenScanCodeOutDialog()
        //{
        //    ScanCodeOutWindow scanCodeOutWindow = new ScanCodeOutWindow();

        //    scanCodeOutWindow.ShowDialog();
        //}
    }
}