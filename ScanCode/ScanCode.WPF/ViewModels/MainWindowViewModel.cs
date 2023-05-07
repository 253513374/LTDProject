using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScanCode.WPF.HubServer.Services;
using ScanCode.WPF.Model;

namespace ScanCode.WPF.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private WtdlSqlService hubService;
        private IMapper Mapper;

        public MainWindowViewModel()
        {
            //  groupOrdersDTOs = new ObservableCollection<GroupOrdersDTO>();
            hubService = App.GetService<WtdlSqlService>();
            Mapper = App.GetService<IMapper>();
        }

        public BindingList<GroupOrdersDTO> GroupOrdersDTOs { get; }

        [ObservableProperty]
        private string? searchString;

        [RelayCommand]
        public async Task Search()
        {
            var result = await hubService.GetGroupedBdxOrdersAsync();
            Mapper.Map(result, GroupOrdersDTOs);
            // throw new NotImplementedException();
        }

        [RelayCommand]
        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }
    }
}