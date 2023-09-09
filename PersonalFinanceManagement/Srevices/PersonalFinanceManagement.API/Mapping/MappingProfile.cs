using AutoMapper;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;

namespace PersonalFinanceManagement.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<TransactionCreateDTO, Transaction>();
            CreateMap<Transaction, TransactionWithCategory>();
                //.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                //.ForMember(dest => dest.CategoryIsIncome, opt => opt.MapFrom(src => src.Category.IsIncome));
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<Wallet, Domain.DTOModels.WalletDTO>().ReverseMap();
            CreateMap<WalletCreateDTO, Wallet>();
            CreateMap<User, UserDTO>();
        }
    }
}
