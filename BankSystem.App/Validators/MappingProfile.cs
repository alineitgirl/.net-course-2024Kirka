using System.Net.NetworkInformation;
using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace BankSystem.App.Validators
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")));
            
            CreateMap<EmployeeDto, Employee>()
                .ReverseMap();

            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")));
            
            CreateMap<ClientDto, Client>().ReverseMap();
        }
    }
}