package com.darrenfinch.feastyweb.food;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.web.bind.annotation.CrossOrigin;

@CrossOrigin("http://localhost:3000")
public interface FoodRepository extends JpaRepository<Food, Long> {
}