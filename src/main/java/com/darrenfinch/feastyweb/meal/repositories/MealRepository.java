package com.darrenfinch.feastyweb.meal.repositories;

import com.darrenfinch.feastyweb.meal.models.Meal;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.rest.core.annotation.RestResource;

import java.util.List;

public interface MealRepository extends JpaRepository<Meal, Long> {
    @RestResource(exported = false)
    List<Meal> findByUserId(long userId);

    @RestResource(exported = false)
    List<Meal> findByTitleContainingAndUserIdIs(String title, long userId);
}
