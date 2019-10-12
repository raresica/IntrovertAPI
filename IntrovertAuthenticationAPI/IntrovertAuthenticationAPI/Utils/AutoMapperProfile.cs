/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using AutoMapper;
using IntrovertAuthenticationAPI.DTOs;
using IntrovertAuthenticationAPI.Entities;

namespace IntrovertAuthenticationAPI.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserEntity, UserRxDto>();
            CreateMap<UserRxDto, UserEntity>();

            CreateMap<UserEntity, UserTxDto>();
            CreateMap<UserTxDto, UserEntity>();
        }
    }
}
