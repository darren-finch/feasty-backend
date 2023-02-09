package com.darrenfinch.feastyweb.mealplan.models;

public interface MealPlanMetaData {
    Long getId();

    Long getUserId();

    String getTitle();

    int getRequiredCalories();

    int getRequiredFats();

    int getRequiredCarbs();

    int getRequiredProteins();
}
