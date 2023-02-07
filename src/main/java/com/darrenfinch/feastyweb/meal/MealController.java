package com.darrenfinch.feastyweb.meal;

import com.darrenfinch.feastyweb.config.auth.UserIdManager;
import com.darrenfinch.feastyweb.meal.models.Meal;
import com.darrenfinch.feastyweb.meal.repositories.MealFoodRepository;
import com.darrenfinch.feastyweb.meal.repositories.MealRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Objects;

import static org.springframework.util.ObjectUtils.isEmpty;

@RestController
public class MealController {
    @Autowired
    private MealRepository mealRepository;

    @Autowired
    private MealFoodRepository mealFoodRepository;

    @Autowired
    private UserIdManager userIdManager;

    @GetMapping("/api/meals")
    public ResponseEntity<Page<Meal>> getMeals(@RequestParam(value = "title", required = false) String title, @RequestParam(value = "pageSize", required = false, defaultValue = "20") int pageSize) {
        if (pageSize < 1) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        }

        long userId = userIdManager.getUserId();
        if (isEmpty(title)) {
            return new ResponseEntity<>(mealRepository.findByUserId(userId, Pageable.ofSize(pageSize)), HttpStatus.OK);
        } else {
            return new ResponseEntity<>(mealRepository.findByTitleContainingAndUserIdIs(title, userId, Pageable.ofSize(pageSize)), HttpStatus.OK);
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
        final var meal = mealRepository.findById(mealId);
        if (meal.isPresent() && userIdManager.userIsAccessingTheirOwnResource(meal.get().getUserId())) {
            mealRepository.deleteById(mealId);
            return new ResponseEntity<>(HttpStatus.OK);
        }
        return new ResponseEntity<>(HttpStatus.NOT_FOUND);
    }
}
