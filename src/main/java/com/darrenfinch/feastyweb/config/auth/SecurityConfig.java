package com.darrenfinch.feastyweb.config.auth;

import com.darrenfinch.feastyweb.config.ApplicationProperties;
import lombok.RequiredArgsConstructor;
import org.springframework.boot.autoconfigure.security.oauth2.resource.OAuth2ResourceServerProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.oauth2.core.DelegatingOAuth2TokenValidator;
import org.springframework.security.oauth2.core.OAuth2Error;
import org.springframework.security.oauth2.core.OAuth2ErrorCodes;
import org.springframework.security.oauth2.core.OAuth2TokenValidatorResult;
import org.springframework.security.oauth2.jwt.*;
import org.springframework.security.web.SecurityFilterChain;

@Configuration
@RequiredArgsConstructor
public class SecurityConfig {
    private final AuthenticationErrorHandler authenticationErrorHandler;

    private final OAuth2ResourceServerProperties resourceServerProps;

    private final ApplicationProperties applicationProps;

    @Bean
    public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        // @formatter:off
        return http.authorizeHttpRequests()
                .requestMatchers("/api/foods**")
                    .authenticated()
                .anyRequest()
                    .permitAll()
                .and()
                    .cors()
                .and()
                    .oauth2ResourceServer()
                        .authenticationEntryPoint(authenticationErrorHandler)
                        .jwt()
                            .decoder(makeJwtDecoder())
                        .and()
                .and()
                .build();
        // @formatter:on
    }

    private JwtDecoder makeJwtDecoder() {
        final var issuer = resourceServerProps.getJwt().getIssuerUri();
        final var decoder = JwtDecoders.<NimbusJwtDecoder>fromIssuerLocation(issuer);
        final var withIssuer = JwtValidators.createDefaultWithIssuer(issuer);
        final var tokenValidator = new DelegatingOAuth2TokenValidator<>(withIssuer, this::withAudience);

        decoder.setJwtValidator(tokenValidator);
        return decoder;
    }

    private OAuth2TokenValidatorResult withAudience(final Jwt token) {
        final var audienceError = new OAuth2Error(
                OAuth2ErrorCodes.INVALID_TOKEN,
                "The token was not issued for the given audience",
                "https://datatracker.ietf.org/doc/html/rfc6750#section-3.1"
        );

        return token.getAudience().contains(applicationProps.getAudience())
                ? OAuth2TokenValidatorResult.success()
                : OAuth2TokenValidatorResult.failure(audienceError);
    }
}