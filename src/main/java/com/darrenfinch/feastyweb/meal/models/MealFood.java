package com.darrenfinch.feastyweb.meal.models;

import com.darrenfinch.feastyweb.food.Food;
import com.fasterxml.jackson.annotation.JsonBackReference;
import jakarta.persistence.*;
import lombok.*;

@Data
@ToString(exclude = "meal")
@Entity
@Table(name = "meal_food")
public class MealFood {
    @EmbeddedId
    private MealFoodKey combinedId;

    @ManyToOne
    @MapsId("mealId")
    @JoinColumn(name = "meal_id")
    @JsonBackReference
    private Meal meal;

    @ManyToOne
    @MapsId("foodId")
    @JoinColumn(name = "food_id")
    private Food baseFood;

    @Column(name = "desired_quantity")
    private double desiredQuantity;

    public void setUserIdOfFood(Long userId) {
        baseFood.setUserId(userId);
    }
}
