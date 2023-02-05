package com.darrenfinch.feastyweb.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.context.properties.ConfigurationProperties;

@lombok.Value
@ConfigurationProperties(prefix = "application")
public class ApplicationProperties {
    @Value("${CLIENT_ORIGIN_URL}")
    String clientOriginUrl;
    @Value("${AUTH0_AUDIENCE}")
    String audience;
}
