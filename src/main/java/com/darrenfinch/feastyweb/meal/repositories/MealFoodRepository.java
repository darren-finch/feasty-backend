package com.darrenfinch.feastyweb.meal.repositories;

import com.darrenfinch.feastyweb.meal.models.MealFood;
import com.darrenfinch.feastyweb.meal.models.MealFoodCombinedId;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.rest.core.annotation.RestResource;

public interface MealFoodRepository extends JpaRepository<MealFood, MealFoodCombinedId> {
    @RestResource(exported = false)
    void deleteAllInBatchByCombinedIdFoodId(Long foodId);
}
