// ModelLayer/MappingProfile.cs
using AutoMapper;
using ModelLayer.Model;
using ReposatoryLayer.Entity;

public class AddressMapping : Profile
{
    public AddressMapping()
    {
        CreateMap<AddressEntity, AddressBookDTO>();
        CreateMap<RequestModel, AddressEntity>();
    }
}
