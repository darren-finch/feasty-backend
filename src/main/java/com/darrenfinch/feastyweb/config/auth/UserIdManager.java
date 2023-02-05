package com.darrenfinch.feastyweb.config.auth;

import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;

@Component
public class UserIdManager {
    public boolean userIsAccessingTheirOwnResource(long resourceUserId) {
        return getUserId() == resourceUserId;
    }

    public long getUserId() {
        String authName = SecurityContextHolder.getContext().getAuthentication().getName();
        return getUserIdNumberFromAuthName(authName);
    }

    private long getUserIdNumberFromAuthName(String authName) {
        // Doing this manually because I suck at regex
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < authName.length(); i++) {
            if (Character.isDigit(authName.charAt(i))) {
                sb.append(authName.charAt(i));
            }
        }

        return Integer.parseInt(sb.substring(0, Math.min(10, sb.length())));
    }
}
