package com.darrenfinch.feastyweb.meal.services;

import com.darrenfinch.feastyweb.config.auth.UserIdManager;
import com.darrenfinch.feastyweb.meal.repositories.MealRepository;
import com.darrenfinch.feastyweb.mealplan.repositories.MealPlanMealRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class MealService {
    @Autowired
    private MealRepository mealRepository;

    @Autowired
    private UserIdManager userIdManager;

    @Autowired
    private MealPlanMealRepository mealPlanMealRepository;

    @Transactional
    public void deleteMeal(long mealId) throws RuntimeException {
        try {
            final var meal = mealRepository.findById(mealId);
            if (meal.isEmpty()) {
                throw new RuntimeException("No meal exists with id = " + mealId);
            }
            if (!userIdManager.userIsAccessingTheirOwnResource(meal.get().getUserId())) {
                throw new RuntimeException("The user does not have permission to delete the meal with id = " + mealId);
            }
            // Delete all meal plan meals that reference this meal so that we don't violate foreign key constraints.
            mealPlanMealRepository.deleteAllInBatchByMealId(mealId);
            mealRepository.deleteById(mealId);
        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }
}
