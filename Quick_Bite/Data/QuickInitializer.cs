using Microsoft.EntityFrameworkCore;
using Quick_Bite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick_Bite.Data
{
    public class QuickInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider,
            bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true)
        {
            using (var context = new QuickContext(
                serviceProvider.GetRequiredService<DbContextOptions<QuickContext>>()))
            {
                #region Prepare the Database
                if (DeleteDatabase || !context.Database.CanConnect())
                {
                    await context.Database.EnsureDeletedAsync(); // Delete the existing version 
                    if (UseMigrations)
                    {
                        await context.Database.MigrateAsync(); // Create the Database and apply all migrations
                    }
                    else
                    {
                        await context.Database.EnsureCreatedAsync(); // Create and update the database 
                    }
                }
                #endregion

                if (SeedSampleData)
                {
                    #region Seed Recipes
                    if (!await context.Recipes.AnyAsync())
                    {
                        var recipes = new List<Recipe>
                        {
                            // Breakfast
                         new Recipe
                            {
                                RecipeName = "Classic Pancakes",
                                Ingredients = "1 cup flour\n1 tbsp sugar\n2 tsp baking powder\n1/2 tsp salt\n1 cup milk\n1 egg\n2 tbsp melted butter",
                                Instructions = "1. Mix dry ingredients\n2. Add wet ingredients\n3. Cook on griddle\n4. Serve with syrup",
                                MealCategory = Recipe.MealType.Breakfast,
                                DietaryCategory = Recipe.Category.Dairy,
                                PrepTime = 10,
                                CookTime = 15,
                                Servings = 4,
                                ImageUrl = "Classic-Pancakes.jpg"
                            },
                            new Recipe
                            {
                                   RecipeName = "Avocado Toast",
                                    Ingredients = "2 slices whole grain bread\n1 ripe avocado\n1 tbsp lemon juice\nSalt and pepper to taste\nRed pepper flakes (optional)",
                                    Instructions = "1. Toast bread\n2. Mash avocado with lemon juice\n3. Spread on toast\n4. Season to taste",
                                    MealCategory = Recipe.MealType.Breakfast,
                                    DietaryCategory = Recipe.Category.Vegan,
                                    PrepTime = 5,
                                    CookTime = 5,
                                    Servings = 2,
                                    ImageUrl = "Avocado-Toast.jpg"
                                },
                            new Recipe
                                {
                                    RecipeName = "Greek Yogurt Parfait",
                                    Ingredients = "1 cup Greek yogurt\n1/2 cup granola\n1/2 cup mixed berries\n1 tbsp honey",
                                    Instructions = "1. Layer yogurt, granola and berries\n2. Drizzle with honey",
                                    MealCategory = Recipe.MealType.Breakfast,
                                    DietaryCategory = Recipe.Category.GlutenFree,
                                    PrepTime = 5,
                                    CookTime = 0,
                                    Servings = 1,
                                    ImageUrl = "Greek-Yogurt-Parfait.jpg"
                                },

                           // Lunch
    new Recipe
    {
        RecipeName = "Chicken Caesar Wrap",
        Ingredients = "1 large tortilla\n1 grilled chicken breast\n2 cups romaine lettuce\n1/4 cup Caesar dressing\n1/4 cup shredded Parmesan\n1/4 cup croutons",
        Instructions = "1. Layer ingredients on tortilla\n2. Roll tightly\n3. Cut in half",
        MealCategory = Recipe.MealType.Lunch,
        DietaryCategory = Recipe.Category.MeatLover,
        PrepTime = 10,
        CookTime = 0,
        Servings = 1,
        ImageUrl = "Chicken-Caesar-Wrap.jpg"
    },
    new Recipe
    {
        RecipeName = "Quinoa Salad Bowl",
        Ingredients = "1 cup cooked quinoa\n1/2 cup chickpeas\n1/2 cup cucumber\n1/4 cup feta cheese\n2 tbsp olive oil\n1 tbsp lemon juice",
        Instructions = "1. Combine all ingredients\n2. Toss with dressing\n3. Chill before serving",
        MealCategory = Recipe.MealType.Lunch,
        DietaryCategory = Recipe.Category.Vegetarian,
        PrepTime = 15,
        CookTime = 0,
        Servings = 2,
        ImageUrl = "Quinoa-Salad-Bowl.jpg"
    },
    new Recipe
    {
        RecipeName = "Turkey Club Sandwich",
        Ingredients = "3 slices bread\n6 slices turkey breast\n2 strips bacon\n2 leaves lettuce\n2 slices tomato\n2 tbsp mayonnaise",
        Instructions = "1. Toast bread\n2. Layer ingredients\n3. Cut diagonally",
        MealCategory = Recipe.MealType.Lunch,
        DietaryCategory = Recipe.Category.MeatLover,
        PrepTime = 10,
        CookTime = 5,
        Servings = 1,
        ImageUrl = "Turkey-Club-Sandwich.jpg"
    },

    // Dinner
    new Recipe
    {
        RecipeName = "Vegetable Stir Fry",
        Ingredients = "2 cups mixed vegetables\n1 tbsp oil\n2 tbsp soy sauce\n1 tsp ginger\n1 clove garlic",
        Instructions = "1. Heat oil\n2. Add vegetables\n3. Add seasonings\n4. Cook until tender",
        MealCategory = Recipe.MealType.Dinner,
        DietaryCategory = Recipe.Category.Vegetarian,
        PrepTime = 15,
        CookTime = 10,
        Servings = 2,
        ImageUrl = "Vegetable-Stir-Fry.jpg"
    },
    new Recipe
    {
        RecipeName = "Grilled Salmon",
        Ingredients = "2 salmon fillets\n1 tbsp olive oil\n1 tsp lemon zest\n1/2 tsp salt\n1/4 tsp black pepper\n1 tbsp fresh dill",
        Instructions = "1. Preheat grill\n2. Season salmon\n3. Grill 4-5 minutes per side",
        MealCategory = Recipe.MealType.Dinner,
        DietaryCategory = Recipe.Category.GlutenFree,
        PrepTime = 10,
        CookTime = 10,
        Servings = 2,
        ImageUrl = "Grilled-Salmon.jpg"
    },
    new Recipe
    {
        RecipeName = "Beef Tacos",
        Ingredients = "1 lb ground beef\n1 packet taco seasoning\n8 taco shells\nToppings: lettuce, tomato, cheese, sour cream",
        Instructions = "1. Brown beef\n2. Add seasoning\n3. Fill shells\n4. Add toppings",
        MealCategory = Recipe.MealType.Dinner,
        DietaryCategory = Recipe.Category.MeatLover,
        PrepTime = 15,
        CookTime = 15,
        Servings = 4,
        ImageUrl = "Beef-Tacos.jpg"
    },

    // Desserts
    new Recipe
    {
        RecipeName = "Chocolate Chip Cookies",
        Ingredients = "2 1/4 cups flour\n1 tsp baking soda\n1 tsp salt\n1 cup butter\n3/4 cup sugar\n3/4 cup brown sugar\n2 eggs\n2 cups chocolate chips",
        Instructions = "1. Mix dry ingredients\n2. Cream butter and sugars\n3. Add eggs\n4. Combine and add chips\n5. Bake at 375°F for 9-11 minutes",
        MealCategory = Recipe.MealType.Dessert,
        DietaryCategory = Recipe.Category.Dairy,
        PrepTime = 15,
        CookTime = 10,
        Servings = 24,
        ImageUrl = "Chocolate-Chip-Cookies.jpg"
    },
    new Recipe
    {
        RecipeName = "Fruit Sorbet",
        Ingredients = "4 cups frozen fruit\n1/4 cup honey\n2 tbsp lemon juice\n1/4 cup water",
        Instructions = "1. Blend all ingredients\n2. Freeze for 2 hours\n3. Scoop and serve",
        MealCategory = Recipe.MealType.Dessert,
        DietaryCategory = Recipe.Category.Vegan,
        PrepTime = 10,
        CookTime = 0,
        Servings = 4,
        ImageUrl = "Fruit-Sorbet.jpg"
    }
};

                        await context.Recipes.AddRangeAsync(recipes);
                        await context.SaveChangesAsync();
                    }
                    #endregion
                }
                #region Seed Drinks
                if (!await context.Drinks.AnyAsync())
                    {
                        var drinks = new List<Drink>
                        {
                              // Non-Alcoholic Cold Drinks
                        new Drink
                        {
                            RecipeName = "Iced Coffee",
                            Ingredients = "1 cup coffee\n1/2 cup milk\n1 tbsp sugar\nIce cubes",
                            Instructions = "1. Brew coffee\n2. Add milk and sugar\n3. Pour over ice",
                            DrinkCategory = Drink.DrinkType.NonAlcoholic,
                            IsAlcoholic = false,
                            ServingTemperature = Drink.Temperature.Cold,
                            PrepTime = 5,
                            Servings = 1
                        },
                        new Drink
                        {
                            RecipeName = "Fresh Lemonade",
                            Ingredients = "4 lemons\n4 cups water\n1/2 cup sugar\nIce cubes\nMint leaves (optional)",
                            Instructions = "1. Juice lemons\n2. Dissolve sugar in water\n3. Combine and add ice\n4. Garnish with mint",
                            DrinkCategory = Drink.DrinkType.NonAlcoholic,
                            IsAlcoholic = false,
                            ServingTemperature = Drink.Temperature.Cold,
                            PrepTime = 10,
                            Servings = 4
                        },
                        new Drink
                        {
                            RecipeName = "Strawberry Smoothie",
                            Ingredients = "1 cup strawberries\n1 banana\n1/2 cup yogurt\n1/2 cup milk\n1 tbsp honey",
                            Instructions = "1. Blend all ingredients\n2. Serve immediately",
                            DrinkCategory = Drink.DrinkType.Smoothie,
                            IsAlcoholic = false,
                            ServingTemperature = Drink.Temperature.Cold,
                            PrepTime = 5,
                            Servings = 2
                        },
                        new Drink
                        {
                            RecipeName = "Iced Matcha Latte",
                            Ingredients = "1 tsp matcha powder\n1/2 cup hot water\n1/2 cup milk\n1 tbsp sweetener\nIce cubes",
                            Instructions = "1. Whisk matcha with hot water\n2. Add milk and sweetener\n3. Pour over ice",
                            DrinkCategory = Drink.DrinkType.NonAlcoholic,
                            IsAlcoholic = false,
                            ServingTemperature = Drink.Temperature.Cold,
                            PrepTime = 5,
                            Servings = 1
                        },

                        // Non-Alcoholic Hot Drinks
                        new Drink
                        {
                            RecipeName = "Hot Chocolate",
                            Ingredients = "2 cups milk\n2 tbsp cocoa powder\n2 tbsp sugar\n1/4 tsp vanilla extract\nWhipped cream (optional)",
                            Instructions = "1. Heat milk\n2. Whisk in cocoa and sugar\n3. Add vanilla\n4. Top with whipped cream",
                            DrinkCategory = Drink.DrinkType.HotBeverage,
                            IsAlcoholic = false,
                            ServingTemperature = Drink.Temperature.Hot,
                            PrepTime = 5,
                            Servings = 2
                        },
                        new Drink
                        {
                            RecipeName = "Chai Tea Latte",
                            Ingredients = "1 cup water\n1 cup milk\n2 tea bags\n1 cinnamon stick\n4 cardamom pods\n2 cloves\n1 tsp honey",
                            Instructions = "1. Boil water with spices\n2. Add tea and steep\n3. Add milk and heat\n4. Strain and sweeten",
                            DrinkCategory = Drink.DrinkType.HotBeverage,
                            IsAlcoholic = false,
                            ServingTemperature = Drink.Temperature.Hot,
                            PrepTime = 10,
                            Servings = 2
                        },
                        new Drink
                        {
                            RecipeName = "Turmeric Golden Milk",
                            Ingredients = "2 cups milk\n1 tsp turmeric\n1/2 tsp cinnamon\n1/4 tsp ginger\n1 tbsp honey\nPinch of black pepper",
                            Instructions = "1. Heat milk with spices\n2. Whisk until frothy\n3. Sweeten to taste",
                            DrinkCategory = Drink.DrinkType.HotBeverage,
                            IsAlcoholic = false,
                            ServingTemperature = Drink.Temperature.Hot,
                            PrepTime = 5,
                            Servings = 2
                        },

                        // Alcoholic Cocktails
                        new Drink
                        {
                            RecipeName = "Classic Mojito",
                            Ingredients = "2 oz white rum\n1 oz lime juice\n2 tsp sugar\n6 mint leaves\nSoda water\nIce",
                            Instructions = "1. Muddle mint with lime and sugar\n2. Add rum and ice\n3. Top with soda\n4. Garnish with mint",
                            DrinkCategory = Drink.DrinkType.Cocktail,
                            IsAlcoholic = true,
                            ServingTemperature = Drink.Temperature.Cold,
                            PrepTime = 7,
                            Servings = 1
                        },
                        new Drink
                        {
                            RecipeName = "Aperol Spritz",
                            Ingredients = "3 oz prosecco\n2 oz Aperol\n1 oz soda water\nOrange slice\nIce",
                            Instructions = "1. Fill glass with ice\n2. Add prosecco and Aperol\n3. Top with soda\n4. Garnish with orange",
                            DrinkCategory = Drink.DrinkType.Cocktail,
                            IsAlcoholic = true,
                            ServingTemperature = Drink.Temperature.Cold,
                            PrepTime = 3,
                            Servings = 1
                        },
                        new Drink
                        {
                            RecipeName = "Whiskey Sour",
                            Ingredients = "2 oz whiskey\n3/4 oz lemon juice\n1/2 oz simple syrup\n1 egg white (optional)\nAngostura bitters",
                            Instructions = "1. Shake all ingredients with ice\n2. Strain into glass\n3. Dash bitters on top",
                            DrinkCategory = Drink.DrinkType.Cocktail,
                            IsAlcoholic = true,
                            ServingTemperature = Drink.Temperature.Cold,
                            PrepTime = 5,
                            Servings = 1
                        },

                        // Warm Alcoholic Drinks
                        new Drink
                        {
                            RecipeName = "Hot Toddy",
                            Ingredients = "1 oz whiskey\n1 tbsp honey\n1/2 oz lemon juice\n1 cup hot water\n1 cinnamon stick",
                            Instructions = "1. Combine all ingredients\n2. Stir until honey dissolves\n3. Garnish with cinnamon",
                            DrinkCategory = Drink.DrinkType.Cocktail,
                            IsAlcoholic = true,
                            ServingTemperature = Drink.Temperature.Hot,
                            PrepTime = 3,
                            Servings = 1
                        },
                        new Drink
                        {
                            RecipeName = "Mulled Wine",
                            Ingredients = "1 bottle red wine\n1 orange\n5 cloves\n2 cinnamon sticks\n1/4 cup honey\n1/4 cup brandy (optional)",
                            Instructions = "1. Combine all ingredients in pot\n2. Heat gently for 15 minutes\n3. Strain and serve warm",
                            DrinkCategory = Drink.DrinkType.Wine,
                            IsAlcoholic = true,
                            ServingTemperature = Drink.Temperature.Hot,
                            PrepTime = 5,
                            Servings = 4
                        }
                    };

                    await context.Drinks.AddRangeAsync(drinks);
                    }
                    #endregion

                    #region Seed Side Dishes
                    if (!await context.SideDishes.AnyAsync())
                    {
                        var sideDishes = new List<SideDish>
                        {
                                // Breads
            new SideDish
            {
                RecipeName = "Garlic Bread",
                Ingredients = "1 loaf French bread\n1/2 cup butter\n4 cloves garlic\n2 tbsp chopped parsley",
                Instructions = "1. Mix softened butter with minced garlic and parsley\n2. Slice bread lengthwise\n3. Spread garlic butter mixture\n4. Bake at 375°F for 10-12 minutes",
                SideDishCategory = SideDish.SideDishType.Bread,
                PrepTime = 10,
                CookTime = 12,
                Servings = 6,
                Pairings = "Pasta dishes, Italian meals, soups"
            },
            new SideDish
            {
                RecipeName = "Cornbread",
                Ingredients = "1 cup cornmeal\n1 cup flour\n1 tbsp baking powder\n1/2 tsp salt\n1 cup milk\n1 egg\n1/4 cup melted butter",
                Instructions = "1. Mix dry ingredients\n2. Add wet ingredients\n3. Pour into greased pan\n4. Bake at 400°F for 20-25 mins",
                SideDishCategory = SideDish.SideDishType.Bread,
                PrepTime = 10,
                CookTime = 25,
                Servings = 8,
                Pairings = "BBQ, chili, Southern-style meals"
            },

            // Potato Dishes
            new SideDish
            {
                RecipeName = "Garlic Mashed Potatoes",
                Ingredients = "4 large potatoes\n4 cloves garlic\n1/2 cup milk\n1/4 cup butter\nSalt and pepper to taste",
                Instructions = "1. Boil potatoes and garlic until tender\n2. Drain and mash\n3. Add warm milk and butter\n4. Season to taste",
                SideDishCategory = SideDish.SideDishType.PotatoDish,
                PrepTime = 15,
                CookTime = 20,
                Servings = 6,
                Pairings = "Roast chicken, steak, meatloaf"
            },
            new SideDish
            {
                RecipeName = "Roasted Rosemary Potatoes",
                Ingredients = "2 lbs baby potatoes\n2 tbsp olive oil\n1 tbsp fresh rosemary\n2 cloves garlic\nSalt and pepper",
                Instructions = "1. Cut potatoes in halves\n2. Toss with oil and seasonings\n3. Roast at 425°F for 30-35 mins",
                SideDishCategory = SideDish.SideDishType.PotatoDish,
                PrepTime = 10,
                CookTime = 35,
                Servings = 6,
                Pairings = "Grilled meats, roasted chicken, fish"
            },

            // Rice Dishes
            new SideDish
            {
                RecipeName = "Cilantro Lime Rice",
                Ingredients = "1 cup white rice\n2 cups water\n1 lime\n1/4 cup chopped cilantro\n1 tbsp butter",
                Instructions = "1. Cook rice normally\n2. Stir in lime zest, juice and cilantro\n3. Add butter before serving",
                SideDishCategory = SideDish.SideDishType.RiceDish,
                PrepTime = 5,
                CookTime = 20,
                Servings = 4,
                Pairings = "Mexican dishes, grilled chicken, fish tacos"
            },
            new SideDish
            {
                RecipeName = "Wild Rice Pilaf",
                Ingredients = "1 cup wild rice blend\n2 cups chicken broth\n1/2 cup mushrooms\n1/4 cup almonds\n2 tbsp parsley",
                Instructions = "1. Sauté mushrooms\n2. Add rice and toast slightly\n3. Cook in broth until tender\n4. Stir in nuts and herbs",
                SideDishCategory = SideDish.SideDishType.RiceDish,
                PrepTime = 15,
                CookTime = 45,
                Servings = 6,
                Pairings = "Roast turkey, pork chops, game meats"
            },

            // Vegetable Sides
            new SideDish
            {
                RecipeName = "Honey Glazed Carrots",
                Ingredients = "1 lb carrots\n2 tbsp honey\n1 tbsp butter\n1/2 tsp thyme",
                Instructions = "1. Slice carrots\n2. Sauté in butter\n3. Add honey and thyme\n4. Cook until glazed",
                SideDishCategory = SideDish.SideDishType.VegetableDish,
                PrepTime = 10,
                CookTime = 15,
                Servings = 4,
                Pairings = "Pork tenderloin, roasted chicken, holiday meals"
            },
            new SideDish
            {
                RecipeName = "Sautéed Green Beans",
                Ingredients = "1 lb green beans\n2 tbsp olive oil\n2 cloves garlic\n1/4 cup sliced almonds",
                Instructions = "1. Blanch beans\n2. Sauté garlic in oil\n3. Add beans and almonds\n4. Cook until crisp-tender",
                SideDishCategory = SideDish.SideDishType.VegetableDish,
                PrepTime = 10,
                CookTime = 10,
                Servings = 6,
                Pairings = "Steak, salmon, Thanksgiving dinner"
            }
                        };

                        await context.SideDishes.AddRangeAsync(sideDishes);
                    }
                    #endregion

                    await context.SaveChangesAsync();
                }
            }
        }
    }
