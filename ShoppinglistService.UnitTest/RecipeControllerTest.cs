using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppinglistService.api.Controllers;
using ShoppinglistService.api.Data;
using ShoppinglistService.api.Model;

namespace ShoppinglistService.UnitTest
{
    public class RecipeControllerTest
    {

        private readonly RecipeController _controller;
        private readonly Mock<DapperDbContext> _mockService;


        //public RecipeControllerTest()
        //{
        //    _mockService = new Mock<DapperDbContext>();
        //    _controller = new RecipeController(_mockService.Object);
        //}

        [Fact]
        public void GetRecipeList()
        {
            //List<Recipe> expectedrecipes = new List<Recipe>() {
            //    new Recipe()
            //    {
            //        RecipeId=1,
            //        name="A Test Recipe!",
            //        description="This is simply a test",
            //        imagePath="https://www.jocooks.com/wp-content/uploads/2019/04/pork-schnitzel-1.jpg",
            //        ingredients=new List<Ingredient> {
            //          new Ingredient()
            //          {
            //              IngredientId=1,
            //              RecipeId =1,
            //              name="Meat",
            //              amount=1
            //           } ,
            //          new Ingredient()
            //          {
            //              IngredientId=2,
            //              RecipeId =1,
            //              name="French Fries",
            //              amount=20
            //           }

            //        }


            //    }
            //};


            //_mockService.Setup(service=> service.CreateConnection());

            ////Act

            //var result = _controller.GetAllRecipes();

            //var okresult = Assert.IsType<OkObjectResult>(result);

            //var returnValue = Assert.IsType<List<Recipe>>(okresult.Value);

            Assert.IsType<NoContentResult>(null);
            
        }
    }
}