# RecipeSharingPlatform Lite-Version
Functionality

The functionality of the RecipeSharingPlatform is a very simple application.

Users

Guests can Register, Login and view the Home Page, Index Page and Details Page.
Logged-in users can publish recipes and edit recipes they have created.
Users (guest-users + logged-in-users) can view all published recipes, including those by other users, on the Home Page (/Recipe/Index).
If the user is the author of the recipe
•	They can't see the [Favorites] button
•	They can see the [Details] button
If the user is not the author of the recipe, they can only add the recipe to their collection [Favorites].\
Recipe

Recipe can be added by logged-in-users. All added recipes are visualized on the Home Page (/Recipe/Index) with some of their associated information. 
Recipes are visualized on the Index Page (/Recipe/Index) with one or two buttons:
•	If the user IS NOT the author of the recipe – [Details][Favorites]
•	If the user IS the author of the recipe – [Details]
The [Favorites] button adds the recipe to the user's collection of favorite recipes (Favorites) unless it is already added.
The [Details] button displays a new page with the full information for the selected recipe. The actions and buttons displayed on this page will vary depending on the user's identity and role:
•	Guest (Not Logged In):
The user can see the full details of the recipe but will not be able to add to favorites, edit, or delete the recipe. No action buttons ([Favorites], [Edit], [Delete]) will be shown.
•	Logged-in User (Not the Author):
The user will see the full details of the recipe and will have access to the [Favorites] button, allowing them to add the recipe to their collection of favorite recipes (if it has not already been added). The [Edit] and [Delete] buttons will not be displayed as they are not the author of the recipe.
•	Author of the Recipe:
The author will have full access to the recipe's details and will be able to see both the [Edit] and [Delete] buttons. However, the [Favorites] button will not be displayed since the author cannot add their own recipe.
The [Edit] button displays a new page with a form, filled in with all of the info for the selected recipe. Users can change this info and save it.

Users have a Favorites page where only the recipes they have selected as favorites are displayed.
•	The [Remove] button removes the selected recipe from the user's collection of favorite recipes.
