package com.darrenfinch.feastyweb.meal.repositories;

import com.darrenfinch.feastyweb.meal.models.MealFood;
import org.springframework.data.jpa.repository.JpaRepository;

public interface MealFoodRepository extends JpaRepository<MealFood, Long> {
}
