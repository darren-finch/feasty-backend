package com.darrenfinch.feastyweb.food;

import com.darrenfinch.feastyweb.config.auth.UserIdManager;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Objects;

import static org.springframework.util.ObjectUtils.isEmpty;

@RestController
public class FoodController {
    @Autowired
    private FoodRepository repository;

    @Autowired
    private UserIdManager userIdManager;

    @GetMapping("/api/foods")
    public ResponseEntity<List<Food>> getFoods(@RequestParam(value = "title", required = false) String title) {
        long userId = userIdManager.getUserId();

        if (isEmpty(title)) {
            return new ResponseEntity<>(repository.findByUserId(userId), HttpStatus.OK);
        } else {
            return new ResponseEntity<>(repository.findByTitleContainingAndUserIdIs(title, userId), HttpStatus.OK);
        }
    }

    @GetMapping("/api/foods/{foodId}")
    public ResponseEntity<Food> getFoodById(@PathVariable("foodId") long foodId) {
        final var food = repository.findById(foodId);
        if (food.isPresent() && userIdManager.userIsAccessingTheirOwnResource(food.get().getUserId())) {
            return new ResponseEntity<>(food.get(), HttpStatus.OK);
        }
        return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
    }

    @PutMapping("/api/foods")
    public ResponseEntity<Long> updateFood(@RequestBody Food food) {
        // Use the user id from the server instead of the client.
        food.setUserId(userIdManager.getUserId());
        final var idOfFoodAfterUpdate = repository.save(food).getId();
        final boolean createdNewFood = !Objects.equals(idOfFoodAfterUpdate, food.getId());
        if (createdNewFood) {
            return new ResponseEntity<>(idOfFoodAfterUpdate, HttpStatus.CREATED);
        } else {
            return new ResponseEntity<>(idOfFoodAfterUpdate, HttpStatus.OK);
        }
    }

    @DeleteMapping("/api/foods/{foodId}")
    public ResponseEntity<Void> deleteFood(@PathVariable("foodId") long foodId) {
        final var food = repository.findById(foodId);
        if (food.isPresent() && userIdManager.userIsAccessingTheirOwnResource(food.get().getUserId())) {
            repository.deleteById(foodId);
            return new ResponseEntity<>(HttpStatus.OK);
        }
        return new ResponseEntity<>(HttpStatus.NOT_FOUND);
    }
}