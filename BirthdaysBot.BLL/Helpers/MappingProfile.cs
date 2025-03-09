namespace BirthdaysBot.BLL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Birthday, BirthdayDTO>()
                .ForMember(dest => dest.BirthdayId, opt => opt.MapFrom(src => src.BirthdayId))
                .ForMember(dest => dest.UserChatId, opt => opt.MapFrom(src => src.UserChatId))
                .ForMember(dest => dest.BirthdayName, opt => opt.MapFrom(src => src.BirthdayName))
                .ForMember(dest => dest.BirthdayDate, opt => opt.MapFrom(src => src.BirthdayDate))
                .ForMember(dest => dest.BirthdayTelegramUsername, opt => opt.MapFrom(src => src.BirthdayTelegramUsername));
            CreateMap<BirthdayDTO, Birthday>()
                .ForMember(dest => dest.BirthdayId, opt => opt.MapFrom(src => src.BirthdayId))
                .ForMember(dest => dest.UserChatId, opt => opt.MapFrom(src => src.UserChatId))
                .ForMember(dest => dest.BirthdayName, opt => opt.MapFrom(src => src.BirthdayName))
                .ForMember(dest => dest.BirthdayDate, opt => opt.MapFrom(src => src.BirthdayDate))
                .ForMember(dest => dest.BirthdayTelegramUsername, opt => opt.MapFrom(src => src.BirthdayTelegramUsername));
            CreateMap<UserBirthdayInfo, Birthday>()
                .ForMember(dest => dest.BirthdayName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.BirthdayDate, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(dest => dest.BirthdayTelegramUsername, opt => opt.MapFrom(src => src.TelegramUsername));
        }
    }
}
