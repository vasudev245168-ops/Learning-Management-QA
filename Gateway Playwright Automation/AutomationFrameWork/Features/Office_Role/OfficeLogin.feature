Feature: OfficeLogin

A short summary of the feature

Feature: Login

  @OfficeLogin
  Scenario: Office user logs in and logs out successfully
    Given the user logs in as "Office"
    When the user logs out