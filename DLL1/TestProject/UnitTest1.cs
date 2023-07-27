using NUnit.Framework;
using System;
using System.Reflection;
using System.Collections.Generic;
using MovieApp;


[TestFixture]
public class MovieTests
{
    
    [Test]
    public void MovieId_ShouldBeInt()
    {
        // Arrange
        Type movieType = typeof(Movie);

        // Act
        PropertyInfo idProperty = movieType.GetProperty("Id");

        // Assert
        Assert.NotNull(idProperty);
        Assert.AreEqual(typeof(int), idProperty.PropertyType);
    }

    [Test]
    public void MovieTitle_ShouldBeString()
    {
        // Arrange
        Type movieType = typeof(Movie);

        // Act
        PropertyInfo titleProperty = movieType.GetProperty("Title");

        // Assert
        Assert.NotNull(titleProperty);
        Assert.AreEqual(typeof(string), titleProperty.PropertyType);
    }

    [Test]
    public void MovieGenre_ShouldBeString()
    {
        // Arrange
        Type movieType = typeof(Movie);

        // Act
        PropertyInfo genreProperty = movieType.GetProperty("Genre");

        // Assert
        Assert.NotNull(genreProperty);
        Assert.AreEqual(typeof(string), genreProperty.PropertyType);
    }

    [Test]
    public void MovieYear_ShouldBeInt()
    {
        // Arrange
        Type movieType = typeof(Movie);

        // Act
        PropertyInfo yearProperty = movieType.GetProperty("Year");

        // Assert
        Assert.NotNull(yearProperty);
        Assert.AreEqual(typeof(int), yearProperty.PropertyType);
    }

    [Test]
    public void MovieClassExists()
    {
        // Arrange
        // Type movieType = typeof(Movie);
            string className = "Movie"; // Fully qualified type name without assembly name

            // Load the assembly from the DLL file explicitly
            Assembly assembly = Assembly.LoadFrom("MovieLibrary.dll");

            // Get the type using the full type name and assembly
            Type movieType = assembly.GetType(className);

        // Assert
        Assert.IsNotNull(movieType);
    }

    [Test]
    public void Movie_Properties_GetSetCorrectly_Id()
    {
        // Arrange
        var movie = new Movie();

        Type movieType = typeof(Movie);
        PropertyInfo idProperty = movieType.GetProperty("Id");

        // Act
        idProperty.SetValue(movie, 1);

        // Assert
        Assert.AreEqual(1, idProperty.GetValue(movie));
    }

    [Test]
    public void Movie_Properties_GetSetCorrectly_Title()
    {
        // Arrange
        var movie = new Movie();

        Type movieType = typeof(Movie);
        PropertyInfo titleProperty = movieType.GetProperty("Title");

        // Act
        titleProperty.SetValue(movie, "Test Movie");

        // Assert
        Assert.AreEqual("Test Movie", titleProperty.GetValue(movie));
    }

    [Test]
    public void Movie_Properties_GetSetCorrectly_Genre()
    {
        // Arrange
        var movie = new Movie();

        Type movieType = typeof(Movie);
        PropertyInfo genreProperty = movieType.GetProperty("Genre");

        // Act
        genreProperty.SetValue(movie, "Action");

        // Assert
        Assert.AreEqual("Action", genreProperty.GetValue(movie));
    }

    [Test]
    public void Movie_Properties_GetSetCorrectly_Year()
    {
        // Arrange
        var movie = new Movie();

        Type movieType = typeof(Movie);
        PropertyInfo yearProperty = movieType.GetProperty("Year");

        // Act
        yearProperty.SetValue(movie, 2022);

        // Assert
        Assert.AreEqual(2022, yearProperty.GetValue(movie));
    }

    [Test]
    public void AddMovie_ValidMovie_SuccessfullyAdded()
    {
        
            string className = "BusinessLayer"; // Fully qualified type name without assembly name

            // Load the assembly from the DLL file explicitly
            Assembly assembly = Assembly.LoadFrom("MovieLibrary.dll");
 try{
        Type businessLayerType =assembly.GetType(className);
       Console.WriteLine(businessLayerType);
        if(businessLayerType != null){
        var businessLayer = Activator.CreateInstance(businessLayerType);

        var movie = new Movie
        {
            Title = "Test Movie",
            Genre = "Action",
            Year = 2022
        };

        // Act
        MethodInfo addMovieMethod = businessLayerType.GetMethod("AddMovie");
        addMovieMethod.Invoke(businessLayer, new object[] { movie });

        // Assert
        MethodInfo getAllMoviesMethod = businessLayerType.GetMethod("GetAllMovies");
        var allMovies = (List<Movie>)getAllMoviesMethod.Invoke(businessLayer, null);

        Assert.Contains(movie, allMovies);}}
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: ");
            Assert.Fail();
        }

    }

    [Test]
        public void AddMovie_InvalidMovie_ThrowsArgumentException()
        {
            // Ensure you have the correct namespace and assembly name for BusinessLayer
            string className = "BusinessLayer"; // Fully qualified type name including assembly name
            
            // Load the assembly from the DLL file explicitly
            Assembly assembly = Assembly.LoadFrom("MovieLibrary.dll");

            // Get the type using the full type name and assembly
            Type businessLayerType = assembly.GetType(className);

            // Check if the class type is null before creating an instance
            if (businessLayerType == null)
            {
                Assert.Fail($"The class '{className}' does not exist.");
            }

            // Create an instance of the BusinessLayer class
            object businessLayer = Activator.CreateInstance(businessLayerType);

            // Create an instance of the Movie class without required data
            var movie = new Movie(); // Empty movie with no required data

            // Act & Assert
            MethodInfo addMovieMethod = businessLayerType.GetMethod("AddMovie");
            try
            {
                addMovieMethod.Invoke(businessLayer, new object[] { movie });
                Assert.Fail("Expected an ArgumentException to be thrown, but no exception was thrown.");
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsInstanceOf<ArgumentException>(ex.InnerException);
                Assert.AreEqual("Title is required.", ex.InnerException.Message);
            }
        }

    [Test]
        public void EditMovie_ValidMovie_SuccessfullyEdited()
        {
            string className = "BusinessLayer"; // Fully qualified type name including assembly name
            
            // Load the assembly from the DLL file explicitly
            Assembly assembly = Assembly.LoadFrom("MovieLibrary.dll");

            // Get the type using the full type name and assembly
            Type businessLayerType = assembly.GetType(className);
            var businessLayer = Activator.CreateInstance(businessLayerType);

            // Add a movie to the data store (assuming a movie with Id 1 exists)
            MethodInfo addMovieMethod = businessLayerType.GetMethod("AddMovie");
            var movie = new Movie
            {
                Id = 1,
                Title = "Sample Movie",
                Genre = "Action",
                Year = 2021
            };
            addMovieMethod.Invoke(businessLayer, new object[] { movie });

            // Edit the movie title
            var editedMovie = new Movie
            {
                Id = 1,
                Title = "Updated Movie",
                Genre = "Action",
                Year = 2021
            };

            // Act
            MethodInfo editMovieMethod = businessLayerType.GetMethod("EditMovie");
            editMovieMethod.Invoke(businessLayer, new object[] { editedMovie });

            // Assert
            MethodInfo getAllMoviesMethod = businessLayerType.GetMethod("GetAllMovies");
            var allMovies = (List<Movie>)getAllMoviesMethod.Invoke(businessLayer, null);

            var updatedMovie = allMovies.Find(m => m.Id == 1);
            Assert.IsNotNull(updatedMovie);
            Assert.AreEqual(editedMovie.Title, updatedMovie.Title);
        }

        [Test]
        public void DeleteMovie_ExistingMovie_SuccessfullyDeleted()
        {
            string className = "BusinessLayer"; // Fully qualified type name including assembly name
            
            // Load the assembly from the DLL file explicitly
            Assembly assembly = Assembly.LoadFrom("MovieLibrary.dll");

            // Get the type using the full type name and assembly
            Type businessLayerType = assembly.GetType(className);
            var businessLayer = Activator.CreateInstance(businessLayerType);

            // Add a movie to the data store (assuming a movie with Id 1 exists)
            MethodInfo addMovieMethod = businessLayerType.GetMethod("AddMovie");
            var movie = new Movie
            {
                Id = 1,
                Title = "Sample Movie",
                Genre = "Action",
                Year = 2021
            };
            addMovieMethod.Invoke(businessLayer, new object[] { movie });

            // Act
            MethodInfo deleteMovieMethod = businessLayerType.GetMethod("DeleteMovie");
            deleteMovieMethod.Invoke(businessLayer, new object[] { 1 });

            // Assert
            MethodInfo getAllMoviesMethod = businessLayerType.GetMethod("GetAllMovies");
            var allMovies = (List<Movie>)getAllMoviesMethod.Invoke(businessLayer, null);

            Assert.IsFalse(allMovies.Exists(m => m.Id == 1));
        }
        
       [Test]
        public void CheckDataAccessLayerClassExistence()
        {
            string className = "DataAccessLayer"; // Fully qualified type name without assembly name

            // Load the assembly from the DLL file explicitly
            Assembly assembly = Assembly.LoadFrom("MovieLibrary.dll");

            // Get the type using the full type name and assembly
            Type dataAccessLayerType = assembly.GetType(className);

            if (dataAccessLayerType != null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();            
            }
        }

        [Test]
        public void CheckBusinessLayerClassExistence()
        {
            string className = "BusinessLayer"; // Fully qualified type name without assembly name

            // Load the assembly from the DLL file explicitly
            Assembly assembly = Assembly.LoadFrom("MovieLibrary.dll");

            // Get the type using the full type name and assembly
            Type businessLayerType = assembly.GetType(className);

            if (businessLayerType != null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();            
            }
        }

    
}
