package com.darrenfinch.feastyweb.mealplan.controllers;

import com.darrenfinch.feastyweb.config.auth.UserIdManager;
import com.darrenfinch.feastyweb.mealplan.models.MealPlan;
import com.darrenfinch.feastyweb.mealplan.models.MealPlanMetaData;
import com.darrenfinch.feastyweb.mealplan.repositories.MealPlanMealRepository;
import com.darrenfinch.feastyweb.mealplan.repositories.MealPlanRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Objects;

@RestController
public class MealPlanController {
    @Autowired
    private MealPlanRepository mealPlanRepository;

    @Autowired
    private UserIdManager userIdManager;

    @Autowired
    private MealPlanMealRepository mealPlanMealsRepository;

    // For now, just returns the metadata associated with each meal, none of the child objects are fetched.
    @GetMapping("/api/mealplans")
    public ResponseEntity<List<MealPlanMetaData>> getMealPlanMetaData() {
        long userId = userIdManager.getUserId();
        return new ResponseEntity<>(mealPlanRepository.findByUserId(userId), HttpStatus.OK);
    }

    @GetMapping("/api/mealplans/{mealPlanId}")
    public ResponseEntity<MealPlan> getMealPlanById(@PathVariable("mealPlanId") long mealPlanId) {
        final var mealPlan = mealPlanRepository.findById(mealPlanId);
        if (mealPlan.isPresent() && userIdManager.userIsAccessingTheirOwnResource(mealPlan.get().getUserId())) {
            return new ResponseEntity<>(mealPlan.get(), HttpStatus.OK);
        }
        return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
    }

    @PutMapping("/api/mealplans")
    public ResponseEntity<Long> updateMealPlan(@RequestBody MealPlan mealPlan) {
        // Use the user id from the server instead of the client.
        mealPlan.setUserId(userIdManager.getUserId());

        // This method does NOT save the meal plan meals from the passed-in meal plan, but because Hibernate is a pain,
        // we will need to set them manually.
        try {
            final var mealPlanFromDB = mealPlanRepository.findById(mealPlan.getId());
            if (mealPlanFromDB.isPresent()) {
                final var mealPlanMealsFromDB = mealPlanMealsRepository.findAllByMealPlanId(mealPlanFromDB.get().getId());
                mealPlan.setMealPlanMeals(mealPlanMealsFromDB);
            }

            final var idOfMealAfterUpdate = mealPlanRepository.save(mealPlan).getId();
            final var createdNewMealPlan = !Objects.equals(idOfMealAfterUpdate, mealPlan.getId());
            if (createdNewMealPlan) {
                return new ResponseEntity<>(idOfMealAfterUpdate, HttpStatus.CREATED);
            } else {
                return new ResponseEntity<>(idOfMealAfterUpdate, HttpStatus.OK);
            }
        } catch (Exception exception) {
            exception.printStackTrace();
            return new ResponseEntity<>((long) -1, HttpStatus.BAD_REQUEST);
        }
    }

    @DeleteMapping("/api/mealplans/{mealPlanId}")
    public ResponseEntity<Void> deleteMealPlan(@PathVariable("mealPlanId") long mealPlanId) {
        final var mealPlan = mealPlanRepository.findById(mealPlanId);
        if (mealPlan.isPresent() && userIdManager.userIsAccessingTheirOwnResource(mealPlan.get().getUserId())) {
            mealPlanRepository.deleteById(mealPlanId);
            return new ResponseEntity<>(HttpStatus.OK);
        }
        return new ResponseEntity<>(HttpStatus.NOT_FOUND);
    }
}
