# Guide

The repository layer for this application is meant to be a very thin layer around the database to allow for easier unit test mocking. As much as possible, we want to avoid putting business logic, exception handling, or any other kind of logic in these repositories. That functionality is meant for the service layer. The general rule of thumb is one database operation per repository function.

However, there is one exception to this rule: authorization. At the moment, I have not found an elegant way to make sure that the user can only access his/her own entities.
This is a cross-cutting concern across our entire application, so it would be good to hide that extra auth check in the repository layer so that services don't have to be concerned with this logic.
