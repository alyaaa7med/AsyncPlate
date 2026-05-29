using AsyncPlate.Core.DTOs;
using AsyncPlate.Core.DTOs.Recipe;
using AsyncPlate.Core.DTOs.Supplier;
using AsyncPlate.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Mapping
{
    public class RecipeProfile : Profile 
    {
        public RecipeProfile()
        {
            
            CreateMap<AddRecipeRequestDTO, Recipe>();

            CreateMap<UpdateRecipeRequestDTO, Recipe>();
            CreateMap<Recipe, RecipeResponseDTO>();

            CreateMap<Product, ProductSummaryDTO>();

            CreateMap<Inventory, InventorySummaryDTO>();
        }
    }
}
