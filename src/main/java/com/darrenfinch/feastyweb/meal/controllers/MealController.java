package com.darrenfinch.feastyweb.meal.controllers;

import com.darrenfinch.feastyweb.config.auth.UserIdManager;
import com.darrenfinch.feastyweb.meal.models.Meal;
import com.darrenfinch.feastyweb.meal.repositories.MealRepository;
import com.darrenfinch.feastyweb.meal.services.MealService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Locale;
import java.util.Objects;

import static org.springframework.util.ObjectUtils.isEmpty;

@RestController
public class MealController {
    @Autowired
    private MealRepository mealRepository;

    @Autowired
    private UserIdManager userIdManager;

    @Autowired
    private MealService mealService;

    @GetMapping("/api/meals")
    public ResponseEntity<List<Meal>> getMeals(@RequestParam(value = "title", required = false) String title) {
        long userId = userIdManager.getUserId();
        if (isEmpty(title)) {
            return new ResponseEntity<>(mealRepository.findByUserId(userId), HttpStatus.OK);
        } else {
            return new ResponseEntity<>(mealRepository.findByTitleContainingAndUserIdIs(title.toLowerCase(Locale.ROOT), userId), HttpStatus.OK);
        }
    }

    @GetMapping("/api/meals/{mealId}")
    public ResponseEntity<Meal> getMealById(@PathVariable("mealId") long mealId) {
        final var meal = mealRepository.findById(mealId);
        if (meal.isPresent() && userIdManager.userIsAccessingTheirOwnResource(meal.get().getUserId())) {
            return new ResponseEntity<>(meal.get(), HttpStatus.OK);
        }
        return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
    }

    @PutMapping("/api/meals")
    public ResponseEntity<Long> updateMeal(@RequestBody Meal meal) {
        // Use the user id from the server instead of the client.
        meal.setUserId(userIdManager.getUserId());
        final var idOfMealAfterUpdate = mealRepository.save(meal).getId();
        final var createdNewMeal = !Objects.equals(idOfMealAfterUpdate, meal.getId());
        if (createdNewMeal) {
            return new ResponseEntity<>(idOfMealAfterUpdate, HttpStatus.CREATED);
        } else {
            return new ResponseEntity<>(idOfMealAfterUpdate, HttpStatus.OK);
        }
    }

    @DeleteMapping("/api/meals/{mealId}")
    public ResponseEntity<Void> deleteMeal(@PathVariable("mealId") long mealId) {
        try {
            mealService.deleteMeal(mealId);
            return new ResponseEntity<>(HttpStatus.OK);
        } catch (Exception exception) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        }
    }
}
