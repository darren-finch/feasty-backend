package com.darrenfinch.feastyweb.mealplan.models;

import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;
import lombok.Data;

import java.io.Serializable;

@Data
@Embeddable
public class MealPlanMealCombinedId implements Serializable {
    @Column(name = "meal_plan_id", nullable = false)
    private Long mealPlanId;

    @Column(name = "meal_id", nullable = false)
    private Long mealId;
}
