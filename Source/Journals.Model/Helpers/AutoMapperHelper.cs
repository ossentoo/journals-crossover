using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Journals.Model;

namespace Medico.Model.Helpers
{
    public static class AutoMapperHelper
    {
        public static void Initialize()
        {
            Mapper.CreateMap<Journal, JournalViewModel>();
            Mapper.CreateMap<JournalViewModel, Journal>();

            Mapper.CreateMap<Journal, JournalUpdateViewModel>();
            Mapper.CreateMap<JournalUpdateViewModel, Journal>();

            Mapper.CreateMap<Journal, SubscriptionViewModel>();
            Mapper.CreateMap<SubscriptionViewModel, Journal>();

        }
    }
}
