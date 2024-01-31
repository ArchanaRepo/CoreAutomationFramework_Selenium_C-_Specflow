Feature: FlipKartLoginFeature

Test Flipkart login Feature

 Scenario: Valid Login
    Given I am on the Flipkart login page
    When I enter valid username "valid_username" and password "valid_password"
    And I click the login button
    Then I should be logged in successfully