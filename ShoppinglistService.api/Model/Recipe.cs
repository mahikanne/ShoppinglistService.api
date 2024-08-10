namespace ShoppinglistService.api.Model
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public string imagePath { get; set; }

        public List<Ingredient> ingredients { get; set; }
    }
}
