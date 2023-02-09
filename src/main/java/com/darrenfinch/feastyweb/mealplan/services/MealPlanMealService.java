package com.darrenfinch.feastyweb.mealplan.services;

import com.darrenfinch.feastyweb.config.auth.UserIdManager;
import com.darrenfinch.feastyweb.meal.repositories.MealRepository;
import com.darrenfinch.feastyweb.mealplan.models.MealPlanMeal;
import com.darrenfinch.feastyweb.mealplan.models.MealPlanMealCombinedId;
import com.darrenfinch.feastyweb.mealplan.repositories.MealPlanMealRepository;
import com.darrenfinch.feastyweb.mealplan.repositories.MealPlanRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class MealPlanMealService {
    @Autowired
    private MealRepository mealRepository;

    @Autowired
    private MealPlanMealRepository mealPlanMealRepository;

    @Autowired
    private MealPlanRepository mealPlanRepository;

    @Autowired
    private UserIdManager userIdManager;

    @Transactional
    public MealPlanMealCombinedId updateMealPlanMeal(MealPlanMeal mealPlanMeal) throws RuntimeException {
        long userId = userIdManager.getUserId();
        final var referencedMealFromDB = mealRepository.findById(mealPlanMeal.getCombinedId().getMealId());
        final var referencedMealPlanFromDB = mealPlanRepository.findById(mealPlanMeal.getCombinedId().getMealPlanId());
        if (referencedMealFromDB.isEmpty()) {
            throw new RuntimeException("No meal exists with the id of " + mealPlanMeal.getCombinedId().getMealId());
        }
        if (referencedMealPlanFromDB.isEmpty()) {
            throw new RuntimeException("No meal plan exists with the id of " + mealPlanMeal.getCombinedId().getMealPlanId());
        }
        if (referencedMealFromDB.get().getUserId() != userId || referencedMealPlanFromDB.get().getUserId() != userId) {
            throw new RuntimeException("The referenced meal or meal plan does not belong to this user.");
        }
        if (referencedMealPlanFromDB.get().getMealPlanMeals().stream().anyMatch((curMealPlanMealItem) -> curMealPlanMealItem.getCombinedId() == mealPlanMeal.getCombinedId())) {
            throw new RuntimeException("Cannot add more than one of the same meal to the same meal plan");
        }

        mealPlanMeal.setMeal(referencedMealFromDB.get());
        mealPlanMeal.setMealPlan(referencedMealPlanFromDB.get());
        return mealPlanMealRepository.save(mealPlanMeal).getCombinedId();
    }

    @Transactional
    public void deleteMealPlanMeal(MealPlanMealCombinedId combinedId) throws RuntimeException {
        long userId = userIdManager.getUserId();
        final var referencedMealPlanFromDB = mealPlanRepository.findById(userId);
        if (referencedMealPlanFromDB.isPresent() && referencedMealPlanFromDB.get().getUserId() != userId) {
            throw new RuntimeException("The meal plan that this meal plan meal references does not belong to the user, so it cannot be deleted.");
        }

        mealPlanMealRepository.deleteById(combinedId);
    }
}
