# VintriProject
Beer Rating via an API
Development document:
Vintri project

By Faramarz Fakhri

Dependencies:
Swagger
FluentValidation
Moq


Vintri Project is a web application that allows users to search for beers and add ratings to them. This README provides an overview of the project, its components, and how to get started.

Table of Contents
Introduction
Project Structure
Components
Getting Started
Usage


Introduction
The Vintri Project is a web-based application built using ASP.NET Web API .Net Framework 4.8. It provides two main functionalities:

Beer Search: Users can search for beers by name using the search feature. The application makes use of the Punk API to fetch beer data based on the search query.
Beer Rating: Users can rate and add comments to beers. Ratings are stored in a JSON file (database.json) for later retrieval.


Project Structure
The project is organized into two main components: BeersController and RatingController. Each of these controllers handles specific functionalities:

BeersController
The BeersController is responsible for handling beer-related operations, including:
Retrieving a list of beers based on a search query.
Combining beer data from the Punk API with user ratings stored in the database.json file.
Displaying beer details along with user ratings.

RatingController
The RatingController manages user ratings for beers. It handles the following actions:
Adding user ratings for specific beers.
Validating user inputs, including email format and rating value.
Storing user ratings in the database.json file.

Models
The project defines several models, including Beer, BeerApiResponse, Rating, and UserRatingWrapper, to represent data structures used by the controllers.


Validation
The project uses the RatingValidator class for validating user ratings, ensuring that usernames are not empty and rating values are within the range of 1 to 5. Additionally, the UsernameValidationAttribute validates the email format of email addresses.

Usage
Once the project is running, you can access it through your web browser via Swagger to run the results.

Also, a test project has been implemented via MSTest and can be run via Visual Studio.
