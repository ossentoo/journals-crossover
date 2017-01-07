using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using AutoMapper;
using Journals.Model;

namespace Medico.Model.Helpers
{
    public static class AutoMapperHelper
    {
        public static void Initialize()
        {
            Mapper.CreateMap<Journal, JournalViewModel>()
                .ForMember(x => x.ContentType, opt => opt.MapFrom(o => o.Issues.First()!=null? o.Issues.First().ContentType:string.Empty))
                .ForMember(x => x.Content, opt => opt.MapFrom(o => o.Issues.First() != null ? o.Issues.First().Content : null));

            Mapper.CreateMap<JournalViewModel, Journal>()
                .ForMember(x=>x.ModifiedDate,  opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(x => x.Issues, opt => opt.MapFrom(o => new Collection<JournalIssue>
                {
                    new JournalIssue { ModifiedDate = DateTime.UtcNow, Content = o.Content, ContentType = o.ContentType, FileName = o.FileName }
                }));

            Mapper.CreateMap<Journal, JournalUpdateViewModel>()
                .ForMember(x => x.ContentType, opt => opt.MapFrom(o => o.Issues.First() != null ? o.Issues.First().ContentType : null))
                .ForMember(x => x.Content, opt => opt.MapFrom(o => o.Issues.First() != null ? o.Issues.First().Content : null));

            Mapper.CreateMap<JournalUpdateViewModel, Journal>()
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(x => x.Issues, opt => opt.MapFrom(o => new Collection<JournalIssue>
                {
                    new JournalIssue { ModifiedDate = DateTime.UtcNow, Content = o.Content, ContentType = o.ContentType, FileName = o.FileName }
                }));

            Mapper.CreateMap<Journal, SubscriptionViewModel>();
            Mapper.CreateMap<SubscriptionViewModel, Journal>();
        }
    }
}
