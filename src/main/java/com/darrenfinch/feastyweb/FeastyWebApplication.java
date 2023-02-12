package com.darrenfinch.feastyweb;

import com.darrenfinch.feastyweb.config.ApplicationProperties;
import io.github.cdimascio.dotenv.Dotenv;
import lombok.extern.log4j.Log4j2;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.properties.ConfigurationPropertiesScan;
import org.springframework.boot.context.properties.EnableConfigurationProperties;

import static java.util.Arrays.stream;

@Log4j2
@SpringBootApplication
@EnableConfigurationProperties(ApplicationProperties.class)
@ConfigurationPropertiesScan
public class FeastyWebApplication {
	enum DotEnv {
		PORT,
		CLIENT_ORIGIN_URL,
		AUTH0_DOMAIN,
		AUTH0_AUDIENCE,
		DATABASE_URL,
		DATABASE_USERNAME,
		DATABASE_PASSWORD
	}

	public static void main(String[] args) {
		dotEnvSafeCheck();

		SpringApplication.run(FeastyWebApplication.class, args);
	}

	private static void dotEnvSafeCheck() {
		final var dotenv = Dotenv.configure()
				.ignoreIfMissing()
				.load();

		stream(DotEnv.values())
				.map(DotEnv::name)
				.filter(varName -> dotenv.get(varName, "").isEmpty())
				.findFirst()
				.ifPresent(varName -> {
					log.error("[Fatal] Missing or empty environment variable: {}", varName);

					System.exit(1);
				});
	}
}
