package com.darrenfinch.feastyweb.meal.models;

import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;
import lombok.Data;

import java.io.Serializable;

@Data
@Embeddable
public class MealFoodCombinedId implements Serializable {
    @Column(name = "meal_id", nullable = false)
    private Long mealId;

    @Column(name = "food_id", nullable = false)
    private Long foodId;
}
