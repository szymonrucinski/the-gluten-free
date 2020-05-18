# Team Core Mechanics

## Members
- Yusuf Teoman Bayoglu
- Jaroslav Dlask
- Michael Kappaurer

## Work Description
Main game aspect: products approaching players based on some randomness and logics

## Work Items
### Assets
- glutenfree products
- products with gluten
- shopping list

### Scripts
- randomly let products move to player
- products depend on shopping list
- products disappear when not bought
- interactive shopping list

# Work done

## Assets

### Glutenfree products
| Glutenfree products | Products with gluten |
|---------------------|----------------------|
| Broccoli            | bread                |
| Cabbage             | cake                 |
| Carrot              | pizza                |
| Melon               | cookie               |
| Pepper              | pastry               |
| Cheese              |                      |
| Egg                 |                      |
| Icecream            |                      |
| Orange              |                      |

All the products have been made by  **Jaroslav Dlask**

### Shopping List
The shopping list is generated every start of the game and contains random products out of the array of glutenfree products.

Whenever a product of the shopping list is bought it gets ticked on the shopping list. 

The shopping list was designed and implemented by **Michael Kappaurer**

## Scripting
The products get emitted on a conveyer belt which moves in front of the player from left to right. If a product is not bought, it disappears at the end of the conveyer belt. 

The more products on the shoppint list are bought, the faster the conveyer belt moves and the harder it gets to catch the right product.

The emitting is randomly out of all the products but the likelihood of glutenfree products is twice as high and the likelihood of products on the shopping list is even higher. 

There are two easier modes where only good products are emitted or only products from the shopping list are used. If the option for only shopping list products is used, even the already bought once don't get emitted. 
This can be selected in the "foodemitter".

To test the shopping list as well as the behaviour of the products, a "buying" was implemented by the "Core Development" team. It simply moves the product into the basket when clicked at the right time. This is improved by the team "Product Scanning".

Our goal is it to buy all products on the shopping list befor the time runs out. The game is over eighter when all products are bought or when the time runs out. 

Base Point System:
For every product bought you get points. If the product is on the shopping list it is automaticly a good product and you are avarded 2 points. If a good product is bought that is not on the shopping list, you get 1 point and if a bad product is bought, 1 point is subtracted.

All the scripts were implemented by **Michael Kappaurer**

# How to start
1. set the time scale to one
2. deactivate the GameEnvironment it will be loaded automatecly if start button is clickt in game
3. set the difficulty in the FoodEmitter
    - nothing is acctivated - all products get emitted
    - "only good Products" - only the good products get emitted
    - "only Products from Shopping List" - only the products appering on the shopping list get emitted
4. Start the game







