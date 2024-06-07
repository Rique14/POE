using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POE
{
    public partial class AddRecipeWindow : Window
    {
        private Dictionary<string, Recipe> dictionaryRecipe;

        public AddRecipeWindow(Dictionary<string, Recipe> dictionaryRecipe)
        {
            InitializeComponent();
            this.dictionaryRecipe = dictionaryRecipe;
        }

        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = IngredientTextBox.Text;
            if (double.TryParse(QuantityTextBox.Text, out double quantity) && double.TryParse(CaloriesTextBox.Text, out double calories))
            {
                string unit = UnitTextBox.Text;
                string foodGroup = FoodGroupTextBox.Text;
                IngredientsListBox.Items.Add(new Ingredient { Name = ingredient, Quantity = quantity, Unit = unit, Calories = calories, FoodGroup = foodGroup });
                IngredientTextBox.Clear();
                QuantityTextBox.Clear();
                UnitTextBox.Clear();
                CaloriesTextBox.Clear();
                FoodGroupTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter valid values for quantity and calories.");
            }
        }

        private void AddStepButton_Click(object sender, RoutedEventArgs e)
        {
            string step = StepTextBox.Text;
            StepsListBox.Items.Add(step);
            StepTextBox.Clear();
        }

        private void SaveRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = RecipeNameTextBox.Text;
            if (!string.IsNullOrEmpty(recipeName) && !dictionaryRecipe.ContainsKey(recipeName))
            {
                var ingredients = new List<Ingredient>();
                foreach (Ingredient ingredient in IngredientsListBox.Items)
                {
                    ingredients.Add(ingredient);
                }

                var steps = new List<string>();
                foreach (string step in StepsListBox.Items)
                {
                    steps.Add(step);
                }

                var recipe = new Recipe(ingredients, steps.ToArray());
                dictionaryRecipe.Add(recipeName, recipe);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid recipe name that does not already exist.");
            }
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }

        public override string ToString()
        {
            return $"{Quantity} {Unit} of {Name} ({Calories} calories, {FoodGroup})";
        }
    }

    public class Recipe
    {
        public List<Ingredient> Ingredients { get; set; }
        public string[] Steps { get; set; }

        public Recipe(List<Ingredient> ingredients, string[] steps)
        {
            Ingredients = ingredients;
            Steps = steps;
        }

        public string GetDetails()
        {
            string details = "Ingredients:\n";
            foreach (var ingredient in Ingredients)
            {
                details += $" - {ingredient}\n";
            }

            details += "\nSteps:\n";
            for (int i = 0; i < Steps.Length; i++)
            {
                details += $" {i + 1}. {Steps[i]}\n";
            }

            return details;
        }

        public double[] Calories => Ingredients.Select(i => i.Calories).ToArray();
        public string[] FoodGroups => Ingredients.Select(i => i.FoodGroup).ToArray();

        public void ScaleRecipe(double scale)
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity *= scale;
            }
        }

        public void ResetRecipe()
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity /= 2;
            }
        }
    }
}
