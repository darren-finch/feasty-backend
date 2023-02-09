package com.darrenfinch.feastyweb.mealplan.controllers;

import com.darrenfinch.feastyweb.config.auth.UserIdManager;
import com.darrenfinch.feastyweb.mealplan.models.MealPlanMeal;
import com.darrenfinch.feastyweb.mealplan.models.MealPlanMealCombinedId;
import com.darrenfinch.feastyweb.mealplan.services.MealPlanMealService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class MealPlanMealController {
    @Autowired
    private UserIdManager userIdManager;

    @Autowired
    private MealPlanMealService mealPlanMealService;

    @PutMapping("/api/mealplanmeals")
    public ResponseEntity<MealPlanMealCombinedId> saveMealPlanMeal(@RequestBody MealPlanMeal mealPlanMeal) {
        try {
            final var response = mealPlanMealService.updateMealPlanMeal(mealPlanMeal);
            return new ResponseEntity<>(response, HttpStatus.OK);
        } catch (RuntimeException exception) {
            exception.printStackTrace();
            return new ResponseEntity<>(null, HttpStatus.BAD_REQUEST);
        }
    }

    @DeleteMapping("/api/mealplanmeals")
    public ResponseEntity<Void> deleteMealPlanMeal(@RequestBody MealPlanMealCombinedId combinedId) {
        try {
            mealPlanMealService.deleteMealPlanMeal(combinedId);
            return new ResponseEntity<>(null, HttpStatus.OK);
        } catch (RuntimeException exception) {
            exception.printStackTrace();
            return new ResponseEntity<>(null, HttpStatus.BAD_REQUEST);
        }
    }
}
