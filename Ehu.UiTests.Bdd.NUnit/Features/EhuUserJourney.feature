@ui @e2e @journey
Feature: EHU website user journey

  Scenario: Explore key sections of the EHU website
    Given the user opens the EHU home page
    And accepts cookies if the banner is shown
    When the user navigates to the About page
    Then the About page should be opened
    And the About page header should contain "About"

    When the user opens the EHU home page
    And searches for "study programs"
    Then the user should see content related to study programs

    When the user switches the site language to Lithuanian
    Then the Lithuanian version of the site should be opened
    And the user should see Lithuanian content on the page

    When the user opens the contacts page
    Then the contacts page should be opened
    And the page should contain contact text "consult@ehu.lt"
    And the page should contain contact text "office@ehu.lt"
    And the page should contain contact text "Facebook"