﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using new_backend.Data;

#nullable disable

namespace new_backend.Migrations
{
    [DbContext(typeof(FeastyDbContext))]
    [Migration("20230507001738_ChangeFoodIdToBeIdentityType")]
    partial class ChangeFoodIdToBeIdentityType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("new_backend.Models.Food", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<int>("Calories")
                        .HasColumnType("integer");

                    b.Property<int>("Carbs")
                        .HasColumnType("integer");

                    b.Property<int>("Fats")
                        .HasColumnType("integer");

                    b.Property<int>("Proteins")
                        .HasColumnType("integer");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("new_backend.Models.Meal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("new_backend.Models.MealFood", b =>
                {
                    b.Property<long>("MealId")
                        .HasColumnType("bigint");

                    b.Property<long>("FoodId")
                        .HasColumnType("bigint");

                    b.Property<double>("DesiredQuantity")
                        .HasColumnType("double precision");

                    b.HasKey("MealId", "FoodId");

                    b.HasIndex("FoodId");

                    b.ToTable("MealFoods");
                });

            modelBuilder.Entity("new_backend.Models.MealPlan", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("RequiredCalories")
                        .HasColumnType("integer");

                    b.Property<int>("RequiredCarbs")
                        .HasColumnType("integer");

                    b.Property<int>("RequiredFats")
                        .HasColumnType("integer");

                    b.Property<int>("RequiredProteins")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("MealPlans");
                });

            modelBuilder.Entity("new_backend.Models.MealPlanMeal", b =>
                {
                    b.Property<long>("MealId")
                        .HasColumnType("bigint");

                    b.Property<long>("MealPlanId")
                        .HasColumnType("bigint");

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