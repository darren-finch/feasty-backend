package com.darrenfinch.feastyweb.meal.repositories;

import com.darrenfinch.feastyweb.meal.models.Meal;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.rest.core.annotation.RestResource;

public interface MealRepository extends JpaRepository<Meal, Long> {
    @RestResource(exported = false)
    Page<Meal> findByUserId(long userId, Pageable pageable);

    @RestResource(exported = false)
    Page<Meal> findByTitleContainingAndUserIdIs(String title, long userId, Pageable pageable);
}
