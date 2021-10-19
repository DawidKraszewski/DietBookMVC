using AutoMapper;
using DietBook.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietBook.MVC.Profiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeEdit>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Author, o => o.MapFrom(s => s.Author))
                .ForMember(d => d.Photo, o => o.MapFrom(s => s.Photo))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate))
                .ForMember(d => d.Steps, o => o.MapFrom(s => s.Steps))
                .ForMember(d => d.RecipeIngredients, o => o.MapFrom(s => s.RecipeIngredients))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
        }

    }
}
