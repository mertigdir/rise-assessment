using AutoMapper;
using Contacting.Dto.Persons;
using Reporting.Application.Shared.Reports.Dto;
using Reporting.Core.Models.Reports;

namespace Reporting.Application.Mapper.Persons
{
    public class PeportProfile : Profile
    {
        public PeportProfile()
        {
            CreateMap<Report, ReportDto>();
        }
    }
}
