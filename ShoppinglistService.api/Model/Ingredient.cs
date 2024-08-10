namespace ShoppinglistService.api.Model
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public int RecipeId { get; set; }  
        public string name { get; set; }

        public int amount { get; set; }
    }
}
