using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace POE
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, Recipe> dictionaryRecipe = new Dictionary<string, Recipe>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow(dictionaryRecipe);
            addRecipeWindow.ShowDialog();
        }

        private void SearchRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = Microsoft.VisualBasic.Interaction.InputBox("Enter recipe name:", "Search Recipe", "");
            if (dictionaryRecipe.ContainsKey(recipeName))
            {
                RecipeDetailsTextBox.Text = dictionaryRecipe[recipeName].GetDetails();
            }
            else
            {
                MessageBox.Show("Recipe not found.");
            }
        }

        private void DisplayAllRecipesButton_Click(object sender, RoutedEventArgs e)
        {
            RecipeListBox.Items.Clear();
            foreach (var recipeEntry in dictionaryRecipe)
            {
                RecipeListBox.Items.Add(recipeEntry.Key);
            }
        }

        private void FilterByIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = FilterIngredientTextBox.Text.ToLower();
            var filteredRecipes = dictionaryRecipe.Where(r => r.Value.Ingredients.Any(i => i.Name.ToLower().Contains(ingredient)));
            RecipeListBox.Items.Clear();
            foreach (var recipeEntry in filteredRecipes)
            {
                RecipeListBox.Items.Add(recipeEntry.Key);
            }
        }

        private void FilterByFoodGroupButton_Click(object sender, RoutedEventArgs e)
        {
            string foodGroup = (FoodGroupComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var filteredRecipes = dictionaryRecipe.Where(r => r.Value.FoodGroups.Contains(foodGroup));
            RecipeListBox.Items.Clear();
            foreach (var recipeEntry in filteredRecipes)
            {
                RecipeListBox.Items.Add(recipeEntry.Key);
            }
        }

        private void FilterByCaloriesButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(MaxCaloriesTextBox.Text, out double maxCalories))
            {
                var filteredRecipes = dictionaryRecipe.Where(r => r.Value.Calories.Sum() <= maxCalories);
                RecipeListBox.Items.Clear();
                foreach (var recipeEntry in filteredRecipes)
                {
                    RecipeListBox.Items.Add(recipeEntry.Key);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number for calories.");
            }
        }

        private void ApplyScaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipeListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a recipe to scale.");
                return;
            }

            string selectedRecipeName = RecipeListBox.SelectedItem.ToString();
            if (dictionaryRecipe.ContainsKey(selectedRecipeName))
            {
                if (ScaleComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a scale factor.");
                    return;
                }

                double scale = Convert.ToDouble((ScaleComboBox.SelectedItem as ComboBoxItem).Content);
                dictionaryRecipe[selectedRecipeName].ScaleRecipe(scale);
                RecipeDetailsTextBox.Text = dictionaryRecipe[selectedRecipeName].GetDetails();
            }
        }

        private void ResetQuantitiesButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipeListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a recipe to reset quantities.");
                return;
            }

            string selectedRecipeName = RecipeListBox.SelectedItem.ToString();
            if (dictionaryRecipe.ContainsKey(selectedRecipeName))
            {
                dictionaryRecipe[selectedRecipeName].ResetRecipe();
                RecipeDetailsTextBox.Text = dictionaryRecipe[selectedRecipeName].GetDetails();
            }
        }

        private void ShowPieChartButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRecipes = MenuListBox.SelectedItems.Cast<string>().Select(r => dictionaryRecipe[r]).ToList();
            if (selectedRecipes.Count > 0)
            {
                Dictionary<string, double> foodGroupTotals = new Dictionary<string, double>();
                foreach (var recipe in selectedRecipes)
                {
                    for (int i = 0; i < recipe.FoodGroups.Length; i++)
                    {
                        if (foodGroupTotals.ContainsKey(recipe.FoodGroups[i]))
                        {
                            foodGroupTotals[recipe.FoodGroups[i]] += recipe.Calories[i];
                        }
                        else
                        {
                            foodGroupTotals[recipe.FoodGroups[i]] = recipe.Calories[i];
                        }
                    }
                }

                PieChartWindow pieChartWindow = new PieChartWindow(foodGroupTotals);
                pieChartWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select at least one recipe from the menu list.");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
