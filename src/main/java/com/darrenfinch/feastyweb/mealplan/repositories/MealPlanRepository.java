package com.darrenfinch.feastyweb.mealplan.repositories;

import com.darrenfinch.feastyweb.mealplan.models.MealPlan;
import com.darrenfinch.feastyweb.mealplan.models.MealPlanMetaData;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.rest.core.annotation.RestResource;

import java.util.List;

public interface MealPlanRepository extends JpaRepository<MealPlan, Long> {
    @RestResource(exported = false)
    List<MealPlanMetaData> findByUserId(Long userId);
}
