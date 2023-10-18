using AutoMapper;
using DB.DBO;
using Services.DTO;

namespace Services {
    public static class Mapper {
        private static MapperConfiguration MapperConfig = new MapperConfiguration(cfg => {
            cfg.CreateMap<UserDBO, UserDTO>();
            cfg.CreateMap<UserDTO, UserDBO>();
            cfg.CreateMap<AddressDBO, AddressDTO>();
            cfg.CreateMap<AddressDTO, AddressDBO>();
            cfg.CreateMap<JwtDBO, JwtDTO>();
            cfg.CreateMap<JwtDTO, JwtDBO>();
        });

        public static AutoMapper.Mapper Get() {
            return new AutoMapper.Mapper(MapperConfig);
        }
    }
}
