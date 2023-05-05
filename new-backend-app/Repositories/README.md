# Guide

The repository layer for this application is meant to be a very thin layer around the database to allow for easier unit test mocking. As much as possible, we want to avoid putting business logic, exception handling, or any other kind of logic in these repositories. That functionality is meant for the service layer. The general rule of thumb is one database operation per repository function.

However, there is one exception to this rule: filtering lists. I don't want to be filtering lists based on a query or the current user in my service tier. That kind of work is best left up to the database as it is more efficient.
