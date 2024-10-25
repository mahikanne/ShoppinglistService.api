using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppinglistService.api.Data;
using ShoppinglistService.api.Model;
using System;
using System.Reflection;

namespace ShoppinglistService.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly DapperDbContext _context;
        public RecipeController(DapperDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        //[Authorize]
        //[ResponseCache(Duration =10,Location =ResponseCacheLocation.Any,NoStore =false)]
        public IEnumerable<Recipe> GetAllRecipes()
        {
            List<Recipe> list = new List<Recipe>();

            var SqlQuery = @"SELECT * FROM [dbo].[Recipe]";

            using var connection = _context.CreateConnection();

            var result = connection.Query<Recipe>(SqlQuery);

            foreach (var item in result)
            {
                var SqlQuery1 = @"SELECT IngredientId,RecipeId,IngredientName as name,IngredientCount as amount FROM [dbo].[RecipeIngredient] where RecipeId = @RecipeId";

                using var connection1 = _context.CreateConnection();

                var result1 = connection.Query<Ingredient>(SqlQuery1, item);
                item.ingredients = result1.ToList();
            }

            return result.ToList();
        }

        [Authorize]
        [HttpGet("{recipeId}")]
        public Recipe GetRecipe(int recipeId)
        {

            var SqlQuery = @"SELECT * FROM [dbo].[Recipe] where RecipeId=@recipeId";

            using var connection = _context.CreateConnection();

            var result = connection.Query<Recipe>(SqlQuery, new { recipeId }).FirstOrDefault();


            var SqlQuery1 = @"SELECT IngredientId,RecipeId,IngredientName as name,IngredientCount as amount FROM [dbo].[RecipeIngredient] where RecipeId = @RecipeId";

            using var connection1 = _context.CreateConnection();

            var result1 = connection.Query<Ingredient>(SqlQuery1, new { recipeId });
            result.ingredients = result1.ToList();


            return result;
        }


        [Authorize]
        [HttpPost]
        public IActionResult InsertRecipeandIngredients([FromBody] Recipe recipe)
        {
            
            string query = @"Insert into Recipe([Name],[Description],[ImagePath]) output inserted.RecipeId values(@name,@description,@imagePath)";
            string query1 = @"Insert into RecipeIngredient(RecipeId,IngredientName,IngredientCount)values(@RecipeId,@name,@amount)";

            using var connection = _context.CreateConnection();
            int no = (int)connection.ExecuteScalar(query, recipe);

            foreach (var item in recipe.ingredients)
            {
                item.RecipeId = no;
                connection.Execute(query1, item);
            }

            return Ok(new { message = "Successfully inserted the recipe" });
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateRecipeandIngredients([FromBody] Recipe recipe)
        {
            string query = "Update Recipe set [Name] =@name,[Description]=@description,[ImagePath]=@imagePath where RecipeId= @RecipeId ";
            string query1 = "Update RecipeIngredient set IngredientName=@name,IngredientCount=@amount where IngredientId=@IngredientId";
            using var connection = _context.CreateConnection();
            connection.Execute(query, recipe);
            foreach (var item in recipe.ingredients)
            {

                connection.Execute(query1, item);
            }

            return Ok(new { message = "Successfully updated the recipe" });
        }

    }
}
