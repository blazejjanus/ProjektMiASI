using AutoMapper;
using DB.DBO;
using Services.DTO;

namespace Services.Utils {
    public static class Mapper {
        private static MapperConfiguration MapperConfig = new MapperConfiguration(cfg => {
            cfg.CreateMap<UserDBO, UserDTO>();
            cfg.CreateMap<UserDTO, UserDBO>();
            cfg.CreateMap<AddressDBO, AddressDTO>();
            cfg.CreateMap<AddressDTO, AddressDBO>();
            cfg.CreateMap<JwtDBO, JwtDTO>();
            cfg.CreateMap<JwtDTO, JwtDBO>();
            cfg.CreateMap<CarDBO, CarDTO>();
            cfg.CreateMap<CarDTO, CarDBO>();
            cfg.CreateMap<OrderDBO, OrderDTO>();
            cfg.CreateMap<OrderDTO, OrderDBO>();
        });

        public static AutoMapper.Mapper Get() {
            return new AutoMapper.Mapper(MapperConfig);
        }
    }
}
