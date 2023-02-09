package com.darrenfinch.feastyweb.food;

import com.darrenfinch.feastyweb.config.auth.UserIdManager;
import com.darrenfinch.feastyweb.meal.repositories.MealFoodRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class FoodService {
    @Autowired
    private FoodRepository repository;

    @Autowired
    private UserIdManager userIdManager;

    @Autowired
    private MealFoodRepository mealFoodRepository;

    @Transactional
    public void deleteFood(long foodId) throws RuntimeException {
        try {
            final var food = repository.findById(foodId);
            if (food.isEmpty()) {
                throw new RuntimeException("No food exists with an id = " + foodId);
            }
            if (!userIdManager.userIsAccessingTheirOwnResource(food.get().getUserId())) {
                throw new RuntimeException("The user does not have permission to delete the food with id = " + foodId);
            }
            // Find all meal foods that reference this food and remove the reference from them so that we don't violate foreign key constraints.
            mealFoodRepository.deleteAllInBatchByCombinedIdFoodId(foodId);
            repository.deleteById(foodId);
        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }
}
