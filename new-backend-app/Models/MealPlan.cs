namespace new_backend.Models;

public class MealPlan
{
    public virtual long Id { get; set; }
    public virtual long UserId { get; set; }
    public virtual string Title { get; set; }
    public virtual int RequiredCalories { get; set; }
    public virtual int RequiredFats { get; set; }
    public virtual int RequiredCarbs { get; set; }
    public virtual int RequiredProteins { get; set; }
    public virtual List<MealPlanMeal> MealPlanMeals { get; set; }
}