using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Quap.Models;
using Quap.Models.DTO;

namespace Quap.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            AllowNullCollections = true;

            CreateMap<Question, QuestionSummary>()
                .ForMember(x => x.createdByUsername, o => o.MapFrom(x => x.createdBy.username))
                .ForMember(x => x.answersCount, o => o.MapFrom(x => x.answers.Count()))
                .ForMember(x => x.hasAcceptedAnswer, o => o.MapFrom(x => x.answers.Any(a => a.accepted)))
                .ForMember(x => x.tags, o => o.ConvertUsing(new TagsConverter()));
                //.AfterMap<TagsAction>();

            //.ForMember(x => x.tags, o => o.MapFrom(x => x.tags.Select(t => t.tag).ToList()));

        }
    }

    public class TagsConverter : IValueConverter<ICollection<QuestionTag>, ICollection<string>>
    {
        public ICollection<string> Convert(ICollection<QuestionTag> sourceMember, ResolutionContext context)
        {
            return sourceMember.Select(t => t.tag.name).ToList();
        }
    }

    public class TagsAction : IMappingAction<Question, QuestionSummary>
    {
        public void Process(Question source, QuestionSummary destination, ResolutionContext context)
        {
            destination.tags = source.tags.Select(t => t.tag.name).ToList();
        }
    }

    // public class CustomResolver : IValueResolver<personSrc, personDest, List<address>>
    // {
    //     public List<address> Resolve(personSrc source, personDest destination, List<address> destMember, ResolutionContext context)
    //     {
    //         List<address> result = new List<adress>();
    //         if (!String.IsNullOrEmpty(source.HomeAddress))
    //         {
    //             result.add(new Address
    //             {
    //                 location = source.HomeAddress,
    //                 type = addressType.Home
    //             });
    //         }
    //         if (!String.IsNullOrEmpty(source.OfficeAddress))
    //         {
    //             result.add(new Address
    //             {
    //                 location = source.OfficeAddress,
    //                 type = addressType.Office
    //             });
    //         }
    //         return result;
    //     }
    // }
}