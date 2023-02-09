package com.darrenfinch.feastyweb.meal.models;

import com.darrenfinch.feastyweb.food.Food;
import com.fasterxml.jackson.annotation.JsonBackReference;
import jakarta.persistence.*;
import lombok.*;
import org.hibernate.Hibernate;

import java.util.Objects;

@Getter
@Setter
@RequiredArgsConstructor
@ToString(exclude = "meal")
@Entity
@Table(name = "meal_food")
public class MealFood {
    @EmbeddedId
    private MealFoodCombinedId combinedId;

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

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || Hibernate.getClass(this) != Hibernate.getClass(o)) return false;
        MealFood mealFood = (MealFood) o;
        return combinedId != null && Objects.equals(combinedId, mealFood.combinedId);
    }

    @Override
    public int hashCode() {
        return Objects.hash(combinedId);
    }
}
