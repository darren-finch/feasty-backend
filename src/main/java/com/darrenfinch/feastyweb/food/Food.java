package com.darrenfinch.feastyweb.food;

import jakarta.persistence.*;
import lombok.*;
import org.hibernate.Hibernate;

import java.util.Objects;

@Getter
@Setter
@ToString
@RequiredArgsConstructor
@Entity
@Table(name = "food")
public class Food {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    @Column(name="userId")
    private Long userId;

    @Column(name = "title")
    private String title;

    @Column(name = "quantity")
    private double quantity;

    @Column(name = "unit")
    private String unit;

    @Column(name = "calories")
    private int calories;

    @Column(name = "fats")
    private int fats;

    @Column(name = "carbs")
    private int carbs;

    @Column(name = "proteins")
    private int proteins;

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || Hibernate.getClass(this) != Hibernate.getClass(o)) return false;
        Food food = (Food) o;
        return id != null && Objects.equals(id, food.id);
    }

    @Override
    public int hashCode() {
        return getClass().hashCode();
    }
}
