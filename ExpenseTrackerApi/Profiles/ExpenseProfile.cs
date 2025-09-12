using AutoMapper;
using ExpenseTrackerApi.Dto;
using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerApi.Profiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile() 
        {
            CreateMap<Expense, ExpenseDto>().ReverseMap();
            CreateMap<Expense, ExpenseForCreationDto>().ReverseMap();
            CreateMap<ExpenseDto, ExpenseForCreationDto>().ReverseMap();
        }
    }
}
