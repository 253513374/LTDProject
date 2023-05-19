using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ScanCode.Share;
using ScanCode.WPF.Model;

namespace ScanCode.WPF
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<GroupOrdersDto, GroupedBdxOrder>();
            CreateMap<GroupedBdxOrder, GroupOrdersDto>();
        }
    }
}