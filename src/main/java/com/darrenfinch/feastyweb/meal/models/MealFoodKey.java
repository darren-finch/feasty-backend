package com.darrenfinch.feastyweb.meal.models;

import jakarta.persistence.*;
import lombok.*;

import java.io.Serializable;

@Data
@Embeddable
public class MealFoodKey implements Serializable {
    @Column(name = "meal_id", nullable = false)
    private Long mealId;

    @Column(name = "food_id", nullable = false)
    private Long foodId;
}
