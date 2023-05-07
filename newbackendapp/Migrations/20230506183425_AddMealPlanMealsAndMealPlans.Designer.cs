﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using new_backend.Data;

#nullable disable

namespace new_backend.Migrations
{
    [DbContext(typeof(FeastyDbContext))]
    [Migration("20230506183425_AddMealPlanMealsAndMealPlans")]
    partial class AddMealPlanMealsAndMealPlans
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("new_backend.Models.Food", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Calories")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Carbs")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Fats")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Proteins")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Quantity")
                        .HasColumnType("REAL");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("new_backend.Models.Meal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("new_backend.Models.MealFood", b =>
                {
                    b.Property<long>("MealId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("FoodId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("DesiredQuantity")
                        .HasColumnType("REAL");

                    b.HasKey("MealId", "FoodId");

                    b.HasIndex("FoodId");

                    b.ToTable("MealFoods");
                });

            modelBuilder.Entity("new_backend.Models.MealPlan", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequiredCalories")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequiredCarbs")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequiredFats")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequiredProteins")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("MealPlans");
                });

            modelBuilder.Entity("new_backend.Models.MealPlanMeal", b =>
                {
                    b.Property<long>("MealId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MealPlanId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MealId", "MealPlanId");

                    b.HasIndex("MealPlanId");

                    b.ToTable("MealPlanMeals");
                });

            modelBuilder.Entity("new_backend.Models.MealFood", b =>
                {
                    b.HasOne("new_backend.Models.Food", "BaseFood")
                        .WithMany()
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("new_backend.Models.Meal", "Meal")
                        .WithMany("MealFoods")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseFood");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("new_backend.Models.MealPlanMeal", b =>
                {
                    b.HasOne("new_backend.Models.Meal", "Meal")
                        .WithMany()
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("new_backend.Models.MealPlan", "MealPlan")
                        .WithMany("MealPlanMeals")
                        .HasForeignKey("MealPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meal");

                    b.Navigation("MealPlan");
                });

            modelBuilder.Entity("new_backend.Models.Meal", b =>
                {
                    b.Navigation("MealFoods");
                });

            modelBuilder.Entity("new_backend.Models.MealPlan", b =>
                {
                    b.Navigation("MealPlanMeals");
                });
#pragma warning restore 612, 618
        }
    }
}
