namespace BlockchainWallet.AutoMapper
{
    using global::AutoMapper;
    using Models.Dto;
    using Models.ViewModels;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<AddressDto, AddressViewModel>();
        }
    }
}
