package com.darrenfinch.feastyweb.mealplan.models;

import com.fasterxml.jackson.annotation.JsonManagedReference;
import jakarta.persistence.*;
import lombok.*;
import org.hibernate.Hibernate;

import java.util.List;
import java.util.Objects;

@Getter
@Setter
@ToString
@RequiredArgsConstructor
@Entity
@Table(name = "meal_plan")
public class MealPlan {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    @Column(name="user_id")
    private Long userId;

    @Column(name = "title")
    private String title;

    @Column(name = "required_calories")
    private int requiredCalories;

    @Column(name = "required_fats")
    private int requiredFats;

    @Column(name = "required_carbs")
    private int requiredCarbs;

    @Column(name = "required_proteins")
    private int requiredProteins;

    @JsonManagedReference
    @OneToMany(fetch = FetchType.LAZY, mappedBy = "mealPlan", cascade = CascadeType.ALL, orphanRemoval = true)
    @ToString.Exclude
    private List<MealPlanMeal> mealPlanMeals;

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || Hibernate.getClass(this) != Hibernate.getClass(o)) return false;
        MealPlan mealPlan = (MealPlan) o;
        return id != null && Objects.equals(id, mealPlan.id);
    }

    @Override
    public int hashCode() {
        return getClass().hashCode();
    }
}
