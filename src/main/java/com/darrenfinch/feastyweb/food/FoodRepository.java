package com.darrenfinch.feastyweb.food;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.rest.core.annotation.RestResource;

import java.util.List;

public interface FoodRepository extends JpaRepository<Food, Long> {
    @RestResource(exported = false)
    List<Food> findByUserId(long userId);

    @RestResource(exported = false)
    List<Food> findByTitleContainingAndUserIdIs(String title, long userId);
}
