Feature: Login

  @CorporateLogin
  Scenario: Corporate user logs in and logs out successfully
    Given the user logs in as "Corporate"
    When the user logs out