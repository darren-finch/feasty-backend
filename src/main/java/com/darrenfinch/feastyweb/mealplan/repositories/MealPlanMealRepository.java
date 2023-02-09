package com.darrenfinch.feastyweb.mealplan.repositories;

import com.darrenfinch.feastyweb.mealplan.models.MealPlanMeal;
import com.darrenfinch.feastyweb.mealplan.models.MealPlanMealCombinedId;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.rest.core.annotation.RestResource;

import java.util.List;

public interface MealPlanMealRepository extends JpaRepository<MealPlanMeal, MealPlanMealCombinedId> {
    @RestResource(exported = false)
    List<MealPlanMeal> findAllByMealPlanId(Long mealPlanId);

    @RestResource(exported = false)
    void deleteAllInBatchByMealId(Long mealId);
}
