using System.Linq;
using AutoMapper;
using Quap.Models;
using Quap.Models.DTO;

namespace Quap.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDetail>()
                .ForMember(dest => dest.totalQuestions, src => src.MapFrom(x => x.questions.Count()));

            CreateMap<TagCreateOrUpdate, Tag>();
        }
    }
}