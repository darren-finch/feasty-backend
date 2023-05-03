# README
Hey there! This is the backend code for my feasty web application. This is always a work in progress, so feel free to watch this repo and maybe even open a pull request if you'd like!

# NOTES FOR PROJECT MAINTAINERS
Only the following 5 status codes are currently allowed to be returned from the API:
- Ok
- NotFound
- Unauthorized
- BadRequest
- InternalServerError

The error handler middleware handles the translation between exceptions and error status codes.
The only raw result that controllers are allowed to return directly is Ok.