using AutoMapper;
using PRN231_Project.Dto;
using PRN231_Project.Models;

namespace PRN231_Project.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto, Contact>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Label, LabelDto>();
            CreateMap<LabelDto, Label>();
            CreateMap<ContactLabel, Contact_LabelDto>();
            CreateMap<Contact_LabelDto, ContactLabel>();
        }
    }
}
