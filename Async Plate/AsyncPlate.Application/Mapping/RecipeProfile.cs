using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.DTOs.Supplier;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Mapping
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
