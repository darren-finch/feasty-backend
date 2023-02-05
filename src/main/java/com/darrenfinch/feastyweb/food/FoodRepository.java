package com.darrenfinch.feastyweb.food;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.rest.core.annotation.RestResource;

public interface FoodRepository extends JpaRepository<Food, Long> {
    @RestResource(exported = false)
    Page<Food> findByUserId(long userId, Pageable pageable);

    @RestResource(exported = false)
    Page<Food> findByTitleContainingAndUserIdIs(String title, long userId, Pageable pageable);
}
