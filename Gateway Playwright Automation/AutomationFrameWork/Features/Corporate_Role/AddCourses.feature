Feature: Add Courses

  @Launching_Applications
  Scenario: Corporate adds a course and Office logs in
    Given the user logs in as "Corporate"
    When Corporate user adds a new course
    And the user logs out
    Then user login with office login