Feature: OnlineShopping 

Add items to cart and verify if they added to cart ,Identify lowest priced item, remove and then verify number of items in cart.

@tag1
Scenario: Add items and verify in cart

	Given I add four random items to my cart

	When I view my cart

	Then I find total four items listed in my cart

	When I search for lowest priced item 

	And I am able to remove the lowest priced item from my cart

	Then I am able to verify three items in my cart
