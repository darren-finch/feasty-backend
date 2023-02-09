package com.darrenfinch.feastyweb.mealplan.models;

import com.darrenfinch.feastyweb.meal.models.Meal;
import com.fasterxml.jackson.annotation.JsonBackReference;
import jakarta.persistence.*;
import lombok.*;
import org.hibernate.Hibernate;

import java.util.Objects;

@Getter
@Setter
@ToString
@RequiredArgsConstructor
@Entity
@Table(name = "meal_plan_meal")
public class MealPlanMeal {
    @EmbeddedId
    @ToString.Include
    private MealPlanMealCombinedId combinedId;

    @ManyToOne
    @MapsId("mealPlanId")
    @JoinColumn(name = "meal_plan_id")
    @JsonBackReference
    @ToString.Exclude
    private MealPlan mealPlan;

    @OneToOne
    @MapsId("mealId")
    @ToString.Include
    @JoinColumn(name ="meal_id")
    private Meal meal;

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || Hibernate.getClass(this) != Hibernate.getClass(o)) return false;
        MealPlanMeal that = (MealPlanMeal) o;
        return combinedId != null && Objects.equals(combinedId, that.combinedId);
    }

    @Override
    public int hashCode() {
        return Objects.hash(combinedId);
    }
}